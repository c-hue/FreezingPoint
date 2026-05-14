using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] Image fadeImage;

    void Awake()
    {
        fadeImage.gameObject.SetActive(false); // start inactive
    }

    public IEnumerator FadeIn(float duration)
    {
        SetAlpha(1f);
        fadeImage.gameObject.SetActive(true);
        float timer = 0f;

        while (timer < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / duration);
            SetAlpha(alpha);

            timer += Time.deltaTime;
            yield return null;
        }
        SetAlpha(0f);
        fadeImage.gameObject.SetActive(false);
    }

    public IEnumerator FadeToBlack(float duration)
    {
        SetAlpha(0f);
        fadeImage.gameObject.SetActive(true);
        float timer = 0f;
        while (timer < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer / duration);
            SetAlpha(alpha);

            timer += Time.deltaTime;
            yield return null;
        }
        SetAlpha(1f);
    }

    void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}
