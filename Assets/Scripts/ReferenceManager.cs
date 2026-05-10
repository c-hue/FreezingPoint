using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager instance;

    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        // Singleton setup
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Canvas getCanvasReference()
    {
        return canvas;
    }
}