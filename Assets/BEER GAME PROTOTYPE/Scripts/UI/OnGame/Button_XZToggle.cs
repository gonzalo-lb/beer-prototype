using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_XZToggle : MonoBehaviour
{
    
    bool XZToggle;
    
    [SerializeField] Text buttonXZToggleText;

    // Start is called before the first frame update
    void Start()
    {        
        XZToggle = false;        
        buttonXZToggleText.text = "X";
    }

    

    public void ToggleXZ()
    {
        if (!XZToggle)
        {
            XZToggle = true;
            buttonXZToggleText.text = "XZ";
        }
        else
        {
            XZToggle = false;
            buttonXZToggleText.text = "X";
        }
    }


    public bool XZToggleBoolState()
    {
        return XZToggle;
    }
}
