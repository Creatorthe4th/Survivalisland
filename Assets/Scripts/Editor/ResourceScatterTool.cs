using UnityEngine;
using UnityEditor;

public class ResourceScatterTool : EditorWindow
{
    private Terrain targetTerrain;
    private GameObject prefabToScatter;
    private Transform parentObject;

    private int scatterCount = 20;
    private float minScale = 0.8f;
    private float maxScale = 1.2f;
    private bool randomYRotation = true;
    private float maxSlopeAngle = 35f;
    private float minSpacing = 2f;
    private LayerMask groundLayer = ~0; // default: everything

    [MenuItem("Tools/Resource Scatter Tool")]
    public static void ShowWindow()
    {
        GetWindow<ResourceScatterTool>("Resource Scatter Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scatter Setup", EditorStyles.boldLabel);

        targetTerrain = (Terrain)EditorGUILayout.ObjectField("Target Terrain", targetTerrain, typeof(Terrain), true);
        prefabToScatter = (GameObject)EditorGUILayout.ObjectField("Prefab To Scatter", prefabToScatter, typeof(GameObject), false);
        parentObject = (Transform)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(Transform), true);

        EditorGUILayout.Space();
        GUILayout.Label("Placement Settings", EditorStyles.boldLabel);

        scatterCount = EditorGUILayout.IntSlider("Count", scatterCount, 1, 500);
        minSpacing = EditorGUILayout.FloatField("Min Spacing", minSpacing);
        maxSlopeAngle = EditorGUILayout.Slider("Max Slope Angle", maxSlopeAngle, 0f, 90f);

        EditorGUILayout.Space();
        GUILayout.Label("Randomization", EditorStyles.boldLabel);

        EditorGUILayout.MinMaxSlider("Scale Range", ref minScale, ref maxScale, 0.1f, 3f);
        EditorGUILayout.LabelField($"Min: {minScale:F2}  Max: {maxScale:F2}");
        randomYRotation = EditorGUILayout.Toggle("Random Y Rotation", randomYRotation);

        EditorGUILayout.Space();

        GUI.enabled = targetTerrain != null && prefabToScatter != null;
        if (GUILayout.Button("Scatter Resources"))
        {
            ScatterResources();
        }
        GUI.enabled = true;

        if (GUILayout.Button("Clear Children From Parent") && parentObject != null)
        {
            ClearParent();
        }
    }

    private void ScatterResources()
    {
        if (targetTerrain == null || prefabToScatter == null)
        {
            Debug.LogWarning("Assign a terrain and prefab before scattering.");
            return;
        }

        TerrainData terrainData = targetTerrain.terrainData;
        Vector3 terrainPos = targetTerrain.transform.position;

        GameObject container = parentObject != null
            ? parentObject.gameObject
            : new GameObject($"{prefabToScatter.name}_Scattered");

        if (parentObject == null)
            Undo.RegisterCreatedObjectUndo(container, "Create Scatter Container");

        Vector3[] placedPositions = new Vector3[scatterCount];
        int placedCount = 0;
        int maxAttemptsPerPoint = 30;
        int totalAttempts = 0;
        int maxTotalAttempts = scatterCount * maxAttemptsPerPoint;

        while (placedCount < scatterCount && totalAttempts < maxTotalAttempts)
        {
            totalAttempts++;

            float randX = Random.Range(0f, terrainData.size.x);
            float randZ = Random.Range(0f, terrainData.size.z);
            Vector3 worldXZ = terrainPos + new Vector3(randX, 0f, randZ);

            float terrainHeight = targetTerrain.SampleHeight(worldXZ);
            Vector3 spawnPos = new Vector3(worldXZ.x, terrainHeight + terrainPos.y, worldXZ.z);

            // Slope check via terrain normal
            float normX = randX / terrainData.size.x;
            float normZ = randZ / terrainData.size.z;
            Vector3 normal = terrainData.GetInterpolatedNormal(normX, normZ);
            float slopeAngle = Vector3.Angle(normal, Vector3.up);

            if (slopeAngle > maxSlopeAngle)
                continue;

            // Spacing check against already-placed points
            bool tooClose = false;
            for (int i = 0; i < placedCount; i++)
            {
                if (Vector3.Distance(placedPositions[i], spawnPos) < minSpacing)
                {
                    tooClose = true;
                    break;
                }
            }
            if (tooClose)
                continue;

            // Passed all checks — place it
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabToScatter, container.scene);
            instance.transform.position = spawnPos;
            instance.transform.SetParent(container.transform, true);

            float randomScale = Random.Range(minScale, maxScale);
            instance.transform.localScale = Vector3.one * randomScale;

            if (randomYRotation)
                instance.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            else
                instance.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);

            Undo.RegisterCreatedObjectUndo(instance, "Scatter Resource");

            placedPositions[placedCount] = spawnPos;
            placedCount++;
        }

        if (placedCount < scatterCount)
        {
            Debug.LogWarning($"Only placed {placedCount}/{scatterCount} resources. " +
                              $"Try reducing spacing, count, or slope restrictions.");
        }
        else
        {
            Debug.Log($"Successfully scattered {placedCount} resources.");
        }
    }

    private void ClearParent()
    {
        if (parentObject == null) return;

        for (int i = parentObject.childCount - 1; i >= 0; i--)
        {
            Undo.DestroyObjectImmediate(parentObject.GetChild(i).gameObject);
        }
    }
}