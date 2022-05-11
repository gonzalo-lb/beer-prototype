using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_LlaveHornalla1 : ObjectInteractable_Simple
{
    [SerializeField] GameObject fuego;

    public override void _OnStart()
    {
        base._OnStart();
        fuego.SetActive(false);
    }

    public override void _OnClick()
    {
        base._OnClick();
        if (fuego.activeSelf) { fuego.SetActive(false); }
        else { fuego.SetActive(true); }
    }
}
