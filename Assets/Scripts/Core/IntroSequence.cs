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
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        yield return screenFade.FadeIn(2.5f);

        yield return Say("Air Command, this is Alpha-91, radio check. Do you copy? Over.", 0);

        yield return Say("Alpha-91, this is Air Command 851. We read you loud and clear. Confirm you are en route to designated LZ.", 1);

        yield return Say("Affirmative, en route to landing zone. Current ETA 14:25. Requesting weather update on approach.", 0);

        yield return Say("Alpha-91, weather reports indicate minimal activity. Flight path is clear and stable. You are good to proceed.", 1);

        yield return Say("Roger... stand by. I'm picking up moderate turbulence ahead. Can you reconfirm conditions?", 0);

        screenShake.Shake(1.2f, 100f);
        yield return new WaitForSeconds(1.2f);

        yield return Say("Alpha-91... we're getting--static--advise immediate—static—adjust altitude... do you copy", 1);

        screenShake.Shake(2f, 100f);
        yield return new WaitForSeconds(1.8f);

        yield return Say("Air Command, you're breaking up! I've lost visual, heavy snow and wind incoming! Mayday, mayday, mayday! Going down!", 0);

        yield return screenFade.FadeToBlack(1f);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Gameplay");
    }

    private IEnumerator Say(string text, int speakerIndex)
    {
        bool done = false;

        dialogue.StartDialogue(text, speakerIndex, () =>
        {
            done = true;
        });

        yield return new WaitUntil(() => done);
    }
}