using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurificadorLP : ObjectInteractable_Simple
{
    [SerializeField] AguaCanilla_ParticleSystem particleSystemAguaCanilla;
    [SerializeField] GameObject waterCollider;

    [Header("VALORES DEL LIQUIDO QUE SALE DEL PURIFICADOR")]
    [Tooltip("En ml/sec")]
    [SerializeField] float fillRateML;

    [Tooltip("Es la temperatura del agua que sale del purificador, en ºC")]
    [SerializeField] float temperaturaDelAgua = 15f;
    [SerializeField] Color colorDelAgua;

    Liquid fillRate_Liquid;

    Coroutine activeCoroutine;

    private void Awake()
    {
        fillRate_Liquid = new Liquid();

        if(fillRateML <= 0) { fillRate_Liquid._volumen = 30000f / 20f; } // Equivale a 30 litros cada 20 segundos
        else { fillRate_Liquid._volumen = fillRateML; }

        if (temperaturaDelAgua <= 0) { fillRate_Liquid._temperatura = 15f; }
        else { fillRate_Liquid._temperatura = temperaturaDelAgua; }

        fillRate_Liquid._densidad = 1000f;

        fillRate_Liquid._color = colorDelAgua;
    }

    public override void _OnClick()
    {
        base._OnClick();
        _StopOutlineFlashing();
        if (!particleSystemAguaCanilla._IsParticleSystemEmissionEnabled())
        {
            particleSystemAguaCanilla._EnableEmissionParticleSystem();
            
            if(activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
                activeCoroutine = StartCoroutine(WaterColliderEnabledDelayed(1f));
            }
            else { activeCoroutine = StartCoroutine(WaterColliderEnabledDelayed(1f)); }            
                        
            Debug.Log("Intentando habilitar particle system");
        }
        else
        {
            particleSystemAguaCanilla._DisableEmissionParticleSystem();

            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
                activeCoroutine = StartCoroutine(WaterColliderDisabledDelayed(1f));
            }
            else { activeCoroutine = StartCoroutine(WaterColliderEnabledDelayed(1f)); }

            Debug.Log("Intentando des-habilitar particle system");
        }
    }

    IEnumerator WaterColliderEnabledDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        waterCollider.SetActive(true);
    }

    IEnumerator WaterColliderDisabledDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        waterCollider.SetActive(false);
    }

    public float _GetFillRate()
    {
        return fillRateML;
    }

    public float _GetTemperaturaDelAgua()
    {
        return temperaturaDelAgua;
    }

    public Liquid _GetFillRate_Liquid()
    {
        return fillRate_Liquid;
    }
}
