using UnityEngine;

public class Region : MonoBehaviour
{

    void Start()
    {
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!RegionSystem.Instance.region1Enter && RegionSystem.Instance.region2Enter)
            {
                PlayerState.Instance.lanternTimer = 120f;
                RegionSystem.Instance.region1Enter = true;
                RegionSystem.Instance.region2Enter = false;
                PlayerState.Instance.warmedUp = true;
            }
            if (!RegionSystem.Instance.region2Enter && RegionSystem.Instance.region3Enter) 
            {
                RegionSystem.Instance.region2Enter = true;
                RegionSystem.Instance.region3Enter = false;
            }
            if (!RegionSystem.Instance.region3Enter && RegionSystem.Instance.region4Enter)
            {
                RegionSystem.Instance.region3Enter = true;
                RegionSystem.Instance.region4Enter = false;
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.name == "Region1")
            {
                RegionSystem.Instance.region1Enter = false;
                RegionSystem.Instance.region2Enter = true;
            } 

            if (gameObject.name == "Region2")
            {
                RegionSystem.Instance.region2Enter = false;
                RegionSystem.Instance.region3Enter = true;
            }

            if (gameObject.name == "Region3")
            {
                RegionSystem.Instance.region3Enter = false;
                RegionSystem.Instance.region4Enter = true;
            }
        }
    }
}