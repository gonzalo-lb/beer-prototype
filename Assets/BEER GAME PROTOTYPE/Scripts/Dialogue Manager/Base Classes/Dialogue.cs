using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName ="Dialogue / New Dialogue")]
[System.Serializable]
public class Dialogue : ScriptableObject
{
    [TextArea(2, 10)]
    public string[] sentences;
    
}
