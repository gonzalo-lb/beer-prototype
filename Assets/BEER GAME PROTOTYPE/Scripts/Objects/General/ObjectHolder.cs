using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour, IObjectHolder
{
    [Tooltip("Puede quedar vacío. En ese caso la ReleasePosition va a ser donde golpeó el Raycast.")]
    [SerializeField] protected Transform objectReleasePosition;
    
    protected bool hasObjectReleasePosition = true;

    #region Start and MonoBehaviour methods
    void Start()
    {
        OnStart();
    }

    public virtual void OnStart()
    {
        gameObject.tag = TagHelper_TAGS.OBJECT_HOLDER;
        gameObject.layer = TagHelper_LAYERS.OBJECT_HOLDER;
        if(objectReleasePosition == null) { hasObjectReleasePosition = false; }
    }    
    #endregion

    #region Virtual Methods
    /// <summary>
    /// Comportamiento del objeto cuando el jugador lo deje en el Holder
    /// </summary>
    public virtual void _OnObjectRelease()
    {

    }    
    #endregion

    #region Comunication Methods
    /// <summary>
    /// Si hay algo en el Transform "objectReleasePosition" devuelve True. 
    /// Si "objectReleasePosition" es null, devuelve False.
    /// </summary>
    /// <returns></returns>
    public bool _HasObjectReleasePosition()
    {
        return hasObjectReleasePosition;
    }

    public Vector3 _GetObjectReleasePosition()
    {
        if (hasObjectReleasePosition) { return objectReleasePosition.position; }
        else { return Vector3.zero; }
    }
    #endregion
    
}
