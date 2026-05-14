using UnityEngine;
using TMPro;

public class LanternTimer : MonoBehaviour
{
    private TMP_Text lanternTimerText;

    void Awake()
    {
        lanternTimerText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        lanternTimerText.text = "Lantern Timer " + Mathf.Ceil(PlayerState.Instance.lanternTimer).ToString();
    }
}