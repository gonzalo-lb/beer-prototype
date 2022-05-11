using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_ResetOrientation : MonoBehaviour
{

    [SerializeField] Transform theCamera;
    [SerializeField] Transform theBody;

    // Start is called before the first frame update
    public void ResetOrientation()
    {
        theBody.transform.rotation = Quaternion.Euler(0, 0, 0);
        theCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
