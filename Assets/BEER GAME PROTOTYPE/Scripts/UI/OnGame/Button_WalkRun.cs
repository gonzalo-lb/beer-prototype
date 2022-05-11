using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_WalkRun : MonoBehaviour
{
    bool running;
    bool XZToggle;
    [SerializeField] Text buttonWalkRunText;
    

    // Start is called before the first frame update
    void Start()
    {
        running = false;        
        buttonWalkRunText.text = "WALK";        
    }

    public void ToggleWalkRun()
    {
        if (!running)
        {
            running = true;
            buttonWalkRunText.text = "RUN";
        }
        else
        {
            running = false;
            buttonWalkRunText.text = "WALK";
        }
    }



    public bool IsRunningBoolState()
    {
        return running;
    }


}
