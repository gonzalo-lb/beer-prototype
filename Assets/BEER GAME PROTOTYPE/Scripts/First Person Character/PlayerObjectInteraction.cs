using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerObjectInteraction : MonoBehaviour
{
    [SerializeField] Camera cam;
    Ray ray;
    RaycastHit hit;
    
    [SerializeField] float rayMaxDistance;
    [SerializeField] LayerMask layerMask;

    AudioSource audioSource;

    [SerializeField] Text infoText;

    Touch touch;
    //bool touchDisabled;

    // Debug
    bool mouseInputAllowed;

    // Pickup variables
    Transform Holder;
    [SerializeField] Transform HolderFrontReference;
    Transform originalParent;
    GameObject objectDetected;
    ObjectPickable objectDetected_ObjectInteractableComponent;
    ObjectHolder objectDetected_ObjectHolderComponent;
    ObjectInteractable_Simple objectDetected_SimpleInteractableComponent;
    GameObject objectPickedUp;
    bool playerIsHoldingAnObject;
    [SerializeField] Button_Use_PickUp buttonUsePickup;

    // Start is called before the first frame update
    void Start()
    {
        Holder = GameObject.Find(TagHelper_GAMEOBJECTS.OBJECT_HOLDER).transform;
        //HolderFrontReference = Holder.Find(TagHelper_GAMEOBJECTS.HOLDER_FRONT_REFERENCE).transform;
        audioSource = GetComponent<AudioSource>();
        rayMaxDistance = 2f;

        if (cam == null)
        {
            cam = FindObjectOfType<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) { mouseInputAllowed = true; }

        GetTouchInputAndShootRay();
        DEBUG_GetMouseInput();
        _UpdatePickedUpObjectRotation();
    }

    void GetTouchInputAndShootRay()
    {
        if (DataManager.DM_GENERALVALUES_currentGameState != GAMESTATE.PLAYING) { return; }

        //if (touchDisabled) { return; }

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {                
                ShootRay();
            }

            //touchDisabled = true;
            //StartCoroutine(SmithTrigger());
        }        
    }

    //IEnumerator SmithTrigger(float delay)
    //{
    //    touchDisabled = true;
    //    yield return new WaitForSeconds(delay);
    //    touchDisabled = false;
    //}

    //IEnumerator SmithTrigger()
    //{
    //    touchDisabled = true;
    //    while (Input.touchCount > 0)
    //    {
    //        yield return null;
    //    }
    //    touchDisabled = false;
    //}



    /// <summary>
    /// Tuve que deshabilitar esto en el juego porque toma al touch como un GetMouseButtonDown(0) 
    /// y me hacía un doble click cada vez que tocaba la pantalla en el celular
    /// </summary>
    void DEBUG_GetMouseInput()
    {
        if (DataManager.DM_GENERALVALUES_currentGameState != GAMESTATE.PLAYING) { return; }

        if (!mouseInputAllowed) { return; }

        if (Input.GetMouseButtonDown(0)) //Se fija si se hizo click izquierdo. Se usa para movimientos
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                infoText.text = "UI";
                return;
            }

            ray = cam.ScreenPointToRay(Input.mousePosition); //Define dónde se hizo click en un Vector3 (Ray)            
            if (Physics.Raycast(ray, out hit, rayMaxDistance, layerMask))
            {
                GetTagAndInteract();
            }            
        }
    }
    

    // Touch position tiene que tener z en 0, y XY entre 0 y 1
    void ShootRay()
    {
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            infoText.text = "UI";
            return;
        }

        ray = cam.ScreenPointToRay(touch.position);        
        //RaycastHit hit;        
        if (Physics.Raycast(ray, out hit, rayMaxDistance, layerMask))
        {
            GetTagAndInteract();
        }        
    }

    void GetTagAndInteract()
    {
        objectDetected = hit.collider.gameObject;
        infoText.text = objectDetected.name;

        //objectDetected.TryGetComponent(typeof(ObjectHolder), out objectDetected_ObjectHolderComponent)
        if (objectDetected.tag == TagHelper_TAGS.OBJECT_PICKABLE) // Si el objeto es "Pickable"
        {
            objectDetected_ObjectInteractableComponent = objectDetected.GetComponent<ObjectPickable>();
            if (buttonUsePickup._IsUsePickUpState())
            {
                PickUpObject(objectDetected);
            }
            else
            {
                objectDetected_ObjectInteractableComponent._OnUse();
            }
            
        }        
        else if (objectDetected.tag == TagHelper_TAGS.OBJECT_HOLDER) // Si el objeto es "Object Holder"
        {
            objectDetected_ObjectHolderComponent = objectDetected.GetComponent<ObjectHolder>();

            if (objectDetected_ObjectHolderComponent._HasObjectReleasePosition())
            { ReleaseObject(objectDetected_ObjectHolderComponent._GetObjectReleasePosition()); }
            else { ReleaseObject(hit.point); }
        }
        else if(objectDetected.tag == TagHelper_TAGS.OBJECT_INTERACTABLE_SIMPLE) // Si es un "Simple Interactable"
        {
            objectDetected_SimpleInteractableComponent = objectDetected.GetComponent<ObjectInteractable_Simple>();
            objectDetected_SimpleInteractableComponent._OnClick();
            audioSource.Play();
        }
    }

    /// <summary>
    /// Si logró agarrar al objeto, retorna True
    /// </summary>
    /// <param name="gameObjectToPick"></param>
    /// <returns></returns>
    bool PickUpObject(GameObject gameObjectToPick)
    {
        if (playerIsHoldingAnObject) { return false; }

        objectPickedUp = gameObjectToPick;
        originalParent = objectPickedUp.transform.parent;
        objectPickedUp.transform.parent = Holder;
        objectPickedUp.transform.localPosition = objectDetected_ObjectInteractableComponent._OffsetPositionHolding();

        //HolderFrontReference.position = objectPickedUp.transform.position;
        //HolderFrontReference.Translate(Vector3.forward, Space.Self);
        //objectPickedUp.transform.LookAt(HolderFrontReference);

        float rotX = objectDetected_ObjectInteractableComponent._OffsetRotationX_Holding();
        float rotY = objectDetected_ObjectInteractableComponent._OffsetRotationY_Holding();
        float rotZ = objectDetected_ObjectInteractableComponent._OffsetRotationZ_Holding();
        objectPickedUp.transform.localRotation = Quaternion.Euler(rotX, rotY, rotZ);
        objectDetected_ObjectInteractableComponent._Set_AlwaysVisible_ON();
        objectDetected_ObjectInteractableComponent._Set_CastShadows_OFF();
        playerIsHoldingAnObject = true;
        objectDetected_ObjectInteractableComponent._OnPickUp();
        audioSource.Play();
        return true;

    }

    void ReleaseObject(Vector3 positionOnRelease)
    {
        if(!playerIsHoldingAnObject) { return; }

        // Aplica el offset en el eje Y, a la posición en la que será soltado el objeto
        positionOnRelease.y += objectDetected_ObjectInteractableComponent._YOffsetOnRelease();

        // Lo vuelve al parent en el que estaba, y resetea el local position y rotation
        objectPickedUp.transform.parent = originalParent;
        objectPickedUp.transform.localPosition = Vector3.zero;
        objectPickedUp.transform.localRotation = Quaternion.Euler(0, 0, 0);

        // Establece la posición
        objectPickedUp.transform.position = positionOnRelease;

        // Establece la rotación, mirando al player pero sin inclinación
        objectPickedUp.transform.LookAt(transform.position);
        Vector3 tempRot = objectPickedUp.transform.rotation.eulerAngles;
        tempRot.x = 0;
        tempRot.z = 0;
        objectPickedUp.transform.rotation = Quaternion.Euler(tempRot);

        objectDetected_ObjectInteractableComponent._Set_AlwaysVisible_OFF();
        objectDetected_ObjectHolderComponent._OnObjectRelease();
        audioSource.Play();

        objectPickedUp = null;
        playerIsHoldingAnObject = false;
    }

    void _UpdatePickedUpObjectRotation()
    {
        if(!playerIsHoldingAnObject) { return; }

        //Quaternion from = objectPickedUp.transform.rotation;
        //Vector3 toAsVector3 = HolderFrontReference.transform.position - objectPickedUp.transform.position;
        //Quaternion to = Quaternion.LookRotation(toAsVector3, Vector3.up);
        //objectPickedUp.transform.rotation = Quaternion.Slerp(from, to, 0.5f);

        Vector3 tempRot = objectPickedUp.transform.rotation.eulerAngles;
        tempRot.x = 0;
        objectPickedUp.transform.rotation = Quaternion.Euler(tempRot);
    }
}
