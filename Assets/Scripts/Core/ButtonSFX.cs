using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public void PlaySFX()
    {
        AudioManager.Instance?.PlayOneShot("ButtonClick", this.transform.position);
    }
}
