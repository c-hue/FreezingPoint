using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplaySequence : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Dialogue dialogue;
    //[SerializeField] private ScreenShake cameraShake;
    [SerializeField] private ScreenFade screenFade;

    private void Start()
    {
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
}