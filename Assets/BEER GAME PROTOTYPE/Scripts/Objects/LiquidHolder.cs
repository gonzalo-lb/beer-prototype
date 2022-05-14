using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// SIEMPRE HAY QUE INICIAR ESTA CLASS DESDE AFUERA, CON EL METODO _START_SET_METHOD(...)

public class LiquidHolder : MonoBehaviour
{
    #region Valores

    // Valores del recipiente y del líquido
    Liquid _liquid;

    float _capacidadDelRecipiente; // En mililitros

    [Tooltip("En gramos.")]
    float _lupulo;

    [Tooltip("En gramos.")]
    float _grano;
    float _granoMacerado;

    float _volumenEnPorcentaje
    {
        get
        {
            return (_liquid._volumen * 100) / _capacidadDelRecipiente;
        }
    }

    #endregion

    #region Inicialización de la Class

    private void Awake()
    {
        _liquid = new Liquid();        
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
        _SetVolumenDeLiquido(volumen_, densidad_);

        // Temperatura
        if (volumen_ <= 0) { _liquid._temperatura = 0; } else { _liquid._temperatura = temperatura_; }

        // Color
        _liquid._color = color_;
    }    

    void _SetVolumenDeLiquido(float volumen, float densidad)
    {
        if(densidad < 1000f) { densidad = 1000f; }

        if (volumen <= 0)
        {
            _liquid._volumen = 0f;
            _liquid._masaDeAzucar = 0f;
        }
        else
        {
            _liquid._volumen = volumen;
            _liquid._densidad = densidad;
        }
    }   

    #endregion

    #region Get Methods

    public float _GetCapacidadDelRecipiente() { return _capacidadDelRecipiente; }

    public float _GetTemperatura() { return _liquid._temperatura; }

    public float _GetVolumenDeLiquido() { return _liquid._volumen; }

    public float _GetMasaDeAzucar() { return _liquid._masaDeAzucar; }

    public float _GetMasaDeAgua() { return _liquid._masaDeAgua; }

    public float _GetMasaTotal() { return _liquid._masaTotal; }

    public float _GetDensidad() { return _liquid._densidad; }

    public float _GetLupulo() { return _lupulo; }

    public float _GetGrano() { return _grano; }

    public float _GetGranoMacerado() { return _granoMacerado; }

    public float _GetVolumenEnPorcentaje() { return _volumenEnPorcentaje; }

    public Color _GetColor() { return _liquid._color; }

    #endregion

    #region Main Functionality

    public void _AgregarLiquido(Liquid liquido)
    {
        // Guardamos variables para calcular la nueva temperatura
        float prevVol = _liquid._volumen;

        // Se agrega el volumen y masa de azucar
        _liquid._volumen += liquido._volumen;
        _liquid._masaDeAzucar += liquido._masaDeAzucar;        

        // Calcular la nueva temperatura del líquido
        if (prevVol <= 0f) { _liquid._temperatura = liquido._temperatura; } // Si no había agua, la temperatura del líquido es la del que se agrega
        else // Si ya tenía líquido, se calcula la nueva temperatura
        {
            float newTemperatura = Library._TemperatureCalcBetween2Liquids(prevVol, _liquid._temperatura, liquido._volumen, liquido._temperatura);
            _liquid._temperatura = newTemperatura;
        }

        // Si la cantidad de líquido supera a la del Holder, calcular nuevas cantidades de agua y mosto
        if (_liquid._volumen > _capacidadDelRecipiente)
        {
            float excedenteDeVolumen = _liquid._volumen - _capacidadDelRecipiente;
            float excedenteEnPorcentaje01 = excedenteDeVolumen / _liquid._volumen;

            // Calcula las nuevas cantidad de agua y mosto limitadas a la capacidad del recipiente
            float volumenARestar = _liquid._volumen * excedenteEnPorcentaje01;
            _liquid._volumen -= volumenARestar;

            // Calcula la nueva densidad
            float masaDeAzucarARestar = _liquid._masaDeAzucar * excedenteEnPorcentaje01;
            _liquid._masaDeAzucar -= masaDeAzucarARestar;
        }
    }

    public void _RestarLiquido(float volumenARestar)
    {
        if (volumenARestar > _liquid._volumen)
        {
            _liquid._volumen = 0f;
            _liquid._temperatura = 0f;
            _liquid._masaDeAzucar = 0f;
            _liquid._color = Color.clear; // No estoy seguro de que esta linea esté bien
        }
        else
        {
            float porcentajeDeVolumenARestar = volumenARestar / _liquid._volumen;
            _liquid._volumen -= _liquid._volumen * porcentajeDeVolumenARestar;
            _liquid._masaDeAzucar -= _liquid._masaDeAzucar * porcentajeDeVolumenARestar;
        }
    }

    public void _AgregarTemperatura(float temperatura)
    {
        if (_liquid._volumen <= 0) { return; }
        else
        {
            _liquid._temperatura += temperatura;            
        }
    }

    public void _RestarTemperatura(float temperatura)
    {
        if (_liquid._volumen <= 0) { return; }
        else
        {
            _liquid._temperatura -= temperatura;            
        }
    }

    public void _Macerar()
    {
        // Tiene que convertir el grano en densidad
    }

    #endregion
}
