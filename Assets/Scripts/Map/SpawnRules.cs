using System;
using System.Collections.Generic;
using UnityEngine;

// Defines each region by name, radius, and holds a list of rules for each resource
[Serializable]
public class RegionDefinition
{
    [SerializeField] public string regionName;

    [Header("Distance from Campsite")]
    [SerializeField] public float innerRadius;
    [SerializeField] public float outerRadius;

    [Header("Spawn Rules")]
    [SerializeField] public List<ResourceSpawnRule> resourceRules = new();
}

// Rules for each resource including max count/respawn rate
[Serializable]
public class ResourceSpawnRule
{
    [SerializeField] public string resourceID;
    [SerializeField] public List<GameObject> prefabs;

    [Header("Spawn Settings")]
    [SerializeField] public int maxCount = 10;
    [SerializeField] public float respawnTime = 120f;

    [Header("Placement")]
    [SerializeField] public float radius = 1f;
    [SerializeField] public bool randomRotation = true;
}