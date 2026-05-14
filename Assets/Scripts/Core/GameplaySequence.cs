using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplaySequence : MonoBehaviour
{
    public static GameplaySequence Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private ScreenFade screenFade;
    [SerializeField] private GameObject losePanel;
    
    private bool hasLost;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        losePanel.SetActive(false);
        AudioManager.Instance?.PlayMusic("GameplayBG");
        StartCoroutine(GameplayIntro());
    }

    private IEnumerator GameplayIntro()
    {
        yield return screenFade.FadeIn(3f);
        
        yield return Say(
        "Ugh... my head... what happened? It's s-s-so cold...",
        "GameplayDialogue1",
        0);
        
        yield return Say(
            "Gasp! The plane...it's completely wrecked! How am I supposed to get out of here? That crate...maybe there's something I can use?",
            "GameplayDialogue2",
            0);
        yield return new WaitForSeconds(3f);
    }

    private IEnumerator Say(string text, string voiceName, int speakerIndex)
    {
        bool done = false;

        dialogue.StartDialogue(text, voiceName, speakerIndex, () =>
        {
            done = true;
        });

        yield return new WaitUntil(() => done);
    }

    public void LoseGame()
    {
        Debug.Log("called");
        hasLost = true;

        AudioManager.Instance?.StopMusic();
        AudioManager.Instance?.StopVoiceLine();

        Time.timeScale = 0f;
        losePanel.SetActive(true);
    }
}