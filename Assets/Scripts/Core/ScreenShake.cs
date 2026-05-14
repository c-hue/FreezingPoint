using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 ogPos;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ogPos = rectTransform.anchoredPosition;
    }

    public void Shake(float duration, float strength)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            rectTransform.anchoredPosition = ogPos;
        }
        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, strength));
    }

    public IEnumerator ShakeRoutine(float duration, float strength)
    {
        Debug.Log("shaking");
        float timer = 0f;

        while (timer < duration)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;
            rectTransform.anchoredPosition = ogPos + new Vector3(x, y, 0f);
            Debug.Log(transform.position);
            
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = ogPos;
        shakeCoroutine = null;
    }
}
