using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler_PrototypeScene : MonoBehaviour
{
    // Singleton
    public static GameHandler_PrototypeScene instance; // En este caso es el nombre de la Class, pero podría ser otro

    // Events
    [Header("EVENTS")]
    [SerializeField] GameEvent StartEvent;
    bool mustTurnOnTheNextEvent = false;
    GameEvent nextEventToTrigger;

    // Canvas
    [Header("CANVAS")]
    [SerializeField] GameObject playerHUDCanvasGO;
    [SerializeField] GameObject dialogueCanvasGO;      
        
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

        DebugMethod();
        
        playerHUDCanvasGO.SetActive(true);
        dialogueCanvasGO.SetActive(false);
    }

    private void Start()
    {
        StartEvent._Raise();
    }

    void DebugMethod()
    {
        DataManager.DM_GENERALVALUES_currentGameState = GAMESTATE.PLAYING;
    }

    #region Dialogos

    /// <summary>
    /// Es el único método que hay que utilizar para llamar al cuadro de diálogo, e iniciar un diálogo. 
    /// Maneja el GameState, activa o desactiva los Canvas (o sus GameObjects) que correspondan, y se 
    /// comunica con el Dialogue Manager para pasarle el diálogo que tiene que iniciar. 
    /// Cuando el Dialogue Manager termina de pasar las oraciones, llama al método ReturnFromDialogueMenu() 
    /// en esta Class. Solo funciona si el GameState está en "PLAYING"
    /// </summary>
    /// <param name="dialogue">Es el diálogo que se va a mostrar en pantalla</param>
    public void _GoToDialogueMenuAndStartDialogue(Dialogue dialogue, bool triggerEventWhenfinish, GameEvent eventToTrigger)
    {
        if (DataManager.DM_GENERALVALUES_currentGameState != GAMESTATE.PLAYING)
        {
            Debug.LogWarning("Se intentó iniciar un diálogo en un GameState distinto de PLAYING");
            return;
        }
        
        DataManager.DM_GENERALVALUES_currentGameState = GAMESTATE.DIALOGUE_SCREEN;
        playerHUDCanvasGO.SetActive(false);
        dialogueCanvasGO.SetActive(true);
        DialogueManager.instance.StartDialogue(dialogue);

        if (triggerEventWhenfinish) { mustTurnOnTheNextEvent = true; nextEventToTrigger = eventToTrigger; }
    }    

    public void _ReturnFromDialogueMenu()
    {
        if (DataManager.DM_GENERALVALUES_currentGameState != GAMESTATE.DIALOGUE_SCREEN) { return; } //Solo se puede iniciar un di�logo si est�s jugando
        DataManager.DM_GENERALVALUES_currentGameState = GAMESTATE.PLAYING;
        dialogueCanvasGO.SetActive(false);
        playerHUDCanvasGO.SetActive(true);

        if (mustTurnOnTheNextEvent) { nextEventToTrigger._Raise(); }
        mustTurnOnTheNextEvent = false;
    }

    #endregion

      
}
