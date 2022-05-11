using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMovement : MonoBehaviour
{
    [SerializeField] GyroManager4 gyroManager;
    [SerializeField] Button_WalkRun buttonsManager;

    // Joystick
    public Joystick joystickL;
    public Joystick joystickR;
    public CharacterController charController;

    // Movement
    [SerializeField] float movementSpeed = 5f;

    // Rotation
    [SerializeField] float joystickRotationSpeed = 5f;
    [SerializeField] float gyroRotationSpeed = 5f;

    private void Start()
    {
        movementSpeed = DataManager.DM_CONTROLLER_movementSpeed;
        joystickRotationSpeed = DataManager.DM_CONTROLLER_joystickRotationSpeed;
        gyroRotationSpeed = DataManager.DM_CONTROLLER_gyroRotationSpeed;
        buttonsManager.ToggleWalkRun();
        Physics.IgnoreLayerCollision(9, 8);
    }

    void Update()
    {
        BodyMovementAndRotation();
    } // Update()

    void BodyMovementAndRotation()
    {
        if (MovementEnabled())
        {
            // Rotation
            float joystick_rotation = joystickR.Horizontal * joystickRotationSpeed * Time.deltaTime;
            float gyro_rotation = gyroManager.GetGyroRotationRateUnbiased_Y_AxisWithCorrection() * gyroRotationSpeed * Time.deltaTime;
            transform.Rotate(0, gyro_rotation, 0);
            transform.Rotate(0, joystick_rotation, 0);

            // Movement
            float x = joystickL.Horizontal;
            float z = joystickL.Vertical;
            Vector3 moveDirection = transform.right * x + transform.forward * z;
            if (buttonsManager.IsRunningBoolState())
            {
                charController.Move(moveDirection * movementSpeed * 2 * Time.deltaTime);
            }
            else
            {
                charController.Move(moveDirection * movementSpeed * Time.deltaTime);
            }
        } // if(MovementEnabled())  
    }

    bool MovementEnabled()
    {
        if(DataManager.DM_GENERALVALUES_currentGameState == GAMESTATE.PLAYING) { return true; }
        else { return false; }
    }
} // Class
