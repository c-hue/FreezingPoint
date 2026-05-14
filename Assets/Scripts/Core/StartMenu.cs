using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{   
    [SerializeField] private ScreenFade screenFade;
    void Start()
    {
        AudioManager.Instance?.PlayMusic("TitleScreenBG");
    }
    
    public void PlayGame()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return screenFade.FadeToBlack(1f);
        SceneManager.LoadSceneAsync("Intro");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}