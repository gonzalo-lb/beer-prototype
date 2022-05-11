using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Olla : ObjectPickable
{
    // Referencias a otros objetos y sus variables
    [Header("REFERENCES")]
    [SerializeField] PurificadorLP purificadorLP;
    float purificadorLPFillRate;
    float purificadorLPTemperaturaDelAgua;
    [SerializeField] Object_Olla_IndicadorTemperatura indicadorDeTemperatura;

    // Variables para abrir y cerrar la válvula
    public Animator animator;
    bool open;
    bool onUseDisabled;
    bool valvulaAbierta;
    bool onVaciadoStayCRWorking;

    //Agua
    [SerializeField] BottleControlCenterPivot bottleControl;    

    [Header("WATER AND FILL SETTINGS")]
    [Tooltip("Capacidad, en mililitros.")]
    [SerializeField] float capacidadDeLaOlla;

    float ollaEmptyRate;

    [Tooltip("Cantidad de agua que hay en la olla al iniciar el objeto, en mililitros. Si es menor a 0, o mayor a la capacidad de la olla, automáticamente se rectifica.")]
    [SerializeField] float cantidadInicialDeAgua;
    [Tooltip("Si la olla inicia vacía, la temperatura pasa a ser 0.")]
    [SerializeField] float temperaturaInicialDelAgua;
    [Tooltip("Es la temperatura del contenido de la olla, en ºC")]
    [SerializeField] float temperatura;
    
    public override void _OnStart()
    {
        base._OnStart();

        // Override offset Holding Position & yOffsetOnRelease
        offsetPositionHolding = new Vector3(0, -0.29f, 0.288f);
        yOffsetOnRelease = 0.2f;

        // Override de otras variables
        capacidadDeLaOlla = 30000f;
        ollaEmptyRate = 1f / 10f; // Tarda 10 segundos en vaciarse
        cantidadInicialDeAgua = 0f;

        // Hace Clamp en las variables que tienen límites
        if(capacidadDeLaOlla <= 0) { Debug.LogWarning("La capacidad de la olla se estableció en un valor menor o igual a 0."); }
        if(cantidadInicialDeAgua < 0 || cantidadInicialDeAgua > capacidadDeLaOlla)
        {
            cantidadInicialDeAgua = Mathf.Clamp(cantidadInicialDeAgua, 0f, capacidadDeLaOlla);
            Debug.LogWarning("La cantidad inicial de agua está por fuera de su capacidad. Fue sobreescrita.");
        }
        if(cantidadInicialDeAgua <= 0f) { temperaturaInicialDelAgua = 0f; }
        temperatura = temperaturaInicialDelAgua;
        
        // Cachea información sobre el purificador
        purificadorLPFillRate = purificadorLP._GetFillRate();
        purificadorLPTemperaturaDelAgua = purificadorLP._GetTemperaturaDelAgua();

        bottleControl._SetFillValue01(cantidadInicialDeAgua / capacidadDeLaOlla);
        _SetIndicadorDeTemperatura(temperatura);        
    }

    public override void _OnUse()
    {
        base._OnUse();

        if (onUseDisabled) { return; }

        if (open) { open = false; }
        else { open = true; }
        animator.SetBool("Abierto", open);
        StartCoroutine(DelayedValvulaStateChange());

        // Deshabilita la función mientras se abre o cierra la válvula
        StartCoroutine(DelayOnUse(1.1f));
    }

    IEnumerator DelayOnUse(float delay)
    {
        onUseDisabled = true;
        yield return new WaitForSeconds(delay);
        onUseDisabled = false;
    }

    IEnumerator DelayedValvulaStateChange()
    {
        yield return new WaitForSeconds(1f);
        valvulaAbierta = open;
        if (valvulaAbierta) { StartCoroutine(OnVaciadoStay()); }
    }

    /// <summary>
    /// Devuelve TRUE si la válvula está abierta, y FALSE si está cerrada
    /// </summary>
    /// <returns></returns>
    public bool _GetValvulaState()
    {
        return valvulaAbierta;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == TagHelper_COLLIDERS.PURIFICADORLP_WATERCOLLIDER)
        {
            Debug.Log("Olla abajo del agua");
                        
            // El FillRate del purificador se consiguió en _OnStart()            
            _AgregarAguaALaOlla(purificadorLPFillRate * Time.deltaTime, purificadorLPTemperaturaDelAgua);            
        }

        if (other.name == TagHelper_COLLIDERS.COCINA_FUEGO_COLLIDER)
        {
            Debug.Log("Olla en el fuego");

            // El TemperaturaRate de la hornalla por ahora es hard coded. Con el juego terminado 
            // hay que hacer como con el purificador, y ponerlo en el horno o en la hornalla
            _AgregarTemperaturaALaOlla(1f);
        }
    }

    /// <summary>
    /// Pasa el fillRate ingresado a valor 01, y lo agrega a la olla
    /// </summary>
    /// <param name="fillRateML">El valor sale del script del purificador, de la canilla, etc.</param>
    void _AgregarAguaALaOlla(float fillRateML, float _temperatura)
    {        
        if(_CantidadDeAguaEnOllaEnML() <= 0f) { temperatura = 15f; }
        float prevFill = _CantidadDeAguaEnOllaEnML();
        float fillRate01 = fillRateML / capacidadDeLaOlla;
        bottleControl._AddWater01(fillRate01);        

        // Calcula la nueva temperatura del agua y la plasma en el indicador        
        float newTemperatura = Library._TemperatureCalcBetween2Liquids(prevFill, temperatura, fillRateML, _temperatura);
        temperatura = newTemperatura;
        _SetIndicadorDeTemperatura(temperatura);

        if(fillRate01 >= 1)
        {
            Debug.LogWarning(temperatura);
        }
    }

    /// <summary>
    /// Agrega temperatura a la olla
    /// </summary>
    /// <param name="temperaturaRateGXS">Expresada en grados por segundo</param>
    void _AgregarTemperaturaALaOlla(float temperaturaRateGXS)
    {
        if(_CantidadDeAguaEnOllaEnML() <= 0) { return; }

        temperatura += temperaturaRateGXS * Time.deltaTime;
        temperatura = Mathf.Clamp(temperatura, 0f, 100f);
        _SetIndicadorDeTemperatura(temperatura);
    }

    float _CantidadDeAguaEnOllaEnML()
    {
        return bottleControl._GetFillValue01() * capacidadDeLaOlla;
    }

    IEnumerator OnVaciadoStay()
    {
        if (onVaciadoStayCRWorking) { yield break; }

        onVaciadoStayCRWorking = true;

        while (valvulaAbierta)
        {
            bottleControl._SubtractWater01(ollaEmptyRate * Time.deltaTime);
            yield return null;
        }

        onVaciadoStayCRWorking = false;
    }

    void _SetIndicadorDeTemperatura(float _temperatura)
    {
        indicadorDeTemperatura._UpdateText(_temperatura.ToString("F1"));
    }

    //void _CalcularTemperaturaDelAgua(float _temperaturaDeAguaQueSeAgrega, float _cantidadDeAguaQueSeAgrega)
    //{
    //    float newTemperatura = Library._TemperatureCalcBetween2Liquids(_CantidadDeAguaEnOllaEnML(), temperatura, _cantidadDeAguaQueSeAgrega, _temperaturaDeAguaQueSeAgrega);
    //    temperatura = newTemperatura;
    //    _SetIndicadorDeTemperatura(temperatura);
    //}
}
