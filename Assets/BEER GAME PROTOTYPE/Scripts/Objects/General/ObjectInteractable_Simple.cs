using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractable_Simple : MonoBehaviour, IObjectInteractable_Simple
{
    // Outline and Flashing Variables
    [SerializeField] protected Outline outline;
    protected bool outlineFlashing = false;
    [SerializeField] protected bool outlineOnAtStart = false;
    [SerializeField] protected float objectFlashingThreshold = 0.2f;
    protected Color outlineBaseColor;

    #region Monobehaviour and Start Methods
    private void Start()
    {
        _OnStart();
    }

    public virtual void _OnStart()
    {
        gameObject.tag = TagHelper_TAGS.OBJECT_INTERACTABLE_SIMPLE;
        gameObject.layer = TagHelper_LAYERS.OBJECT_INTERACTABLE;
        SetVariables();
        GetOrCreateOulineComponent();        
    }

    protected void SetVariables()
    {
        outlineBaseColor = new Color(241f / 255f, 24f / 255f, 24f / 255f);
    }
    #endregion

    #region Virtual Methods
    public virtual void _OnClick()
    {

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
