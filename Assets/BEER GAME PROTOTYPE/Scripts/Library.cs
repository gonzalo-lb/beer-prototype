using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : MonoBehaviour
{
    // Variables para RandomAlt()
    static bool randomAlt_firstRandomDone_;
    static int randomAlt_esteRandom;
    static int randomAlt_randomAnterior;

    //Variables para FuncionMantisa()
    static float funcionMantisa_return;

    /// <summary>
    /// Si el número es par, devuelve true. Si es impar devuelve false
    /// </summary>
    /// <param name="_number"></param>
    /// <returns></returns>
    public static bool _EsPar(int _number)
    {
        if (_number % 2 == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Devuelve un número Random, sin repetir el que devolvió por última vez
    /// </summary>
    /// <param name="_min">Mínimo</param>
    /// <param name="_max">Máximo</param>
    /// <returns></returns>
    public static int _RandomAlt(int _min, int _max)
    {
        if (randomAlt_firstRandomDone_ == false)
        {
            randomAlt_esteRandom = Random.Range(_min, _max);
            randomAlt_randomAnterior = randomAlt_esteRandom;
            randomAlt_firstRandomDone_ = true;
            return randomAlt_esteRandom;
        }

        randomAlt_esteRandom = Random.Range(_min, _max);
        if (randomAlt_esteRandom != randomAlt_randomAnterior)
        {
            randomAlt_randomAnterior = randomAlt_esteRandom;
            return randomAlt_esteRandom;
        }
        else
        {
            randomAlt_esteRandom++;
            if (randomAlt_esteRandom > _max)
            {
                randomAlt_esteRandom = _min;
            }
            randomAlt_randomAnterior = randomAlt_esteRandom;
            return randomAlt_esteRandom;
        }
    }

    /// <summary>
    /// Devuelve un valor correspondiente X menos su parte entera
    /// Ej: 0:0 - 0.5:0.5 - 1.5:0.5 - 1.9:0.9
    /// </summary>
    /// <param name="_x"></param>
    /// <returns></returns>
    public static float _FuncionMantisa(float _x)
    {
        funcionMantisa_return = _x - Mathf.FloorToInt(_x);
        return funcionMantisa_return;
    }

    /// <summary>
    /// Retorna el primer GameObject que encuentre con el Tag indicado.
    /// No busca dentro de los Child de cada Child. Solo busca en la primera sub-jerarquía.
    /// Si hay más de un Child GameObject con el mismo Tag, solo va a retornar el primero que encuentre, según la indexación de childs.
    /// Si no encuentra ninguno, retorna Null.
    /// </summary>
    public static GameObject _GetChildrenWithTag(GameObject _parent, string _tag)
    {
        Transform parent = _parent.transform;

        if (parent.childCount == 0)
        {
            return null;
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                return child.gameObject;
            }
        }

        return null;
    }

    /// <summary>
    /// Retorna un array con los child del GameObject que se indique como parámetro.
    /// No busca dentro de los Child de cada Child. Solo busca en la primera sub-jerarquía.
    /// Si no encuentra ninguno, retorna Null.
    /// </summary>
    public static GameObject[] _GetChildrensGameObject(GameObject _parent)
    {
        GameObject[] toReturn;
        List<GameObject> detectedGameObjects = new List<GameObject>();

        Transform parent = _parent.transform;

        if (parent.childCount == 0)
        {
            return null;
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            detectedGameObjects.Add(parent.GetChild(i).gameObject);
        }

        toReturn = detectedGameObjects.ToArray();
        return toReturn;
    }

    /// <summary>
    /// Remapea un rango de números, a otro rango. El rango original es el que se va a utilizar 
    /// luego como parámetro "value" y suele ir de 0 a 1. El target es el rango a remapear. 
    /// El retorno se encuentra limitado al rango original.
    /// </summary>
    /// <param name="targetMin">Suele ir de 0 a 1.</param>
    /// <param name="targetMAx">Suele ir de 0 a 1.</param>
    /// <param name="inputMin">Es el rango a remapear.</param>
    /// <param name="inputMax">Es el rango a remapear.</param>
    /// <param name="inputValue">Retorna el input, remapeado al rango target.</param>
    /// <returns></returns>
    public static float _Remap(float targetMin, float targetMAx, float inputMin, float inputMax, float inputValue)
    {
        float rel = Mathf.InverseLerp(targetMin, targetMAx, inputValue);
        return Mathf.Lerp(inputMin, inputMax, rel);
    }

    /// <summary>
    /// Remapea el target a través de un input de 0 a 1, donde un input01 de 0 devuelve targetMin, y 
    /// un input01 de 1 devuelve targetMax.
    /// </summary>
    /// <param name="targetMin"></param>
    /// <param name="targetMax"></param>
    /// <param name="input01Value">Debe ser un valor entre 0 y 1.</param>
    /// <returns></returns>
    public static float _Remap01ToTarget(float targetMin, float targetMax, float input01Value)
    {
        input01Value = Mathf.Clamp01(input01Value);
        float rel = Mathf.InverseLerp(0, 1, input01Value);
        return Mathf.Lerp(targetMin, targetMax, rel);
    }

    public static float _RemapTargetTo01(float targetMin, float targetMax, float input01Value)
    {        
        float rel = Mathf.InverseLerp(targetMin, targetMax, input01Value);
        return Mathf.Lerp(0, 1, rel);
    }

    /// <summary>
    /// Calcula la temperatura final entre 2 líquidos que tengan la misma constante. Devuelve un 
    /// FLOAT con la temperatura en grados centígrados. No calcula cuánto se tarda en llegar a esa 
    /// temperatura.
    /// </summary>
    /// <param name="M1">Masa del líquido 1, en gramos</param>
    /// <param name="T1">Temperatura dél líquido 1, en grados centígrados</param>
    /// <param name="M2">Masa del líquido 2, en gramos</param>
    /// <param name="T2">Temperatura dél líquido 2, en grados centígrados</param>
    /// <returns></returns>
    public static float _TemperatureCalcBetween2Liquids(float M1, float T1, float M2, float T2)
    {
        return -(-(M1*T1)-(M2*T2))/(M1 + M2);
    }
}
