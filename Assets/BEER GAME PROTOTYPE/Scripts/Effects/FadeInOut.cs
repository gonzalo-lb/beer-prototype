using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Se requiere que el material que tiene el GameObject sea Transparent, u otro que utilice el Alpha

// Solamente opera a través de los métodos FadeIn() FadeOut() y FadeIn_Out() (están ordenados por regiones)

public class FadeInOut : MonoBehaviour
{
    [Header("Fade Speed")]
    [Tooltip("In seconds.")]
    [SerializeField] protected float fadeSpeed = 1;    
    
    [Header("Alpha Clamp")]
    [Tooltip("Should be a value between 0 and 1.")]
    [SerializeField] protected float minAlpha = 0;

    [Tooltip("Should be a value between 0 and 1.")]
    [SerializeField] protected float maxAlpha = 1;

    [Header("DEBUG")]
    [Tooltip("Shown in Inspector only for debug purposes.")]
    [SerializeField] protected bool coRoutineWorking;

    Renderer renderComponent;

    [Tooltip("Shown in Inspector only for debug purposes.")]
    [SerializeField] Coroutine currentCoroutine;

    void Awake()
    {
        renderComponent = GetComponent<Renderer>();        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) { _FadeOut_Object(); }
        if (Input.GetKeyDown(KeyCode.D)) { _FadeIn_Object(); }
        if (Input.GetKeyDown(KeyCode.S)) { _FadeIn_Out_Object(); }
    }

    #region Coroutines
    protected IEnumerator FadeIn_Coroutine()
    {
        coRoutineWorking = true;

        do
        {
            Color objectColor = renderComponent.material.color;
            float fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

            fadeAmount = Mathf.Clamp(fadeAmount, minAlpha, maxAlpha);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            renderComponent.material.color = objectColor;
            yield return null;
        } while (renderComponent.material.color.a < maxAlpha);

        coRoutineWorking = false;
    }

    protected IEnumerator FadeOut_Coroutine()
    {
        coRoutineWorking = true;

        do
        {
            Color objectColor = renderComponent.material.color;
            float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            fadeAmount = Mathf.Clamp(fadeAmount, minAlpha, maxAlpha);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            renderComponent.material.color = objectColor;
            yield return null;
        } while (renderComponent.material.color.a > minAlpha);

        coRoutineWorking = false;
    }

    protected IEnumerator FadeIn_Out_Coroutine()
    {
        coRoutineWorking = true;

        bool addAlpha = true;

        while (coRoutineWorking)        
        {
            if (addAlpha)
            {
                Color objectColor = renderComponent.material.color;
                float fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                fadeAmount = Mathf.Clamp(fadeAmount, minAlpha, maxAlpha);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                renderComponent.material.color = objectColor;

                if (renderComponent.material.color.a >= maxAlpha) { addAlpha = false; }

                yield return null;
            }
            else
            {
                Color objectColor = renderComponent.material.color;
                float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                fadeAmount = Mathf.Clamp(fadeAmount, minAlpha, maxAlpha);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                renderComponent.material.color = objectColor;

                if (renderComponent.material.color.a <= minAlpha) { addAlpha = true; }

                yield return null;
            }
        }
    }
    #endregion

    #region Value Set Methods
    public void _SetFadeSpeedValue(float newSpeed)
    {
        fadeSpeed = newSpeed;
    }

    public void _SetMinAlphaValue(float newSpeed)
    {
        minAlpha = newSpeed;
    }
    public void _SetMaxAlphaValue(float newSpeed)
    {
        maxAlpha = newSpeed;
    }
    #endregion

    #region Main Functionality Methods
    public void _FadeIn_Object()
    {
        _StopFadeEffect();
        currentCoroutine = StartCoroutine(FadeIn_Coroutine());
    }

    public void _FadeOut_Object()
    {
        _StopFadeEffect();
        currentCoroutine = StartCoroutine(FadeOut_Coroutine());
    }

    public void _FadeIn_Out_Object()
    {
        _StopFadeEffect();
        currentCoroutine = StartCoroutine(FadeIn_Out_Coroutine());        
    }

    public void _StopFadeEffect()
    {
        if (currentCoroutine != null) { StopCoroutine(currentCoroutine); }
        coRoutineWorking = false;
    }

    public void _AlphaTo1()
    {
        _StopFadeEffect();
        Color objectColor = renderComponent.material.color;
        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, 1);
        renderComponent.material.color = objectColor;
    }

    public void _AlphaTo0()
    {
        _StopFadeEffect();
        Color objectColor = renderComponent.material.color;
        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, 0);
        renderComponent.material.color = objectColor;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newValue">Debería ser entre 0 y 1</param>
    public void _AlphaToNewValue(float newValue)
    {
        _StopFadeEffect();
        Color objectColor = renderComponent.material.color;
        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, newValue);
        renderComponent.material.color = objectColor;
    }
    #endregion
}
