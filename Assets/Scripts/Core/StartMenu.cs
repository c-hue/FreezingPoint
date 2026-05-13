using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{   
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Intro");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}