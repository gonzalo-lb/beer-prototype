using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        OnLoadThisScene();
    }

    public void OnLoadThisScene()
    {
        DataManager.DM_GENERALVALUES_currentGameState = GAMESTATE.MAIN_MENU;
    }
}
