using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RegionManager : MonoBehaviour
{
    [Header("Center")]
    [SerializeField] Transform campsite;

    [Header("Regions")]
    [SerializeField] List<RegionDefinition> regions = new();

    [Header("Spawn Settings")]
    [SerializeField] private Transform resourceContainer;
    [SerializeField] private LayerMask blockedLayers;
    [SerializeField] private int maxSpawnAttempts = 40;

    private Dictionary<string, int> activeResourceCounts = new Dictionary<string, int>();
   
    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        foreach (RegionDefinition region in regions)
        {
            SpawnResourcesForRegion(region);
        }
    }

// --- Resources ----------------------------------------------------------------------------------------------------

    // Spawn each resource that resides in region based on max count
    void SpawnResourcesForRegion(RegionDefinition region)
    {
        foreach (ResourceSpawnRule rule in region.resourceRules)
        {
            string key = GetResourceKey(region,rule);
            if (!activeResourceCounts.ContainsKey(key))
            {
                activeResourceCounts[key] = 0;
            }

            while (activeResourceCounts[key] < rule.maxCount)
            {
                SpawnResource(region, rule);
            }
        }
    }

    void SpawnResource(RegionDefinition region, ResourceSpawnRule rule)
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Debug.Log("spawning " + rule + rule.resourceID + " for " + region.regionName);
            Vector3 pos = GetRandomPoint(region);
            pos = PlaceOnTerrain(pos);

            if (!isClear(pos, rule.radius, blockedLayers)) continue;

            Quaternion rotation = rule.randomRotation ? Quaternion.Euler(0f, Random.Range(0f, 360f), 0f) : Quaternion.identity;

            GameObject prefab = rule.prefabs[Random.Range(0,rule.prefabs.Count)];
            GameObject obj = Instantiate(prefab, pos, rotation, resourceContainer);
            
            Debug.Log(prefab);
            InteractableObject interactable = obj.GetComponent<InteractableObject>();
            Debug.Log(interactable);
            interactable.SetupProceduralResource(this, region, rule);
            string key = GetResourceKey(region, rule);
            activeResourceCounts[key]++;

            return;
        }
    }

    // Subtract from count when resource is collected
    public void OnResourceCollected(RegionDefinition region, ResourceSpawnRule rule)
    {
        string key = GetResourceKey(region, rule);

        if (activeResourceCounts.ContainsKey(key))
        {
            activeResourceCounts[key]--;
            activeResourceCounts[key] = Mathf.Max(activeResourceCounts[key], 0);
        }
        StartCoroutine(RespawnResource(region, rule));
    }

    // Start respawn timer for resource
    IEnumerator RespawnResource(RegionDefinition region, ResourceSpawnRule rule)
    {
        yield return new WaitForSeconds(rule.respawnTime);
        string key = GetResourceKey(region, rule);
        if (activeResourceCounts[key] < rule.maxCount)
        {
            SpawnResource(region, rule);
        }
    }

    // Make key from region + resource
    string GetResourceKey(RegionDefinition region, ResourceSpawnRule rule)
    {
        return $"{region.regionName}_{rule.resourceID}";
    }

// --- Position Helpers ----------------------------------------------------------------------------------------------------

    // Get random point within region boundaries
    Vector3 GetRandomPoint(RegionDefinition region)
    {
        float radius = Random.Range(region.innerRadius, region.outerRadius);
        float angle = Random.Range(0f, Mathf.PI * 2f);

        Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);

        return campsite.position + offset;
    }

    // Place objects on terrain height 
    Vector3 PlaceOnTerrain(Vector3 position)
    {
        position.y = Terrain.activeTerrain.SampleHeight(position);
        return position;
    }

    // Prevent objects spawning into each other
    bool isClear(Vector3 position, float radius, LayerMask blockedLayers)
    {
        return !Physics.CheckSphere(position, radius, blockedLayers, QueryTriggerInteraction.Ignore);
    }

// --- Region debug view --------------------------------------------------------------------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        foreach (RegionDefinition region in regions)
        {
            DrawCircle(campsite.position, region.outerRadius);
        }
    }

    void DrawCircle(Vector3 center, float radius)
    {
        int segments = 100;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i / (float)segments * Mathf.PI * 2f;
            Vector3 nextPoint = center + new Vector3(
                Mathf.Cos(angle) * radius,
                0,
                Mathf.Sin(angle) * radius
            );

            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
