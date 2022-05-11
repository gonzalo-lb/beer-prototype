using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager4 : MonoBehaviour
{
    float xRotValue;
    float yRotValue;
    float zRotValue;
    float xRotValueUB;
    float yRotValueUB;
    float zRotValueUB;

    // Start is called before the first frame update
    void Awake()
    {
        Input.gyro.enabled = true;        
    }

    // Update is called once per frame
    void Update()
    {
        SetRotationValuesAndApplyCorrection();        
    }

    void SetRotationValuesAndApplyCorrection()
    {
        xRotValue = -Input.gyro.rotationRate.x;
        yRotValue = -Input.gyro.rotationRate.y;
        zRotValue = Input.gyro.rotationRate.z;

        xRotValueUB = -Input.gyro.rotationRateUnbiased.x;
        yRotValueUB = -Input.gyro.rotationRateUnbiased.y;
        zRotValueUB = Input.gyro.rotationRateUnbiased.z;
    }

    public Vector3 GetGyroRotationRateWithCorrection()
    {
        return new Vector3(xRotValue, yRotValue, zRotValue);
    }

    public Vector3 GetGyroRotationRateUnbiasedWithCorrection()
    {
        return new Vector3(xRotValueUB, yRotValueUB, zRotValueUB);
    }

    public Vector3 GetGyroRotationRate_XZ_AxisWithCorrection()
    {
        return new Vector3(xRotValue, 0, zRotValue);
    }

    public Vector3 GetGyroRotationRateUnbiased_XZ_AxisWithCorrection()
    {
        return new Vector3(xRotValueUB, 0, zRotValueUB);
    }

    public float GetGyroRotationRateUnbiased_X_AxisWithCorrection()
    {
        return xRotValueUB;
    }

    public float GetGyroRotationRateUnbiased_Y_AxisWithCorrection()
    {
        return yRotValueUB;
    }

    public float GetGyroRotationRateUnbiased_Z_AxisWithCorrection()
    {
        return zRotValueUB;
    }

    public float GetGyroRotationRate_X_AxisWithCorrection()
    {
        return xRotValue;
    }

    public float GetGyroRotationRate_Y_AxisWithCorrection()
    {
        return yRotValue;
    }

    public float GetGyroRotationRate_Z_AxisWithCorrection()
    {
        return zRotValue;
    }
}
