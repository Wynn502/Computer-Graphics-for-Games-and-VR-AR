using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearWater : MonoBehaviour


{
    public float waveHeight = 0.5f;
    public float waveSpeed = 1.0f;
    public float noiseScale = 0.1f;
    public int numPoints = 100;
    //public float pointRadius = 1.0f;

    private MeshFilter meshFilter;
    private Vector3[] baseVertices;
    private Vector3[] displacedVertices;
    private Vector2[] uvs;
    private Vector2[] noiseOffsets;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        baseVertices = meshFilter.mesh.vertices;
        displacedVertices = new Vector3[baseVertices.Length];
        uvs = meshFilter.mesh.uv;
        noiseOffsets = new Vector2[numPoints];

        // Generate sample points randomly
        for (int i = 0; i < numPoints; i++)
        {
            noiseOffsets[i] = new Vector2(Random.value, Random.value);
        }
    }

    void Update()
    {
        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector2 uv = uvs[i];

            // Calculate the distance between each vertex and the nearest sample point,
            // and obtain the characteristic value of the sample point corresponding to the minimum distance
            float minDistance = float.MaxValue;
            float featureValue = 0;
            for (int j = 0; j < numPoints; j++)
            {
                Vector2 offset1 = noiseOffsets[j] + uv * noiseScale;
                Vector2 point = new Vector2(Mathf.Floor(offset1.x), Mathf.Floor(offset1.y));
                Vector2 fraction = offset1 - point;
                float distance = (fraction - new Vector2(0.5f, 0.5f)).magnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    featureValue = j;
                }
            }

            // Calculate offset from characteristic value
            float offset = Mathf.Lerp(-waveHeight, waveHeight, featureValue / numPoints);

            displacedVertices[i] = baseVertices[i] + new Vector3(0, offset * Mathf.Sin(Time.time * waveSpeed + baseVertices[i].x), 0);
        }

        meshFilter.mesh.vertices = displacedVertices;
        meshFilter.mesh.RecalculateNormals();
    }
}

