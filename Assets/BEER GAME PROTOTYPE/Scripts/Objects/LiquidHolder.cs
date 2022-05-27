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

    float _volumenEnPorcentaje
    {
        get
        {
            return (liquidHolder_liquid._volumen * 100) / _capacidadDelRecipiente;
        }
    }

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
        _SetVolumenDeLiquido(volumen_, densidad_);

        // Temperatura
        if (volumen_ <= 0) { liquidHolder_liquid._temperatura = 0; } else { liquidHolder_liquid._temperatura = temperatura_; }

        // Color
        liquidHolder_liquid._color = color_;
    }    

    void _SetVolumenDeLiquido(float volumen, float densidad)
    {
        liquidHolder_liquid._volumen = volumen;
        liquidHolder_liquid._densidad = densidad;        
    }   

    #endregion

    #region Get Methods

    public float _GetCapacidadDelRecipiente() { return _capacidadDelRecipiente; }

    public float _GetTemperatura() { return liquidHolder_liquid._temperatura; }

    public float _GetVolumenDeLiquido() { return liquidHolder_liquid._volumen; }

    public float _GetMasaDeAzucar() { return liquidHolder_liquid._masaDeAzucar; }

    public float _GetMasaDeAgua() { return liquidHolder_liquid._masaDeAgua; }

    public float _GetMasaTotal() { return liquidHolder_liquid._masaTotal; }

    public float _GetDensidad() { return liquidHolder_liquid._densidad; }

    public float _GetLupulo() { return _lupulo; }

    public float _GetGrano() { return _grano; }

    public float _GetGranoMacerado() { return _granoMacerado; }

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
        }
    }

    public void _RestarTemperatura(float temperatura)
    {
        if (liquidHolder_liquid._volumen <= 0) { return; }
        else
        {
            liquidHolder_liquid._temperatura -= temperatura;            
        }
    }

    public void _Macerar()
    {
        // Tiene que convertir el grano en densidad
    }

    #endregion
}
