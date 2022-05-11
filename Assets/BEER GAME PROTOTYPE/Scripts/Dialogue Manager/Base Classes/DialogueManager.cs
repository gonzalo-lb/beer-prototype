using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    //SINGLETON
    public static DialogueManager instance;

    public TextMeshProUGUI textDisplay;    
    private Queue<string> sentences;    
    public float typingSpeed;
    public float alphaPercentaje;
    public float alphaSpeed;
    public float alphaSpeedInSeconds;
        
    public GameObject continueButton;
    //private AudioSource sound;

    string currentSentence;
        
    // Dialogos
    public Dialogue dialogos_DebugDialogue;
    public Dialogue dialogue_event01;
    public Dialogue dialogue_event03;

        

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

        typingSpeed = 0.02f;
        alphaSpeedInSeconds = 1;
        //sound = GetComponent<AudioSource>();
        sentences = new Queue<string>();        
    }    

    /// <summary>
    /// Inicia las corutinas para que se escriba el texto. 
    /// NO maneja los Canvas. El PlayerHUD lo tiene que desactivar quien invoque este método, y también
    /// tiene que activar el Canvas (o su GameObject) del diálogo. 
    /// Este método tampoco maneja el GameState
    /// </summary>
    /// <param name="_dialogue">Si bien se le puede pasar cualquier Dialogue -class-, sería ideal guardar en el Dialogue Manager todos los diálogos del juego</param>
    public void StartDialogue(Dialogue _dialogue)
    {
        sentences.Clear();
        foreach (string sentence in _dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }        
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndTypeText();
            return;
        }
        //sound.Play();
        continueButton.SetActive(false);
        currentSentence = sentences.Dequeue();
        textDisplay.text = "";
        StartCoroutine(Type());
        StartCoroutine(TextFadeIn());
    }
    
    IEnumerator Type()
    {
        foreach(char letter in currentSentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        continueButton.SetActive(true);
    }

    IEnumerator TextFadeIn()
    {
        alphaPercentaje = 0;        
        while(alphaPercentaje < 1)
        {
            textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, alphaPercentaje);
            alphaPercentaje += Time.deltaTime / alphaSpeedInSeconds;
            yield return null;
        }        
    }    

    void EndTypeText()
    {
        textDisplay.text = "";        
        continueButton.SetActive(false);
        GameHandler_PrototypeScene.instance._ReturnFromDialogueMenu();        
    }
    
}
