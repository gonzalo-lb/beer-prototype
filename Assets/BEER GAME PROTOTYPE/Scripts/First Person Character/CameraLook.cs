using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    
    [SerializeField] GyroManager4 gyroManager;    
    public Joystick joystickR;
    [SerializeField] Button_XZToggle buttonsManager;

    [SerializeField] float joystickRotationSpeed = 5f;
    [SerializeField] float gyroRotationSpeed = 5f;

    private void Start()
    {
        joystickRotationSpeed = DataManager.DM_CONTROLLER_joystickRotationSpeed;
        gyroRotationSpeed = DataManager.DM_CONTROLLER_gyroRotationSpeed;
    }

    void Update()
    {
        CameraLookRotation();
    }

    void CameraLookRotation()
    {
        if (MovementEnabled())
        {
            if (buttonsManager.XZToggleBoolState())
            {
                transform.Rotate(gyroManager.GetGyroRotationRateUnbiased_XZ_AxisWithCorrection() * gyroRotationSpeed * Time.deltaTime);
            }
            else
            {
                float gyro_rotation = gyroManager.GetGyroRotationRateUnbiased_X_AxisWithCorrection() * gyroRotationSpeed * Time.deltaTime;
                Vector3 thisRotation = new Vector3(gyro_rotation, 0, 0);
                transform.Rotate(thisRotation);
            }

            float joystick_rotation = joystickR.Vertical * joystickRotationSpeed * Time.deltaTime;
            transform.Rotate(-joystick_rotation, 0, 0);
        }
    }

    bool MovementEnabled()
    {
        if (DataManager.DM_GENERALVALUES_currentGameState == GAMESTATE.PLAYING) { return true; }
        else { return false; }
    }
}
