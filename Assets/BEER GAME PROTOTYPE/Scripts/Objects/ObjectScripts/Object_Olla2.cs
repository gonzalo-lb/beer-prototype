using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Olla2 : ObjectPickable
{
    LiquidHolder liquidHolder;

    // Valores iniciales
    [Header("VALORES INICIALES DE LA OLLA")]
    [Tooltip("Ingresar valor en mili litros.")]
    [SerializeField] float capacidadDeLaOlla = 30000f;
    [Tooltip("Cantidad de líquido que hay en la olla, en mili litros.")]
    [SerializeField] float volumenAlIniciar;    
    [Tooltip("Densidad del líquido con el que inicia la olla.")]
    [SerializeField] float densidadAlIniciar;
    [Tooltip("Temperatura del líquido con el que inicia la olla, en grados centígrados.")]
    [SerializeField] float temperaturaAlIniciar;
    [Tooltip("Color del líquido inicial.")]
    [SerializeField] Color colorAlIniciar;
    [Tooltip("Cantidad de segundos que tarda en vaciarse la olla cuando la válvula está abierta.")]
    [SerializeField] float emptyRate = 10f;

    // Referencias a otros objetos y sus variables
    [Header("REFERENCIAS")]
    [SerializeField] PurificadorLP purificadorLP;
    Liquid purificadorLPLiquid;
    [SerializeField] Object_Olla_IndicadorTemperatura indicadorDeTemperatura;

    // Variables para abrir y cerrar la válvula
    [Header("ANIMATOR")]
    public Animator animator;
    bool open;
    bool onUseDisabled;
    bool valvulaAbierta;
    bool onVaciadoStayCRWorking;

    #region DEBUG VARIABLES

    [Header("DEBUG - READ ONLY")]
    [SerializeField] float __volumen;    
    [SerializeField] float __temperatura;
    [SerializeField] float __densidad;
    [SerializeField] float __masaDeAzucar;
    [SerializeField] float __masaDeAgua;
    [SerializeField] float __masaDeTotal;
    [SerializeField] Color __color;
    [SerializeField] float __lupulo;
    [SerializeField] float __grano;
    [SerializeField] float __granoMacerado;
    

    [Header("DEBUG - MOSTO")]
    [SerializeField] float mostoVolumen = 500f;
    [SerializeField] float mostoTemperatura = 55f;
    [SerializeField] float mostoDensidad = 1010f;

    #endregion

    // Lo que sea que en un futuro controle al mesh del agua

    // Lo que sea que en un futuro controle al mesh del indicador de temperatura

    private void Awake()
    {
        liquidHolder = new LiquidHolder(capacidadDeLaOlla, volumenAlIniciar, densidadAlIniciar, temperaturaAlIniciar, colorAlIniciar);
    }

    private void Update()
    {
        DEBUGMETHOD();
    }

    void DEBUGMETHOD()
    {
        __volumen = liquidHolder._GetVolumenDeLiquido();
        __temperatura = liquidHolder._GetTemperatura();
        __densidad = liquidHolder._GetDensidad();
        __masaDeAzucar = liquidHolder._GetMasaDeAzucar();
        __masaDeAgua = liquidHolder._GetMasaDeAgua();
        __masaDeTotal = liquidHolder._GetMasaTotal();
        __lupulo = liquidHolder._GetLupulo();
        __grano = liquidHolder._GetGrano();
        __granoMacerado = liquidHolder._GetGranoMacerado();
        __color = liquidHolder._GetColor();
    }

    public override void _OnStart()
    {
        base._OnStart();

        // Override offset Holding Position & yOffsetOnRelease
        offsetPositionHolding = new Vector3(0, -0.29f, 0.288f);
        yOffsetOnRelease = 0.2f;

        // Setea las variables del liquid holder
        _CheckOnStartValues();
        liquidHolder._START_SET_METHOD(capacidadDeLaOlla, volumenAlIniciar, densidadAlIniciar, temperaturaAlIniciar, colorAlIniciar); // La olla arranca vacía        

        // Override offset Holding Position & yOffsetOnRelease
        // ...

        // Cachea información sobre el purificador
        purificadorLPLiquid = purificadorLP._GetFillRate_Liquid();

        // Envía información al script que maneje el mesh del agua en función del seteo ya realizado
        // ...

        // Envía información al script del indicador de temperatura
        _SetIndicadorDeTemperatura(liquidHolder._GetTemperatura());

        
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

    IEnumerator OnVaciadoStay()
    {
        if (onVaciadoStayCRWorking) { yield break; }

        onVaciadoStayCRWorking = true;

        while (valvulaAbierta)
        {
            _RestarLiquido(emptyRate * Time.deltaTime);            
            yield return null;
        }

        onVaciadoStayCRWorking = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == TagHelper_COLLIDERS.PURIFICADORLP_WATERCOLLIDER)
        {
            Debug.Log("Olla abajo del agua");

            // El FillRate del purificador se consiguió en _OnStart()            
            _AgregarLiquido(purificadorLPLiquid._WithVolumeAndMasaAsDeltaTime());
        }

        if (other.name == TagHelper_COLLIDERS.COCINA_FUEGO_COLLIDER)
        {
            Debug.Log("Olla en el fuego");

            // El TemperaturaRate de la hornalla por ahora es hard coded. Con el juego terminado 
            // hay que hacer como con el purificador, y ponerlo en el horno o en la hornalla

            _AgregarTemperatura(1f * Time.deltaTime);
        }
        
        if (other.name == "Mosto")
        {
            Debug.Log("Olla en el mosto");

            Liquid mosto = new Liquid();

            mosto._volumen = __volumen;
            mosto._masaDeAzucar = __masaDeAzucar;
            mosto._temperatura = mostoTemperatura;
            mosto._densidad = mostoDensidad;

            _AgregarLiquido(mosto._WithVolumeAndMasaAsDeltaTime());
        }
    }

    #region Agregar-Quitar Liquido o temperatura y actualizar indicadores o UI

    void _AgregarLiquido(Liquid liquid)
    {
        liquidHolder._AgregarLiquido(liquid);

        // Actualizar mesh del líquido, indicador de temperatura, etc
        _SetIndicadorDeTemperatura(liquidHolder._GetTemperatura());
        Debug.Log("Volumen de líquido = " + liquidHolder._GetVolumenDeLiquido());
    }

    void _RestarLiquido(float _volumenARestar)
    {
        liquidHolder._RestarLiquido(_volumenARestar);

        // Actualizar mesh del líquido, indicador de temperatura, etc
        _SetIndicadorDeTemperatura(liquidHolder._GetTemperatura());
        Debug.Log("Volumen de líquido = " + liquidHolder._GetVolumenDeLiquido());
    }

    void _AgregarTemperatura(float _temperatura)
    {
        liquidHolder._AgregarTemperatura(_temperatura);

        // Actualizar mesh del líquido, indicador de temperatura, etc
        _SetIndicadorDeTemperatura(liquidHolder._GetTemperatura());
    }

    void _RestarTemperatura(float _temperatura)
    {
        liquidHolder._RestarTemperatura(_temperatura);

        // Actualizar mesh del líquido, indicador de temperatura, etc
        _SetIndicadorDeTemperatura(liquidHolder._GetTemperatura());
    }

    void _SetIndicadorDeTemperatura(float _temperatura)
    {
        indicadorDeTemperatura._UpdateText(_temperatura.ToString("F1"));
    }

    #endregion

    #region Sub-Metodos

    void _CheckOnStartValues()
    {
        // Capacidad de la olla
        if (capacidadDeLaOlla <= 0)
        {
            Debug.LogWarning("Como la variable 'capacidadDeLaOlla contiene un valor menor o igual a 0, se setea en 30 litros'");
            capacidadDeLaOlla = 30000f;
        }

        // Volumen inicial
        if(volumenAlIniciar < 0)
        {
            Debug.LogWarning("Como la variable 'volumenInicial contiene un valor menor a 0, se setea en 0 litros'");
            volumenAlIniciar = 0;
        }
        else if(volumenAlIniciar > capacidadDeLaOlla)
        {
            Debug.LogWarning("El volumen inicial es mayor a la capacidad de la olla. Se setea en el máximo.");
            volumenAlIniciar = capacidadDeLaOlla;
        }              

        // Densidad
        if(densidadAlIniciar < 0)
        {
            Debug.LogWarning("La variable densidad no puede ser inferior a 0. Se setea en 0.");
            densidadAlIniciar = 0;
        }

        // Temperatura
        if(temperaturaAlIniciar < 0)
        {
            Debug.LogWarning("La variable temperatura no puede ser inferior a 0. Se setea en 0.");
            temperaturaAlIniciar = 0;
        }
        else if (temperaturaAlIniciar > 100f)
        {
            Debug.LogWarning("La variable temperatura no puede ser mayor a 100. Se setea en 100.");
            temperaturaAlIniciar = 100f;
        }

        // Empty Rate
        if(emptyRate <= 0)
        {
            Debug.LogWarning("El EmptyRate no puede ser menor o igual a 0. Se setea en 10 segundos.");
            emptyRate = 10f;
        }

        emptyRate = capacidadDeLaOlla / emptyRate;       

    } // _CheckOnStartValues()

    #endregion

    #region Get Methods

    public float _GetCapacidadDelRecipiente() { return liquidHolder._GetCapacidadDelRecipiente(); }

    public float _GetTemperatura() { return liquidHolder._GetTemperatura(); }

    public float _GetVolumenDeLiquido() { return liquidHolder._GetVolumenDeLiquido(); }

    public float _GetMasaDeAzucar() { return liquidHolder._GetMasaDeAzucar(); }

    public float _GetMasaDeAgua() { return liquidHolder._GetMasaDeAgua(); }

    public float _GetMasaTotal() { return liquidHolder._GetMasaTotal(); }

    public float _GetDensidad() { return liquidHolder._GetDensidad(); }

    public float _GetAzucarExcedente() { return liquidHolder._GetAzucarExcedente(); }

    public float _GetLupulo() { return liquidHolder._GetLupulo(); }

    public float _GetGrano() { return liquidHolder._GetGrano(); }

    public float _GetGranoMacerado() { return liquidHolder._GetGranoMacerado(); }

    public float _GetPesoDelGrano() { return liquidHolder._GetPesoDelGrano(); }

    public float _GetVolumenEnPorcentaje() { return liquidHolder._GetVolumenEnPorcentaje(); }

    public Color _GetColor() { return liquidHolder._GetColor(); }

    #endregion
}
