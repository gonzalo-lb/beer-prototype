using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemapScript : MonoBehaviour
{
    public float originFrom;
    public float originTo;
    public float targetFrom;
    public float targetTo;
    public float value;
    public float result;
    public bool update;

    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            update = false;
            DoTheRemap();
        }
    }

    void DoTheRemap()
    {
        result = Library._Remap(originFrom, originTo, targetFrom, targetTo, value);
    }
}
