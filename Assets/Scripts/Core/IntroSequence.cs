using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSequence : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private ScreenShake screenShake;
    [SerializeField] private ScreenFade screenFade;

    private void Start()
    {
        AudioManager.Instance?.PlayMusic("IntroBG");
        AudioManager.Instance?.PlayOneShot("IntroSound");
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        yield return screenFade.FadeIn(3f);

        yield return Say(
            "Air Command, this is Alpha-91, radio check. Do you copy? Over.",
            "IntroDialogue1",
            0);

        yield return Say(
            "Alpha-91, this is Air Command 851. We read you loud and clear. Confirm you are en route to designated LZ.",
            "IntroDialogue2",
            1);

        yield return Say(
            "Affirmative, en route to landing zone. Current ETA 14:25. Requesting weather update on approach.",
            "IntroDialogue3",
            0);

        yield return Say(
            "Alpha-91, weather reports indicate minimal activity. Flight path is clear and stable. You are good to proceed.",
            "IntroDialogue4",
            1);

        yield return Say(
            "Roger... stand by. I'm picking up moderate turbulence ahead. Can you reconfirm conditions?",
            "IntroDialogue5",
            0);
        AudioManager.Instance?.PlayOneShot("Turbulence");
        screenShake.Shake(1.2f, 10f);
        yield return new WaitForSeconds(1.2f);

        yield return Say(
            "Alpha-91... we're getting--static--advise immediate—-static—-adjust altitude... do you copy?",
            "IntroDialogue6",
            1);
        AudioManager.Instance?.PlayOneShot("Turbulence");
        screenShake.Shake(2f, 10f);
        yield return new WaitForSeconds(1.8f);

        yield return Say(
            "Air Command, you're breaking up! I've lost visual, heavy snow and wind incoming! Mayday, mayday, mayday! Going down!",
            "IntroDialogue7",
            0);

        AudioManager.Instance?.PlayOneShot("Turbulence");
        screenShake.Shake(1f, 10f);
        yield return screenFade.FadeToBlack(1f);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Gameplay 1");
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