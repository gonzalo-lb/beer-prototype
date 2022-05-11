using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BottleControlCenterPivot : MonoBehaviour
{
    // COMO CALCULAR LAS CURVAS:

    // Primero calcular el SARM. Para eso, poner el mesh a 0º, y vaciarlo. Luego, ponerlo a 90º, y 
    // mover el SARM hasta que esté en el punto justo donde está nuevamente vacío. En la curva, ingresar 2 
    // valores: a 0º --> 1; a 90º --> lo que haya dado el SARM para que esté vacío.

    // Cálculo de la Fill Curve:
    // Usar la parte del script que actualiza la curva del SARM al rotar. Con eso andando, ir rotando
    // el mesh de a 18º y poner el FillValue justo para que desborde. Ese FillValue es el que tiene que
    // ir en la curva a 0º, 18º, 36º, 54º, 72º, y 90º

    // Luego, hay que hacer algún clamp, o que pasado cierto valor lleve el FillValue más lejos
    // porque sino con la olla vacía dependiendo de cómo se la rote aparece algo de agua

    // VALORES DE LA CURVA para olla 30 litros con pivot en el centro (SE tiene en cuenta el SARM
    // a 0º = 1; 90º = 0.92)
    //ROT FillAmount
    //0 	0.171 full
    //18	0.114
    //36	0.045    
    //54	-0.029
    //72	-0.105
    //90	-0.172

    //AguaSizeFullValue = 0.171f;
    //AguaSizeEmptyValue = -0.172f;

    [Header("REFERENCES")]
    [SerializeField] MeshRenderer AguaMeshRenderer;
    [SerializeField] Collider AguaCollider;
    [Tooltip("Es un transform de referencia para calcular el ángulo de rotación del mesh. Hay que poner un empty como child del mesh que hace de agua. Al iniciar su posición se calcula de forma automática.")]
    [SerializeField] Transform dummy;
    
    [Header("CURVES")]
    [SerializeField] AnimationCurve FillAmountCurve;
    [SerializeField] AnimationCurve ScaleAndRotationMultiplierCurve;

    [Header("START SETUP")]
    [Tooltip("0 = Empty - 1 = Full")]
    [SerializeField] float startFillValue = 0.5f;
    [SerializeField] bool calculateEmptyFullValues = true;
    [SerializeField] float AguaSizeHeight;
    [SerializeField] float AguaSizeFullValue;
    [SerializeField] float AguaSizeEmptyValue;
    float AguaSizeEmptyTimes3;

    [Header("FOR DEBUG - DO NOT TOUCH!")]
    [SerializeField] float currentFillAmount; // En valores del shader
    [SerializeField] float currentFillAmount01; // En valores relativos (0 = Vacío; 1 = Lleno)
    [SerializeField] bool updateFillEnabled;

    Quaternion prevRotation;

    #region Funcionamiento base

    void Start()
    {
        // Setup the curves
        FillAmountCurve.AddKey(0, 0.171f);
        FillAmountCurve.AddKey(18f, 0.114f);
        FillAmountCurve.AddKey(36f, 0.045f);        
        FillAmountCurve.AddKey(54f, -0.029f);
        FillAmountCurve.AddKey(72f, -0.105f);
        FillAmountCurve.AddKey(90f, -0.172f);

        ScaleAndRotationMultiplierCurve.AddKey(0f, 1f);
        ScaleAndRotationMultiplierCurve.AddKey(90f, 0.92f);

        float tempAguaSizeHeight = AguaCollider.bounds.size.y;

        // Calculate Full and empty Values for this mesh (optional)
        if (calculateEmptyFullValues)
        {
            AguaSizeHeight = tempAguaSizeHeight;
            AguaSizeFullValue = AguaSizeHeight / 2;
            AguaSizeEmptyValue = -AguaSizeFullValue;
        }

        AguaSizeEmptyTimes3 = AguaSizeEmptyValue * 3f;

        // Calcula la posición del DummyTransform para que quede al centro, y en la parte de arriba del mesh
        Vector3 tempDummyPos = Vector3.zero;
        tempDummyPos.y += tempAguaSizeHeight / 2;
        dummy.localPosition = tempDummyPos;
        
        float tempStartFillValue = Mathf.Clamp01(startFillValue);
        _SetFillValue01(tempStartFillValue);        

        prevRotation = transform.rotation;        

        updateFillEnabled = true;

        _UpdateFillFromRotation();
    }
        
    void Update()
    {

        if (transform.rotation != prevRotation)
        {
            _UpdateFillFromRotation();
        }

        prevRotation = transform.rotation;        
    }

    void _UpdateFillFromRotation()
    {
        if (!updateFillEnabled) { return; }

        float currentAngleRotation = _GetCurrentWaterAngle();        

        _EvaluateScaleAndRotationMultiplier(currentAngleRotation);

        _EvaluateFillAmount(currentAngleRotation);
    }

    void _EvaluateFillAmount(float currentAngleRotation)
    {
        float tempFillAmount = FillAmountCurve.Evaluate(Mathf.Abs(currentAngleRotation));                

        if (tempFillAmount < currentFillAmount)
        {
            currentFillAmount01 = Library._RemapTargetTo01(AguaSizeEmptyValue, AguaSizeFullValue, tempFillAmount);
            currentFillAmount = tempFillAmount;
            AguaMeshRenderer.material.SetFloat("_FillAmount", currentFillAmount);
        }

        // Esto se hace para que al estar vacío igual no quede un fondo en los ángulos inferiores del
        // recipiente
        //if (currentAngleRotation > 87f || currentFillAmount01 < 0.5f) { AguaMeshRenderer.material.SetFloat("_FillAmount", AguaSizeEmptyTimes3); }
        if (currentAngleRotation > 87f || currentFillAmount01 < 0.03f) { AguaMeshRenderer.enabled = false; }
        else { AguaMeshRenderer.enabled = true; }
    }

    void _EvaluateScaleAndRotationMultiplier(float currentAngleRotation)
    {
        float SARMCurveValue = ScaleAndRotationMultiplierCurve.Evaluate(currentAngleRotation);        
        AguaMeshRenderer.material.SetFloat("_ScaleAndRotationMultiplier", SARMCurveValue);
    }

    /// <summary>
    /// Toma valores relativos (0: Vacío; 1: Lleno) y devuelve el valor que corresponde a _FillAmount del Shader
    /// </summary>
    /// <param name="fillAmount">Ingresar fillAmount entre 0-1, y devuelve el valor remapeado entre los valores de vacio y lleno de ese mesh.</param>
    /// <returns></returns>
    float _FillAmountRemapped(float fillAmount)
    {
        fillAmount = Mathf.Clamp01(fillAmount);
        return Library._Remap01ToTarget(AguaSizeEmptyValue, AguaSizeFullValue, fillAmount);
    }

    /// <summary>
    /// Toma el valor en formato del shader (_FillAmount) y devuelve un valor 01 relativo a si está lleno o vacío.
    /// </summary>
    /// <param name="relativeValue">Ingresar _FillAmount del shader, y devuelve el valor remapeado entre entre 0 y 1</param>
    /// <returns></returns>
    float FillAmountRelativeRemapped(float relativeValue)
    {        
        return Library._RemapTargetTo01(AguaSizeEmptyValue, AguaSizeFullValue, relativeValue);
    }

    float _GetCurrentWaterAngle()
    {
        Vector3 from = dummy.position - transform.position;
        Vector3 to = Vector3.up;
        float angle = Vector3.Angle(from, to);        
        return angle;
    }

    #endregion

    #region Métodos públicos (comunicación)

    /// <summary>
    /// Establece un nuevo valor de llenado. Se renderiza en el momento. Luego, se calculan los valores 
    /// de llenado según la rotación actual
    /// </summary>
    /// <param name="fillValue01">0 = vacío; 1 = lleno</param>
    public void _SetFillValue01(float fillValue01)
    {
        float newFillValue = _FillAmountRemapped(fillValue01);
        AguaMeshRenderer.material.SetFloat("_FillAmount", newFillValue);
        currentFillAmount = newFillValue;
        currentFillAmount01 = FillAmountRelativeRemapped(newFillValue);
        AguaMeshRenderer.material.SetFloat("_FillAmount", currentFillAmount);
        _UpdateFillFromRotation();
    }

    public float _GetFillValue01() { return currentFillAmount01; }

    public void _AddWater01(float waterAmount01)
    {
        currentFillAmount01 += waterAmount01;
        if (currentFillAmount01 > 1f) { currentFillAmount01 = 1f; }
        currentFillAmount = _FillAmountRemapped(currentFillAmount01);
        AguaMeshRenderer.material.SetFloat("_FillAmount", currentFillAmount);
        _EvaluateFillAmount(_GetCurrentWaterAngle());
    }

    public void _SubtractWater01(float waterAmount01)
    {
        currentFillAmount01 -= waterAmount01;
        if (currentFillAmount01 < 0f) { currentFillAmount01 = 0f; }
        currentFillAmount = _FillAmountRemapped(currentFillAmount01);
        AguaMeshRenderer.material.SetFloat("_FillAmount", currentFillAmount);
        _EvaluateFillAmount(_GetCurrentWaterAngle());
    }

    #endregion
}
