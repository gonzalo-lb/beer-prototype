using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagHelper_SCENES
{
    public const string PROTOTYPE_GAME_SCENE = "GamePrototype";
}

public class TagHelper_GAMEOBJECTS
{
    public const string OBJECT_HOLDER = "Object Holder";
    public const string HOLDER_FRONT_REFERENCE = "Front Reference";
}

public class TagHelper_TAGS
{
    public const string OBJECT_PICKABLE = "Object Pickable";
    public const string OBJECT_HOLDER = "Object Holder"; // GameObject sobre el que se puede dejar un objeto
    public const string OBJECT_INTERACTABLE_SIMPLE = "Simple Interactable";
}

public class TagHelper_LAYERS
{
    public const int OBJECT_INTERACTABLE = 6;
    public const int ALWAYS_VISIBLE = 7;
    public const int OBJECT_HOLDER = 8;
    public const int PLAYER = 9;
    public const int OBJECT_PICKABLE = 10;

}

public class TagHelper_COLLIDERS
{
    public const string PURIFICADORLP_WATERCOLLIDER = "PurificadorLPWaterCollider";
    public const string COCINA_FUEGO_COLLIDER = "HotCollider";
}
