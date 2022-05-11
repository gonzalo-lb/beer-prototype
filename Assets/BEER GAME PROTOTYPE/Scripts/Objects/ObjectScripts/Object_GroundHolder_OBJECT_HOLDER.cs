using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_GroundHolder_OBJECT_HOLDER : ObjectHolder
{
    [SerializeField] FadeInOut fadeInOut;
    [Header("EVENTS")]
    [SerializeField] GameEvent eventToTrigger;
    [Header("DIALOGUES")]
    [SerializeField] Dialogue dialogueToSpawn;

    public override void OnStart()
    {
        base.OnStart();
        this.gameObject.layer = 8;
    }
    
    public override void _OnObjectRelease()
    {
        fadeInOut._AlphaTo0();
        eventToTrigger._Raise();
    }

    public void _EVENT_SpawnDialogue()
    {
        GameHandler_PrototypeScene.instance._GoToDialogueMenuAndStartDialogue(dialogueToSpawn, false, EventManager.instance.dummyEvent);
    }
}
