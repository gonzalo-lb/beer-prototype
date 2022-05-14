using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public float _volumen // En mililitros
    {
        get { return _volumen; }
        set
        {
            if(value < 0) { value = 0; }
            _volumen = value;
        }
    }

    public float _temperatura // En ºC
    {
        get { return _temperatura; }
        set
        {
            _temperatura = value;
            _temperatura = Mathf.Clamp(_temperatura, 0f, 100f);
        }
    }
    
    public float _masaDeAgua // Es pasar el volumen de agua a mili gramos, porque la densidad se mide en miligramos/mililitros
    {
        get
        {
            return _volumen * 1000f;
        }
    }

    public float _masaTotal // En miligramos
    {
        get
        {
            return _masaDeAgua + _masaDeAzucar;
        }
    }
        
    public float _masaDeAzucar // En miligramos
    {
        get { return _masaDeAzucar; }
        set
        {
            if (value < 0) { value = 0; }
            _volumen = value;
        }
    }

    public float _densidad // En miligramos/mililitros
    {
        get
        {
            return Mathf.Round(_masaTotal / _volumen);
        }
        set
        {
            if (value < 1000f)
            {
                Debug.LogWarning("Se está intentando establecer una densidad por debajo de 1000. Se establece en el mínimo (1000).");
                value = 1000f;
            }

            _masaDeAzucar = (value * _volumen) - _masaDeAgua;
        }
    }

    public Color _color;
    
    public Liquid _WithVolume_MasaAndTemperatureAsDeltaTime()
    {
        Liquid toReturn = new Liquid();

        toReturn._volumen = _volumen * Time.deltaTime;
        toReturn._masaDeAzucar = _masaDeAzucar * Time.deltaTime;        
        toReturn._temperatura = _temperatura * Time.deltaTime;

        return toReturn;
    }

    public Liquid _WithVolumeAndMasaAsDeltaTime()
    {
        Liquid toReturn = new Liquid();

        toReturn._volumen = _volumen * Time.deltaTime;
        toReturn._masaDeAzucar = _masaDeAzucar * Time.deltaTime;
        toReturn._temperatura = _temperatura;

        return toReturn;
    }

    public Liquid _WithTemperatureAsDeltaTime()
    {
        Liquid toReturn = new Liquid();

        toReturn._volumen = _volumen;
        toReturn._masaDeAzucar = _masaDeAzucar;
        toReturn._temperatura = _temperatura * Time.deltaTime;

        return toReturn;
    }
}
