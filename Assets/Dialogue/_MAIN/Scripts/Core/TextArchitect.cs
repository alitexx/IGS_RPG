using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class TextArchitect
{
    private TextMeshProUGUI tmpro_ui;
    private TextMeshPro tmpro_world;
    public TMP_Text tmpro => tmpro_ui != null ? tmpro_ui : tmpro_world;
    public string currentText => tmpro.text; // whatever text we currently have
    public string targetText { get; private set; } = ""; // what we're trying to build
    public string preText { get; private set; } = ""; //whatever is already on the architext will be stored in this variable before new text
    //private int preTextLength = 0;

    public string fullTargetText => preText + targetText; // full length of the text

    public enum BuildMethod { instant, typewriter} // does text pop up instantly, or is it typed?
    public BuildMethod buildMethod = BuildMethod.typewriter; // default to typed text (typewriter)

    public Color textColor { get { return tmpro.color; } set { tmpro.color = value; } } // color of the text

    public float speed { get { return baseSpeed * speedMultiplier; } set { speedMultiplier = value; } }
    private const float baseSpeed = 1;
    public float speedMultiplier = 1; //changed from config menu!!

    public int charactersPerCycle { get { return speed <= 2f ? characterMultiplier : speed <= 2.5f ? characterMultiplier * 2 : characterMultiplier * 3; } }
    private int characterMultiplier = 1;

    public bool hurryUp = false; // when true, text moves at double speed

    public GameObject continueButton;

    public AudioSource audioPlayer;
    public TextArchitect(TextMeshProUGUI tmpro_ui) // if we give it a tmproUGUI
    {
        this.tmpro_ui = tmpro_ui;
    }
    public TextArchitect(TextMeshPro tmpro_world) // if we give it a tmpro
    {
        this.tmpro_world = tmpro_world;
    }

    public Coroutine Build(string text) // builds the text
    {
        preText = "";
        targetText = text;
        
        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    public Coroutine Append(string text) // appends text already in the architext
    {
        preText = tmpro.text;
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }
    private Coroutine playAudio = null;
    private Coroutine buildProcess = null; //handles text generation
    public bool isBuilding => buildProcess != null;

    public void Stop() // stops text if we are currently building text
    {
        if (!isBuilding) { return; }
        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;
    }

    IEnumerator Building()
    {
        Prepare();
        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                yield return Build_Typewriter();
                break;
            case BuildMethod.instant:
                break;
        }
        yield return null;
    }

    private void OnComplete()
    {
        buildProcess = null;
        hurryUp = false;
        continueButton.SetActive(true);
    }

    public void ForceComplete()
    {
        switch (buildMethod)
        {
            case BuildMethod.instant:
                break;
            case BuildMethod.typewriter:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;
        }
        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;
        OnComplete();
    }

    private void Prepare() // prepare text based on the build method (for no glitches)
    {
        switch (buildMethod)
        {
            case BuildMethod.instant:
                Prepare_Instant();
                break;
            case BuildMethod.typewriter:
                Prepare_Typewriter();
                break;
        }
    }

    private void Prepare_Instant()
    {
        //tmpro.color = tmpro.color; // if I need this
        tmpro.text = fullTargetText;
        tmpro.ForceMeshUpdate();
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;

    }
    private void Prepare_Typewriter()
    {
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preText;
        if (preText != "") // if pretest is NOT null
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }
        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();
    }

    private IEnumerator Build_Typewriter()
    {
        playAudio = tmpro.StartCoroutine(playTalking());
        while (tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount) // while we still have text to display
        {
            tmpro.maxVisibleCharacters += hurryUp ? charactersPerCycle * 5 : charactersPerCycle;
            yield return new WaitForSeconds(0.015f / speed);
        }
        tmpro.StopCoroutine(buildProcess);   
        buildProcess = null;
        OnComplete();
    }

    private IEnumerator playTalking()
    {
        while (tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount) // while we still have text to display
        {
            audioPlayer.Play();
            yield return new WaitForSeconds(0.05f / speed);
        }
        tmpro.StopCoroutine(playAudio);
    }
}
