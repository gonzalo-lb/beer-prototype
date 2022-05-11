using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    [Header("EVENTS")]
    // Events
    public GameEvent dummyEvent;
    [SerializeField] GameEvent StartOpeningDialogueEvent;
    [SerializeField] GameEvent ResaltarOllaEvent;

    [Header("DIALOGUES")]
    // Diálogos
    [SerializeField] Dialogue StartOpeningDialogue;

    private void Awake()
    {
        //  ARMA EL SINGLETON
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }
    
    #region Event --> StartOpeningDialogue
    public void _EVENT_StartOpeningDialogue() => Event_StartOpeningDialogueEvent();

    void Event_StartOpeningDialogueEvent()
    {
        StartCoroutine(TriggerDelayedEvent());
    }

    IEnumerator TriggerDelayedEvent()
    {
        yield return new WaitForSeconds(2f);
        GameHandler_PrototypeScene.instance._GoToDialogueMenuAndStartDialogue(StartOpeningDialogue, true, ResaltarOllaEvent);
    }
    #endregion
}
