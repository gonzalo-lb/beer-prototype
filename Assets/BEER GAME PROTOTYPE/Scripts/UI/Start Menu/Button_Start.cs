using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Start : MonoBehaviour
{
    public void LoadPrototypeGameScene()
    {
        SceneManager.LoadScene(TagHelper_SCENES.PROTOTYPE_GAME_SCENE);
        DataManager.DM_GENERALVALUES_currentGameState = GAMESTATE.PLAYING;
    }
}
