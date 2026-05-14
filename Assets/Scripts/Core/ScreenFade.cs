using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    [SerializeField]  Image fadeImage;

    void Awake()
    {
        SetAlpha(1f); // start black
    }

    public IEnumerator FadeIn(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / duration);
            SetAlpha(alpha);

            timer += Time.deltaTime;
            yield return null;
        }
        SetAlpha(0f);
    }

    public IEnumerator FadeToBlack(float duration)
    {
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
