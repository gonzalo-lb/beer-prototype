using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickable : MonoBehaviour, IObjectPickable
{
    // Outline and Flashing Variables
    [Header("OUTLINE")]
    [SerializeField] protected Outline outline;
    protected bool outlineFlashing = false;
    [SerializeField] protected bool outlineOnAtStart = false;
    [SerializeField] protected float objectFlashingThreshold = 0.2f;
    protected Color outlineBaseColor;

    // Object Settings
    [Header("OFFSET SETTINGS")]
    [SerializeField] protected bool isCenterPivot;
    protected Collider collider;
    [SerializeField] protected Vector3 offsetPositionHolding;
    [SerializeField] protected bool calculateYOffsetOnRelease = true;
    [SerializeField] protected float yOffsetOnRelease;
    [SerializeField] protected Vector3 offsetRotationHolding;    

    // Always Visible
    protected int originalLayer;
    [Header("DEBUG - BASE CLASS")]
    [SerializeField] protected GameObject[] thisGameObjectChildren;    

    #region MonoBehaviour and Start Methods

    // Start is called before the first frame update
    void Start()
    {
        _OnStart();        
    }

    public virtual void _OnStart()
    {
        gameObject.tag = TagHelper_TAGS.OBJECT_PICKABLE;
        gameObject.layer = TagHelper_LAYERS.OBJECT_PICKABLE;
        SetVariables();
        GetOrCreateOulineComponent();

        if (isCenterPivot)
        {
            collider = GetComponent<Collider>();
            if (calculateYOffsetOnRelease) { yOffsetOnRelease = collider.bounds.size.y / 2; }            
        }
    }
        
    protected void SetVariables()
    {
        outlineBaseColor = new Color(241f / 255f, 24f / 255f, 24f / 255f);

        if(thisGameObjectChildren.Length == 0)
        {
            thisGameObjectChildren = Library._GetChildrensGameObject(this.gameObject);
        }        

        originalLayer = this.gameObject.layer;
        
    }

    #endregion
    
    #region Virtual Methods

    /// <summary>
    /// Comportamiento del objeto cuando el jugador le haga click, por fuera de "Is pickable", cuyo 
    /// funcionamiento es independiente de este método.
    /// </summary>
    public virtual void _OnPickUp()
    {

    }

    public virtual void _OnRelease()
    {

    }

    public virtual void _OnUse()
    {

    }
    #endregion    

    #region AlwaysVisible

    public void _Set_AlwaysVisible_ON()
    {
        this.gameObject.layer = TagHelper_LAYERS.ALWAYS_VISIBLE;

        if(thisGameObjectChildren == null) { return; }

        for (int i = 0; i < thisGameObjectChildren.Length; i++)
        {
            thisGameObjectChildren[i].layer = TagHelper_LAYERS.ALWAYS_VISIBLE;
        }
    }

    public void _Set_AlwaysVisible_OFF()
    {
        this.gameObject.layer = originalLayer;

        if (thisGameObjectChildren == null) { return; }

        for (int i = 0; i < thisGameObjectChildren.Length; i++)
        {
            // Como todos los children tendrían el mismo layer, todos vuelven al mismo
            thisGameObjectChildren[i].layer = originalLayer;
        }
    }

    public void _Set_CastShadows_OFF()
    {
        MeshRenderer meshRenderer;
        if (this.gameObject.TryGetComponent(out meshRenderer))
        {
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        if (thisGameObjectChildren == null) { return; }

        for (int i = 0; i < thisGameObjectChildren.Length; i++)
        {
            if (thisGameObjectChildren[i].TryGetComponent(out meshRenderer))
            {
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }
    }

    public void _Set_CastShadows_ON()
    {
        MeshRenderer meshRenderer;
        if (this.gameObject.TryGetComponent(out meshRenderer))
        {
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }

        if (thisGameObjectChildren == null) { return; }

        for (int i = 0; i < thisGameObjectChildren.Length; i++)
        {
            if(thisGameObjectChildren[i].TryGetComponent(out meshRenderer))
            {
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }
    }

    #endregion

    #region Offsets

    public Vector3 _OffsetPositionHolding()
    {
        return offsetPositionHolding;
    }

    public float _YOffsetOnRelease()
    {
        return yOffsetOnRelease;
    }

    public float _OffsetRotationX_Holding()
    {
        return offsetRotationHolding.x;
    }

    public float _OffsetRotationY_Holding()
    {
        return offsetRotationHolding.y;
    }

    public float _OffsetRotationZ_Holding()
    {
        return offsetRotationHolding.z;
    }
    #endregion

    #region Outline And Flashing

    protected void GetOrCreateOulineComponent()
    {
        outline = GetComponent<Outline>();

        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();

            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = outlineBaseColor;            
            outline.OutlineWidth = 6.2f;

            outline.enabled = false;
        }

        if (outlineOnAtStart) { outline.enabled = true; }
        else { outline.enabled = false; }

    }

    protected IEnumerator ObjectFlashing()
    {
        bool itsOff = false;
        while (outlineFlashing)
        {
            if (itsOff)
            {
                outline.enabled = true;
                itsOff = false;
            }
            else
            {
                outline.enabled = false;
                itsOff = true;
            }
            yield return new WaitForSeconds(objectFlashingThreshold);
        }
        outline.enabled = false;
    }

    public void _StartOutlineFlashing()
    {
        if (outlineFlashing)
        {
            Debug.LogWarning("Se está intentando iniciar nuevamente un Flashing en curso");
            return;
        }

        outlineFlashing = true;
        StartCoroutine(ObjectFlashing());                
    }

    public void _StopOutlineFlashing()
    {        
        if (!outlineFlashing)
        {
            Debug.LogWarning("El Flashing que se intenta deshabilitar ya estaba deshabilitado");
            return;
        }

        outlineFlashing = false;        
    }

    public void _EnableOutline()
    {
        outline.enabled = true;
    }

    public void _DisableOutline()
    {
        outline.enabled = false;
    }

    #endregion


}
