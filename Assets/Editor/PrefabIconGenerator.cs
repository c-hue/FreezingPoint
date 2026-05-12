using UnityEngine;
using UnityEditor;
using System.IO;

public class PrefabIconGenerator : EditorWindow
{
    private GameObject prefab;

    private int iconSize = 256;
    private string saveFolder = "Assets/GeneratedIcons";

    private Vector3 iconRotation = new Vector3(25f, 90f, 0f);
    private Vector3 iconOffset = Vector3.zero;
    private float zoom = 0.7f;

    private float lightIntensity = 1.5f;
    private Vector3 lightRotation = new Vector3(50f, -30f, 0f);

    [MenuItem("Tools/Icon Generator")]
    public static void OpenWindow()
    {
        GetWindow<PrefabIconGenerator>("Icon Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Icon Generator", EditorStyles.boldLabel);

        prefab = (GameObject)EditorGUILayout.ObjectField(
            "Prefab",
            prefab,
            typeof(GameObject),
            false
        );

        iconSize = EditorGUILayout.IntField("Icon Size", iconSize);
        saveFolder = EditorGUILayout.TextField("Save Folder", saveFolder);

        GUILayout.Space(10);
        GUILayout.Label("Icon Settings", EditorStyles.boldLabel);

        iconRotation = EditorGUILayout.Vector3Field("Icon Rotation", iconRotation);
        iconOffset = EditorGUILayout.Vector3Field("Icon Offset", iconOffset);
        zoom = EditorGUILayout.Slider("Zoom", zoom, 0.3f, 2f);

        GUILayout.Space(10);
        GUILayout.Label("Lighting", EditorStyles.boldLabel);

        lightIntensity = EditorGUILayout.Slider("Light Intensity", lightIntensity, 0f, 5f);
        lightRotation = EditorGUILayout.Vector3Field("Light Rotation", lightRotation);

        GUILayout.Space(10);

        if (GUILayout.Button("Generate Icon"))
        {
            if (prefab == null)
            {
                Debug.LogError("No prefab selected.");
                return;
            }

            GenerateIcon(prefab);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Log Preset"))
        {
            iconRotation = new Vector3(0f, 135f, 0f);
            iconOffset = Vector3.zero;
            zoom = 0.7f;
        }

        if (GUILayout.Button("Blueberry Preset"))
        {
            iconRotation = new Vector3(0f, 90f, -25f);
            iconOffset = Vector3.zero;
            zoom = 0.7f;
        }

        if (GUILayout.Button("Axe Preset"))
        {
            iconRotation = new Vector3(135f, 90f, 90f);
            iconOffset = Vector3.zero;
            zoom = 0.7f;
        }
    }

    private void GenerateIcon(GameObject prefabToRender)
    {
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        GameObject instance = Instantiate(prefabToRender);
        instance.transform.position = iconOffset;
        instance.transform.rotation = Quaternion.Euler(iconRotation);

        Bounds bounds = GetBounds(instance);

        GameObject camObj = new GameObject("Icon Camera");
        Camera cam = camObj.AddComponent<Camera>();

        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0);
        cam.orthographic = true;

        float largestSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        cam.orthographicSize = largestSize * zoom;

        cam.transform.position = bounds.center + new Vector3(0f, 0f, -10f);
        cam.transform.LookAt(bounds.center);

        GameObject lightObj = new GameObject("Icon Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = lightIntensity;
        lightObj.transform.rotation = Quaternion.Euler(lightRotation);

        RenderTexture rt = new RenderTexture(iconSize, iconSize, 24, RenderTextureFormat.ARGB32);
        cam.targetTexture = rt;

        Texture2D texture = new Texture2D(iconSize, iconSize, TextureFormat.RGBA32, false);

        RenderTexture.active = rt;
        cam.Render();

        texture.ReadPixels(new Rect(0, 0, iconSize, iconSize), 0, 0);
        texture.Apply();

        string path = $"{saveFolder}/{prefabToRender.name}_Icon.png";
        File.WriteAllBytes(path, texture.EncodeToPNG());

        RenderTexture.active = null;
        cam.targetTexture = null;

        DestroyImmediate(instance);
        DestroyImmediate(camObj);
        DestroyImmediate(lightObj);
        DestroyImmediate(rt);
        DestroyImmediate(texture);

        AssetDatabase.Refresh();

        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.alphaIsTransparency = true;
            importer.SaveAndReimport();
        }

        Debug.Log("Generated icon: " + path);
    }

    private Bounds GetBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            return new Bounds(obj.transform.position, Vector3.one);
        }

        Bounds bounds = renderers[0].bounds;

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }
}