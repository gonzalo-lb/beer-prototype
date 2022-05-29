using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugClass : MonoBehaviour
{

    public LiquidHolder liquidHolder;

    public Liquid liquid;

    public float volumen;
    public float temperatura;
    public float masaDeAgua;
    public float masaTotal;
    public float masadeAzucar;
    public float densidad;
    public Color color;

    public bool SetLiquid;
    public bool LogLiquidValues;
    public bool LogLiquidHolderValues;
    public bool AgregarLiquidoYDebugLog;
    public bool Macerar;

    private void Awake()
    {
        liquid = new Liquid();
        liquidHolder = new LiquidHolder(30000, 0, 0, 0, Color.blue);
        liquidHolder._AgregarGrano(5000f);
    }    

    private void Update()
    {
        if (SetLiquid)
        {
            SetLiquid = false;
            liquid._volumen = volumen;            
            liquid._temperatura = temperatura;            
            liquid._densidad = densidad;
            liquid._color = color;
        }

        if (LogLiquidValues)
        {
            LogLiquidValues = false;
            Debug.LogWarning("LIQUID VALUES");
            Debug.Log("liquid._volumen = " + liquid._volumen);
            Debug.Log("liquid._temperatura = " + liquid._temperatura);
            Debug.Log("liquid._masaDeAgua = " + liquid._masaDeAgua);
            Debug.Log("liquid._masaTotal = " + liquid._masaTotal);
            Debug.Log("liquid._masaDeAzucar = " + liquid._masaDeAzucar);
            Debug.Log("liquid._densidad = " + liquid._densidad);
            Debug.Log("liquid._color = " + liquid._color);
        }

        if (LogLiquidHolderValues)
        {
            LogLiquidHolderValues = false;
            Debug.LogWarning("LIQUID HOLDER VALUES");
            Debug.Log("liquidHolder: volumen = " + liquidHolder._GetVolumenDeLiquido());
            Debug.Log("liquidHolder: temperatura = " + liquidHolder._GetTemperatura());
            Debug.Log("liquidHolder: masaDeAgua = " + liquidHolder._GetMasaDeAgua());
            Debug.Log("liquidHolder: masaTotal = " + liquidHolder._GetMasaTotal());
            Debug.Log("liquidHolder: masaDeAzucar = " + liquidHolder._GetMasaDeAzucar());
            Debug.Log("liquidHolder: densidad = " + liquidHolder._GetDensidad());
            Debug.Log("liquidHolder: color = " + liquidHolder._GetColor());
            Debug.Log("liquidHolder: grano = " + liquidHolder._GetGrano());
            Debug.Log("liquidHolder: peso del grano = " + liquidHolder._GetPesoDelGrano());

        }

        if (AgregarLiquidoYDebugLog)
        {
            AgregarLiquidoYDebugLog = false;
            liquidHolder._AgregarLiquido(liquid);
            LogLiquidHolderValues = true;
        }

        if (Macerar)
        {
            Macerar = false;
            Debug.LogWarning("MACERADO");
            Debug.Log("Masa de azucar = " + liquidHolder._GetMasaDeAzucar() + "; Cantidad de grano previo al macerado = " + liquidHolder._GetGrano() + "; Peso total del grano = " + liquidHolder._GetPesoDelGrano() + "; Densidad = " + liquidHolder._GetDensidad() + "; Volumen de líquido = " + liquidHolder._GetVolumenDeLiquido());
            liquidHolder._Macerar(liquidHolder._GetGrano());
            Debug.Log("Masa de azucar = " + liquidHolder._GetMasaDeAzucar() + "; Cantidad de grano luego del macerado = " + liquidHolder._GetGrano() + "; Peso total del grano = " + liquidHolder._GetPesoDelGrano() + "; Densidad = " + liquidHolder._GetDensidad() + "; Volumen de líquido = " + liquidHolder._GetVolumenDeLiquido());
        }
    }
}
