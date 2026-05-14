using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class Dialogue : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] float textSpeed = 20f;

    [Header("Speaker")]
    [SerializeField] string[] speakers;
    [SerializeField] TextMeshProUGUI speakerText;

    private string currLine;
    public bool isTyping;
    private Action onDialogueComplete;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        // when user clicks left button, skip line
        if(Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                textComponent.text = currLine;
                isTyping = false;
            }
            else
            {
                CloseDialogue();
            }
        }
    }

    public void StartDialogue(string text, string voiceName, int speakerIndex, Action onComplete = null)
    {
        gameObject.SetActive(true);
        currLine = text;
        onDialogueComplete = onComplete;

        textComponent.text = "";
        speakerText.text = speakers[speakerIndex];

        AudioManager.Instance?.LowerVolume();
        AudioManager.Instance?.PlayVoiceLine(voiceName);

        StartCoroutine(TypeLine());
    }

    // type each character one by one
    IEnumerator TypeLine()
    {
        isTyping = true;
        foreach (char c in currLine)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    // exit dialogue screen
    void CloseDialogue()
    {
        gameObject.SetActive(false);
        AudioManager.Instance?.StopVoiceLine();
        onDialogueComplete?.Invoke();
    }
}