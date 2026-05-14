using UnityEngine;

public class RegionSystem : MonoBehaviour
{
    public static RegionSystem Instance { get; set; }
    public bool region1Enter;
    public bool region2Enter;
    public bool region3Enter;
    public bool region4Enter;
    public string regionName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        region1Enter = true;
        region2Enter = false;
        region3Enter = false;
        region4Enter = false;
    }

    void Update()
    {
        if (region1Enter)
        {
            PlayerState.Instance.inFreezing1 = false;
            PlayerState.Instance.inFreezing2 = false;
            PlayerState.Instance.inFreezing3 = false;
        }

        if (region2Enter)
        {
            PlayerState.Instance.inFreezing1 = true;
            PlayerState.Instance.inFreezing2 = false;
            PlayerState.Instance.inFreezing3 = false;
        }

        if (region3Enter)
        {
            PlayerState.Instance.inFreezing1 = false;
            PlayerState.Instance.inFreezing2 = true;
            PlayerState.Instance.inFreezing3 = false;
        }
        if (region4Enter)
        {
            PlayerState.Instance.inFreezing1 = false;
            PlayerState.Instance.inFreezing2 = false;
            PlayerState.Instance.inFreezing3 = true;
        }
    }
}