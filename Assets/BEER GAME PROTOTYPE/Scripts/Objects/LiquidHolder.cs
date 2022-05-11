using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// SIEMPRE HAY QUE INICIAR ESTA CLASS DESDE AFUERA, CON EL METODO _START_SET_METHOD(...)


public class LiquidHolder
{
    #region Valores

    float _miliLitrosAgua;
    float _miliLitrosMosto;
    float _temperatura;

    [Tooltip("Especificada en gramos.")]
    float _masa; // Debería estar en gramos

    [Tooltip("En gramos.")]
    float _lupulo;

    [Tooltip("En gramos.")]
    float _grano;

    float _densidad
    {
        get
        {
            return Mathf.Round((_masa * 1000f) / _volumen);
        }
        set
        {
            if (value < 900f) { value = 900f; }
            _masa = (value * _volumen) / 1000f;
        }
    }
    float _volumen
    {
        get
        {
            return _miliLitrosAgua + _miliLitrosMosto;
        }
    }

    float _volumenEnPorcentaje
    {
        get
        {
            return (_volumen * 100) / _capacidadDelRecipiente;
        }
    }
        
    float _capacidadDelRecipiente; // En mililitros
         
    float _porcentajeDeMosto01 // Tiene que ser un número entre 0 y 1, donde 0 es que el líquido es todo agua, y 1 que el líquido es todo mosto.
    {
        get
        {
            return Mathf.Clamp01(_miliLitrosMosto / _volumen);
        }
        set
        {
            float newValue = Mathf.Clamp01(value);
            float prevVol = _volumen;
            _miliLitrosMosto = prevVol * newValue;
            _miliLitrosAgua = prevVol - _miliLitrosMosto;
        }
    }    

    #endregion

    #region Set Metodos

    public void _START_SET_METHOD(float capacidadRecipiente_, float volumen_, float porcentajeMosto01_, float densidad_, float temperatura_)
    {
        // Capacidad del recipiente
        if (capacidadRecipiente_ <= 0)
        {
            _capacidadDelRecipiente = 30000;
            Debug.LogWarning("La capacidad del recipiente no puede ser igual o menor a 0. Se setea en 30.000 ml.");
        }else { _capacidadDelRecipiente = capacidadRecipiente_; }        

        // Volumen inicial de líquido
        if(volumen_ < 0) { volumen_ = 0; }
        _SetVolumenDeLiquido(volumen_, Mathf.Clamp01(porcentajeMosto01_));

        // Densidad
        if(densidad_ < 0) { densidad_ = 1000f; }
        if(volumen_ <= 0) { _densidad = 0; } else { _densidad = densidad_; }

        // Temperatura
        if (volumen_ <= 0) { _temperatura = 0; } else { _temperatura = temperatura_; }
    }

    // void _SetCapacidadDelRecipiente(float capacidad) => _capacidadDelRecipiente = capacidad;
    
    // void _SetTemperatura(float temperatura) => _temperatura = temperatura;

    void _SetVolumenDeLiquido(float volumen, float __porcentajeDeMosto01)
    {
        if(volumen <= 0)
        {
            _miliLitrosMosto = 0;
            _miliLitrosAgua = 0;
            _masa = 0;
            _temperatura = 0;
        }
        else
        {
            _miliLitrosMosto = volumen * __porcentajeDeMosto01;
            _miliLitrosAgua = volumen - _miliLitrosMosto;
        }        
    }

    // void _SetMiliLitrosDeAgua(float mlDeAgua) { _miliLitrosAgua = mlDeAgua; }

    // void _SetMiliLitrosDeMosto(float mlDeMosto) { _miliLitrosMosto = mlDeMosto; }

    // void _SetMasaDeAzucar(float masaDeAzucar) { _masaDeAzucar = masaDeAzucar; }

    #endregion

    #region Get Methods

    public float _GetCapacidadDelRecipiente() { return _capacidadDelRecipiente; }

    public float _GetTemperatura() { return _temperatura; }

    public float _GetVolumenDeLiquido() { return _volumen; }
    
    public float _GetMiliLitrosDeAgua() { return _miliLitrosAgua; }

    public float _GetMiliLitrosDeMosto() { return _miliLitrosMosto; }

    public float _GetMasaDeAzucar() { return _masa; }

    public float _GetDensidad() { return _densidad; }

    public float _GetLupulo() { return _lupulo; }

    public float _GetVolumenEnPorcentaje() { return _volumenEnPorcentaje; }

    public float _GetPorcentajeDeMosto01() { return _porcentajeDeMosto01; }

    #endregion

    #region Main Functionality

    public void _AgregarLiquido(Liquid liquido)
    {
        // Guardamos variables para calcular la nueva temperatura
        float prevVol = _volumen;        

        // Se agrega el volumen y masa de azucar
        _miliLitrosAgua += liquido._miliLitrosAgua;
        _miliLitrosMosto += liquido._miliLitrosMosto;
        _masa += liquido._masa;
        
        // Calcular la nueva temperatura del líquido
        if(prevVol <= 0f) { _temperatura = liquido._temperatura; } // Si no había agua, la temperatura del líquido es la del que se agrega
        else // Si ya tenía líquido, se calcula la nueva temperatura
        {
            float newTemperatura = Library._TemperatureCalcBetween2Liquids(prevVol, _temperatura, liquido._volumen, liquido._temperatura);
            _temperatura = newTemperatura;
        }
        
        // Si la cantidad de líquido supera a la del Holder, calcular nuevas cantidades de agua y mosto
        if(_volumen > _capacidadDelRecipiente)
        {
            float excedenteDeVolumen = _volumen - _capacidadDelRecipiente;
            float excedenteEnPorcentaje01 = excedenteDeVolumen / _volumen;

            // Calcula las nuevas cantidad de agua y mosto limitadas a la capacidad del recipiente
            float porcentajeDeMosto = _porcentajeDeMosto01;
            _miliLitrosMosto = _capacidadDelRecipiente * porcentajeDeMosto;
            _miliLitrosAgua = _capacidadDelRecipiente - _miliLitrosMosto;

            // Calcula la nueva densidad
            float masaDeAzucarARestar = _masa * excedenteEnPorcentaje01;
            _masa -= masaDeAzucarARestar;
        }
    }

    public void _RestarLiquido(float volumenARestar)
    {
        if(volumenARestar > _volumen)
        {
            _miliLitrosAgua = 0f;
            _miliLitrosMosto = 0f;
            _temperatura = 0f;
            _masa = 0f;
        }
        else
        {
            float porcentajeDeVolumenARestar = volumenARestar / _volumen;
            _miliLitrosAgua -= _miliLitrosAgua * porcentajeDeVolumenARestar;
            _miliLitrosMosto -= _miliLitrosMosto * porcentajeDeVolumenARestar;
            _masa -= _masa * porcentajeDeVolumenARestar;
        }
    }

    public void _AgregarTemperatura(float temperatura)
    {
        if(_volumen <= 0) { return; }
        else
        {
            _temperatura += temperatura;
            if(_temperatura > 100f) { _temperatura = 100f; }
        }
    }

    public void _RestarTemperatura(float temperatura)
    {
        if (_volumen <= 0) { return; }
        else
        {
            _temperatura -= temperatura;
            if (_temperatura < 0f) { _temperatura = 0f; }
        }
    }

    public void _Macerar()
    {
        // Tiene que convertir el grano en densidad
    }

    #endregion
}
