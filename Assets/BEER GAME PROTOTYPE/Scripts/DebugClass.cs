using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugClass : MonoBehaviour
{

    public LiquidHolder liquidHolder;

    public Liquid liquid;

    public float masaDeAzucar;
    public float miliLitrosDeAgua;
    public float miliLitrosDeMosto;
    public float temperatura;

    public bool SetLiquid;
    public bool LogLiquidValues;
    public bool LogLiquidHolderValues;
    public bool AgregarLiquidoYDebugLog;

    private void Awake()
    {
        liquid = new Liquid();
        liquidHolder = new LiquidHolder();
        liquidHolder._START_SET_METHOD(30000, 0, 0, 0, 0);
    }

    private void Update()
    {
        if (SetLiquid)
        {
            SetLiquid = false;
            liquid._masa = masaDeAzucar;
            liquid._miliLitrosAgua = miliLitrosDeAgua;
            liquid._miliLitrosMosto = miliLitrosDeMosto;
            liquid._temperatura = temperatura;
        }

        if (LogLiquidValues)
        {
            LogLiquidValues = false;
            Debug.LogWarning("LIQUID VALUES");
            Debug.Log("liquid._masaDeAzucar = " + liquid._masa);
            Debug.Log("liquid._miliLitrosAgua = " + liquid._miliLitrosAgua);
            Debug.Log("liquid._miliLitrosMosto = " + liquid._miliLitrosMosto);
            Debug.Log("liquid._temperatura = " + liquid._temperatura);
        }

        if (LogLiquidHolderValues)
        {
            LogLiquidHolderValues = false;
            Debug.LogWarning("LIQUID HOLDER VALUES");
            Debug.Log("liquidHolder: masaDeAzucar = " + liquidHolder._GetMasaDeAzucar());
            Debug.Log("liquidHolder: miliLitrosAgua = " + liquidHolder._GetMiliLitrosDeAgua());
            Debug.Log("liquidHolder: miliLitrosMosto = " + liquidHolder._GetMiliLitrosDeMosto());
            Debug.Log("liquidHolder: temperatura = " + liquidHolder._GetTemperatura());
            Debug.Log("liquidHolder: volumen de líquido = " + liquidHolder._GetVolumenDeLiquido());
            Debug.Log("liquidHolder: densidad = " + liquidHolder._GetDensidad());
        }

        if (AgregarLiquidoYDebugLog)
        {
            AgregarLiquidoYDebugLog = false;
            liquidHolder._AgregarLiquido(liquid);
            LogLiquidHolderValues = true;
        }
    }
}
