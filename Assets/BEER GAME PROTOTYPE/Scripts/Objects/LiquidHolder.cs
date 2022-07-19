using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidHolder
{
    #region Valores

    // Valores del recipiente y del líquido
    Liquid liquidHolder_liquid;

    float _capacidadDelRecipiente; // En mililitros

    [Tooltip("En gramos.")]
    float _lupulo;

    [Tooltip("En gramos.")]
    float _grano;
    float _granoMacerado;
    float _pesoDelGrano
    {
        get
        {
            return _grano + _granoMacerado;
        }
    }

    float _volumenEnPorcentaje
    {
        get
        {
            return (liquidHolder_liquid._volumen * 100) / _capacidadDelRecipiente;
        }
    }

    float _azucarExcedente;

    #endregion

    #region Inicialización de la Class

    public LiquidHolder(float capacidadRecipiente, float volumen, float densidad, float temperatura, Color color)
    {
        liquidHolder_liquid = new Liquid();

        _START_SET_METHOD(capacidadRecipiente, volumen, densidad, temperatura, color);
    }

    #endregion

    #region Set Metodos
        
    public void _START_SET_METHOD(float capacidadRecipiente_, float volumen_, float densidad_, float temperatura_, Color color_)
    {
        // Capacidad del recipiente
        if (capacidadRecipiente_ <= 0)
        {
            _capacidadDelRecipiente = 30000;
            Debug.LogWarning("La capacidad del recipiente no puede ser igual o menor a 0. Se setea en 30.000 ml.");
        }
        else { _capacidadDelRecipiente = capacidadRecipiente_; }

        // Volumen inicial de líquido                
        _SetVolumen(volumen_);
        _SetDensidad(densidad_);

        // Temperatura
        if (volumen_ <= 0) { liquidHolder_liquid._temperatura = 0; } else { liquidHolder_liquid._temperatura = temperatura_; }

        // Color
        liquidHolder_liquid._color = color_;
    }    
    
    public void _SetVolumen(float volumen) => liquidHolder_liquid._volumen = volumen;

    public void _SetTemperatura(float temperatura) => liquidHolder_liquid._temperatura = temperatura;

    public void _SetDensidad(float densidad) => liquidHolder_liquid._densidad = densidad;

    public void _SetMasaDeAzucar(float masadeazucar) => liquidHolder_liquid._masaDeAzucar = masadeazucar;

    public void _SetColor(Color color) => liquidHolder_liquid._color = color;

    public void _SetLupulo(float lupulo) => _lupulo = lupulo;

    public void _SetGrano(float grano) => _grano = grano;

    public void _SetGranoMacerado(float granomacerado) => _granoMacerado = granomacerado;

    #endregion

    #region Get Methods

    public float _GetCapacidadDelRecipiente() { return _capacidadDelRecipiente; }

    public float _GetTemperatura() { return liquidHolder_liquid._temperatura; }

    public float _GetVolumenDeLiquido() { return liquidHolder_liquid._volumen; }

    public float _GetMasaDeAzucar() { return liquidHolder_liquid._masaDeAzucar; }

    public float _GetMasaDeAgua() { return liquidHolder_liquid._masaDeAgua; }

    public float _GetMasaTotal() { return liquidHolder_liquid._masaTotal; }

    public float _GetDensidad() { return liquidHolder_liquid._densidad; }

    public float _GetAzucarExcedente() { return _azucarExcedente; }

    public float _GetLupulo() { return _lupulo; }

    public float _GetGrano() { return _grano; }

    public float _GetGranoMacerado() { return _granoMacerado; }

    public float _GetPesoDelGrano() { return _pesoDelGrano; }

    public float _GetVolumenEnPorcentaje() { return _volumenEnPorcentaje; }

    public Color _GetColor() { return liquidHolder_liquid._color; }    

    #endregion

    #region Main Functionality

    public void _AgregarLiquido(Liquid liquido)
    {
        // Guardamos variables para calcular la nueva temperatura
        float prevVol = liquidHolder_liquid._volumen;

        // Se agrega el volumen y masa de azucar
        liquidHolder_liquid._volumen += liquido._volumen;
        liquidHolder_liquid._masaDeAzucar += liquido._masaDeAzucar;        

        // Calcular la nueva temperatura del líquido
        if (prevVol <= 0f) { liquidHolder_liquid._temperatura = liquido._temperatura; } // Si no había agua, la temperatura del líquido es la del que se agrega
        else // Si ya tenía líquido, se calcula la nueva temperatura
        {
            float newTemperatura = Library._TemperatureCalcBetween2Liquids(prevVol, liquidHolder_liquid._temperatura, liquido._volumen, liquido._temperatura);
            liquidHolder_liquid._temperatura = newTemperatura;
        }

        // Calcula la nueva capacidad máxima de soluto del mosto
        _OnTemperatureChange(liquidHolder_liquid._temperatura);

        // Si la cantidad de líquido supera a la del Holder, calcular nuevas cantidades de agua y mosto
        if (liquidHolder_liquid._volumen > _capacidadDelRecipiente)
        {
            float excedenteDeVolumen = liquidHolder_liquid._volumen - _capacidadDelRecipiente;
            float excedenteEnPorcentaje01 = excedenteDeVolumen / liquidHolder_liquid._volumen;

            // Calcula las nuevas cantidad de agua y mosto limitadas a la capacidad del recipiente
            float volumenARestar = liquidHolder_liquid._volumen * excedenteEnPorcentaje01;
            liquidHolder_liquid._volumen -= volumenARestar;

            // Calcula la nueva densidad
            float masaDeAzucarARestar = liquidHolder_liquid._masaDeAzucar * excedenteEnPorcentaje01;
            liquidHolder_liquid._masaDeAzucar -= masaDeAzucarARestar;
        }

        // Calcular nuevo color
        if (prevVol <= 0f) { liquidHolder_liquid._color = liquido._color; } // Si no había agua, el del líquido es la del que se agrega
        else // Si ya tenía líquido, se calcula el nuevo color
        {
            float _t = liquido._volumen / (prevVol + liquido._volumen); // Si esto llega a fallar, cambiar esa linea por esta: liquidHolder_liquid._color / (prevVol + liquido._volumen);
            liquidHolder_liquid._color = Color.Lerp(liquidHolder_liquid._color, liquido._color, _t);
        }
    }

    public void _RestarLiquido(float volumenARestar)
    {
        if (volumenARestar > liquidHolder_liquid._volumen)
        {
            liquidHolder_liquid._volumen = 0f;
            liquidHolder_liquid._temperatura = 0f;
            liquidHolder_liquid._masaDeAzucar = 0f;
            liquidHolder_liquid._color = Color.clear; // No estoy seguro de que esta linea esté bien
        }
        else
        {
            float porcentajeDeVolumenARestar = volumenARestar / liquidHolder_liquid._volumen;
            liquidHolder_liquid._volumen -= liquidHolder_liquid._volumen * porcentajeDeVolumenARestar;
            liquidHolder_liquid._masaDeAzucar -= liquidHolder_liquid._masaDeAzucar * porcentajeDeVolumenARestar;
        }
    }

    public void _AgregarTemperatura(float temperatura)
    {
        if (liquidHolder_liquid._volumen <= 0) { return; }
        else
        {
            liquidHolder_liquid._temperatura += temperatura;

            // Calcula la nueva capacidad máxima de soluto del mosto
            _OnTemperatureChange(liquidHolder_liquid._temperatura);
        }
    }

    public void _RestarTemperatura(float temperatura)
    {
        if (liquidHolder_liquid._volumen <= 0) { return; }
        else
        {
            liquidHolder_liquid._temperatura -= temperatura;

            // Calcula la nueva capacidad máxima de soluto del mosto
            _OnTemperatureChange(liquidHolder_liquid._temperatura);
        }
    }

    public void _AgregarGrano(float grano)
    {
        _grano += grano;
    }

    /// <summary>
    /// Convierte el grano en azucar. Durante ese proceso, el grano absorve agua. Este método al macerar disminuye la variable _grano y 
    /// la pasa a _granoMacerado.
    /// </summary>
    /// <param name="granoAMacerar"></param>
    public void _Macerar(float granoAMacerar)
    {
        // Revisa si hay algo de grano o de líquido. Si falta alguno no se puede macerar
        if(granoAMacerar <= 0) { Debug.LogWarning("_Macerar: RETURN. Grano en 0"); return; }
        if(liquidHolder_liquid._volumen <= 0) { Debug.LogWarning("_Macerar: RETURN. Líquido en 0"); return; }
        if(granoAMacerar > _grano)
        {
            Debug.LogWarning("void _Macerar(): Se intenta macerar más grano del que hay en el recipiente. Se limita la operación a la cantidad de grano que tiene el macerador.");
            granoAMacerar = _grano;
        }

        // Calcula cuánto grano va a poder macerar, en función de la cantidad de líquido que hay
        // Primer supuesto: Maceración completa. Requiere 10% más de agua que de grano
        float cantidadNecesariaDeLiquido = granoAMacerar + (granoAMacerar * 0.1f);
        if(liquidHolder_liquid._volumen >= cantidadNecesariaDeLiquido)
        {
            Debug.LogAssertion("Primer supuesto: Maceración completa.");
            _Macerar_SubMethod_Macerar(granoAMacerar);            
        }
        else
        {
            Debug.LogAssertion("Segundo supuesto: Maceración parcial.");
            float cantidadLimitadaDeGranoAMacerar = liquidHolder_liquid._volumen * 0.4f;
            _Macerar_SubMethod_Macerar(cantidadLimitadaDeGranoAMacerar);
        }

        // Calcula la nueva capacidad máxima de soluto del mosto
        _OnTemperatureChange(liquidHolder_liquid._temperatura);

        void _Macerar_SubMethod_Macerar(float cantidadDeGrano)
        {
            liquidHolder_liquid._volumen -= cantidadDeGrano;
            _grano -= cantidadDeGrano;
            _granoMacerado += cantidadDeGrano * 2f;
            liquidHolder_liquid._masaDeAzucar += cantidadDeGrano * 124f;
        }
    }
    
    /// <summary>
    /// Calcula la capacidad máxima de soluto del mosto y si hay un excedente de azucar, lo guarda en la variable _azucarExcedente. 
    /// Si, por el contrario, por la temperatura puede absorver el excedente que haya, lo absorve. 
    /// Hay que llamar este método cada vez que varía la temperatura del mosto.
    /// </summary>
    public void _OnTemperatureChange(float nuevaTemperatura)
    {
        // Si no hay líquido en el recipiente, no hace nada
        if (liquidHolder_liquid._volumen <= 0f) { return; }

        // Limita la temperatura del líquido a 0 - 100º
        nuevaTemperatura = Mathf.Clamp(nuevaTemperatura, 0f, 100f);        

        Debug.LogWarning("OnTemperatureChange() Called");
        Debug.Log("Masa de azucar del Liquid Holder = " + liquidHolder_liquid._masaDeAzucar);
        Debug.Log("Azucar excedente = " + _azucarExcedente);

        // Calcula la capacidad máxima de soluto
        float solutoMaximo = (36.25f * nuevaTemperatura) + 1275f;
        Debug.Log("Capacidad máxima de soluto = " + solutoMaximo);

        // Calcula la masa de azúcar máxima que puede tener la solución
        float masaDeAzucarMaxima = (solutoMaximo * liquidHolder_liquid._volumen) - liquidHolder_liquid._masaDeAgua;
        Debug.Log("Masa de azucar máxima = " + masaDeAzucarMaxima);

        Debug.Log("Masa de azucar en Liquid Holder = " + liquidHolder_liquid._masaDeAzucar);
        // Calcula si hay excedente
        if (liquidHolder_liquid._masaDeAzucar > masaDeAzucarMaxima)
            // Si la masa de azucar se excede el soluto máximo, pasa el excedente a _azucarExcedente
        {
            Debug.Log("HAY EXCEDENTE DE MASA DE AZUCAR");
            float excedente = liquidHolder_liquid._masaDeAzucar - masaDeAzucarMaxima;
            liquidHolder_liquid._masaDeAzucar = masaDeAzucarMaxima;
            _azucarExcedente = excedente;
        }
        else if (_azucarExcedente > 0f)
            // Si la masa de azucar no se excede del máximo, y hay excedente para disolver, disuelve hasta el máximo
        {
            Debug.Log("NO HAY EXCEDENTE DE MASA DE AZUCAR");
            // Primero calcula cuánto puede absorver
            float cuantoPuedeAbsorver = masaDeAzucarMaxima - liquidHolder_liquid._masaDeAzucar;
            Debug.Log("Cuánta masa de azucar puede absorver = " + cuantoPuedeAbsorver);
            if (cuantoPuedeAbsorver >= _azucarExcedente) // Si el azucar excedente puede ser absorvida completa o en parte, pasa a la masa de azucar del mosto
            {
                Debug.Log("ABSORVIENDO TODO EL EXCEDENTE");
                liquidHolder_liquid._masaDeAzucar += _azucarExcedente;
                _azucarExcedente = 0;
            }
            else // Sino, solo absorve lo que puede
            {
                Debug.Log("SE ABSORVIÓ PARTE DEL EXCEDENTE");
                // Calcula el excedente
                float excedente = _azucarExcedente - cuantoPuedeAbsorver;
                _azucarExcedente = excedente;
                liquidHolder_liquid._masaDeAzucar = masaDeAzucarMaxima;
            }
        }

        Debug.LogWarning("Finalizado. Los nuevos valores son los siguientes:");
        Debug.Log("Masa de azucar del Liquid Holder = " + liquidHolder_liquid._masaDeAzucar);
        Debug.Log("Azucar excedente = " + _azucarExcedente);
    } // OnTemperatureChange()

    #endregion
}
