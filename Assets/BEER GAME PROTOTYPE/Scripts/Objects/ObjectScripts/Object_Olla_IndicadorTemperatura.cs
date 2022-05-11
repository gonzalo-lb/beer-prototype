using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Object_Olla_IndicadorTemperatura : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    Vector3 rot180onYAxis = new Vector3(0, 180f, 0);
    [SerializeField] TextMesh text;

    private void Awake() { text = GetComponent<TextMesh>(); }

    void Update()
    {
        transform.LookAt(playerCamera);
        transform.Rotate(rot180onYAxis, Space.Self);
    }

    public void _UpdateText(string _text) { text.text = _text; }
}
