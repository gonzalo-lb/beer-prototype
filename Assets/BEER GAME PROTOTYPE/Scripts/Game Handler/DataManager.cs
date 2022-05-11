using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAMESTATE : int { NONE, MAIN_MENU, PLAYING, DIALOGUE_SCREEN, PAUSED };
public enum EVENTSTATE : int { _01_OPENINGDIALOGUE,
                               _02_WAITING_DIALOGUE_TO_END,
                               _03_RESALTAR_OLLA,
                               _04_RESALTAR_HOLDER_OLLA,
                               _99_EVENTO_SIN_CREAR};

public enum OBJECTS : int { _01_OBJECT_INTERACTABLE_STANDARD,
                            _02_PURIFICADOR_LP,
                            _03_OLLA};

public class DataManager : MonoBehaviour
{
    // EXAMPLE VALUES
    public static int Example_value;
    public static int[] Example_array_value = { 99, 99, 99, 99 };

    // CONTROLLER VALUES
    public static float DM_CONTROLLER_movementSpeed = 1f;
    public static float DM_CONTROLLER_joystickRotationSpeed = 50f;
    public static float DM_CONTROLLER_gyroRotationSpeed = 100f;

    // GENERAL VALUES    
    public static GAMESTATE DM_GENERALVALUES_currentGameState = GAMESTATE.MAIN_MENU;
    public static EVENTSTATE DM_GENERALVALUES_currentEventState = EVENTSTATE._01_OPENINGDIALOGUE;

    public static void ResetVariables()
    {
        Example_value = 0;
        Example_array_value = new int[] { 99, 99, 99, 99 };        
    }
}
