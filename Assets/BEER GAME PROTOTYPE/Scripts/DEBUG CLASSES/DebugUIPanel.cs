using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIPanel : MonoBehaviour
{
    [Header("UI Variables")]
    [SerializeField] Text volumenText;
    [SerializeField] Text temperaturaText;
    [SerializeField] Text masaDeAguaText;
    [SerializeField] Text masaDeAzucarText;
    [SerializeField] Text masaTotalText;
    [SerializeField] Text densidadText;
    [SerializeField] Text azucarExcedenteText;
    [SerializeField] Text colorText;
    [SerializeField] Text capacidadDelRecipienteText;
    [SerializeField] Text lupuloText;
    [SerializeField] Text granoText;
    [SerializeField] Text granoMaceradoText;

    [Header("Olla")]
    [SerializeField] Object_Olla2 olla;

    float updateUIRate; // in seconds
    bool updateUIEnabled;


    // Start is called before the first frame update
    void Start()
    {
        updateUIRate = 0.25f;
        updateUIEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (updateUIEnabled) { StartCoroutine(UpdateUI()); }
    }

    IEnumerator UpdateUI()
    {
        updateUIEnabled = false;

        volumenText.text = "Volumen = " + olla._GetVolumenDeLiquido().ToString();
        temperaturaText.text = "Temperatura = " + olla._GetTemperatura().ToString();
        masaDeAguaText.text = "Masa de agua = " + olla._GetMasaDeAgua().ToString();
        masaDeAzucarText.text = "Masa de azucar = " + olla._GetMasaDeAzucar().ToString();
        masaTotalText.text = "Masa total = " + olla._GetMasaTotal().ToString();
        densidadText.text = "Densidad = " + olla._GetDensidad().ToString();
        azucarExcedenteText.text = "Azucar excedente = " + olla._GetAzucarExcedente().ToString();
        colorText.text = "Color = " + olla._GetColor().ToString();
        capacidadDelRecipienteText.text = "Capacidad del recipiente = " + olla._GetCapacidadDelRecipiente().ToString();
        lupuloText.text = "Lúpulo = " + olla._GetLupulo().ToString();
        granoText.text = "Grano = " + olla._GetGrano().ToString();
        granoMaceradoText.text = "Grano macerado = " + olla._GetGranoMacerado().ToString();

        yield return new WaitForSeconds(updateUIRate);

        updateUIEnabled = true;
    }
}
