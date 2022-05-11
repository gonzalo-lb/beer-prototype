using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Liquid
{
    public float _miliLitrosAgua;    
    public float _miliLitrosMosto;    
    public float _temperatura;

    [Tooltip("Especificada en gramos.")]
    public float _masa; // Debería estar en gramos
    
    public float _densidad
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
    public float _volumen
    {
        get
        {
            return _miliLitrosAgua + _miliLitrosMosto;
        }
    }

    public void _SetVolumenDeLiquido(float volumen, float __porcentajeDeMosto01)
    {
        if (volumen <= 0)
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

    public Liquid _WithVolumeAndTemperatureAsDeltaTime()
    {
        Liquid toReturn = new Liquid();

        toReturn._masa = _masa * Time.deltaTime;
        toReturn._miliLitrosAgua = _miliLitrosAgua * Time.deltaTime;
        toReturn._miliLitrosMosto = _miliLitrosMosto * Time.deltaTime;
        toReturn._temperatura = _temperatura * Time.deltaTime;

        return toReturn;
    }

    public Liquid _WithVolumeAsDeltaTime()
    {
        Liquid toReturn = new Liquid();

        toReturn._masa = _masa * Time.deltaTime;
        toReturn._miliLitrosAgua = _miliLitrosAgua * Time.deltaTime;
        toReturn._miliLitrosMosto = _miliLitrosMosto * Time.deltaTime;
        toReturn._temperatura = _temperatura;

        return toReturn;
    }

    public Liquid _WithTemperatureAsDeltaTime()
    {
        Liquid toReturn = new Liquid();

        toReturn._masa = _masa;
        toReturn._miliLitrosAgua = _miliLitrosAgua;
        toReturn._miliLitrosMosto = _miliLitrosMosto;
        toReturn._temperatura = _temperatura * Time.deltaTime;

        return toReturn;
    }
}
