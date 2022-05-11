using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler_PrototypeSceneSimple : MonoBehaviour
{    
    void Start()
    {
        DataManager.DM_GENERALVALUES_currentGameState = GAMESTATE.PLAYING;
    }    
}
