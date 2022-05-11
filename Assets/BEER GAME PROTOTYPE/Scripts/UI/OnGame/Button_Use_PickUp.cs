using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Use_PickUp : MonoBehaviour
{
    [SerializeField] Text buttonUsePickUp;
    bool usePickUp;

    // Start is called before the first frame update
    void Start()
    {
        usePickUp = true;
        buttonUsePickUp.text = "Pickup";
    }

    public void ToggleUsePickUp()
    {
        if (!usePickUp)
        {
            usePickUp = true;
            buttonUsePickUp.text = "Pickup";
        }
        else
        {
            usePickUp = false;
            buttonUsePickUp.text = "Use";
        }
    }

    /// <summary>
    /// Devuelve TRUE si está en Pickup, y FALSE si está en Use
    /// </summary>
    /// <returns></returns>
    public bool _IsUsePickUpState()
    {
        return usePickUp;
    }
}
