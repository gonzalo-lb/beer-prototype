using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid
{
    private float _Volumen;
    public float _volumen // En mililitros
    {
        get { return _Volumen; }
        set
        {
            if(value < 0f) { value = 0f; }
            _Volumen = value;
        }
    }

    private float _Temperatura;
    public float _temperatura // En ºC
    {
        get { return _Temperatura; }
        set
        {
            _Temperatura = value;
            _Temperatura = Mathf.Clamp(_temperatura, 0f, 100f);
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

    private float _MasaDeAzucar;
    public float _masaDeAzucar // En miligramos
    {
        get { return _MasaDeAzucar; }
        set
        {
            if (value < 0) { value = 0; }
            _MasaDeAzucar = value;
        }
    }

    public float _densidad // En miligramos/mililitros. Los densímetros tienen un máximo de 1160
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

    public float _etanol;
    public float _graduacionAlcoholica
    {
        get
        {
            return (_etanol * 100f) / _volumen;
        }
    }

    public float _IBU;

    public float _residuos; // Esto determina la turbidez del líquido

    public float _volumenesDeCO2;

    public float _proteina; // Se la tiene que sumar la maceración, y con la cocción tiene que ir disminuyendo

    public Color _color;
    public Color _colorDelAgua
    {
        get
        {
            return new Color(0, 13, 255, 150);
        }
    }    
    public Color _colorDelMosto
    {
        get
        {
            return new Color(111, 79, 0, 255);
        }
    }

    public Liquid _WithVolume_MasaAndTemperatureAsDeltaTime()
    {
        Liquid toReturn = new Liquid();

        toReturn._volumen = _volumen * Time.deltaTime;
        toReturn._masaDeAzucar = _masaDeAzucar * Time.deltaTime;        
        toReturn._temperatura = _temperatura * Time.deltaTime;
        toReturn._color = _color;

        return toReturn;
    }

    public Liquid _WithVolumeAndMasaAsDeltaTime()
    {
        Liquid toReturn = new Liquid();

        toReturn._volumen = _volumen * Time.deltaTime;
        toReturn._masaDeAzucar = _masaDeAzucar * Time.deltaTime;
        toReturn._temperatura = _temperatura;
        toReturn._color = _color;

        return toReturn;
    }

    public Liquid _WithTemperatureAsDeltaTime()
    {
        Liquid toReturn = new Liquid();

        toReturn._volumen = _volumen;
        toReturn._masaDeAzucar = _masaDeAzucar;
        toReturn._temperatura = _temperatura * Time.deltaTime;
        toReturn._color = _color;

        return toReturn;
    }
}
