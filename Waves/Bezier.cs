using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour

{
    public float waveSpeed = 1.0f;
    public float waveHeight = 0.5f;
    public float noiseScale = 0.1f;

    public Transform startPoint;
    public Transform endPoint;

    private MeshFilter meshFilter;
    private Vector3[] baseVertices;
    private Vector3[] displacedVertices;
    private Vector3[] bezierPoints;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        baseVertices = meshFilter.mesh.vertices;
        displacedVertices = new Vector3[baseVertices.Length];
        bezierPoints = new Vector3[4];
        bezierPoints[0] = startPoint.position;
        bezierPoints[1] = startPoint.position + Vector3.up * 5.0f;
        bezierPoints[2] = endPoint.position + Vector3.up * 5.0f;
        bezierPoints[3] = endPoint.position;
    }

    void Update()
    {
        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];
            float t = Mathf.Clamp01((vertex.x - startPoint.position.x) / (endPoint.position.x - startPoint.position.x));
            Vector3 pointOnCurve = GetBezierPoint(t, bezierPoints);
            float offset = Mathf.PerlinNoise(pointOnCurve.x * noiseScale, pointOnCurve.z * noiseScale) * waveHeight * Mathf.Sin(Time.time * waveSpeed + pointOnCurve.x);
            displacedVertices[i] = vertex + new Vector3(0, offset, 0);
        }

        meshFilter.mesh.vertices = displacedVertices;
        meshFilter.mesh.RecalculateNormals();
    }

    private Vector3 GetBezierPoint(float t, Vector3[] points)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1.0f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * points[0] +
            3.0f * oneMinusT * oneMinusT * t * points[1] +
            3.0f * oneMinusT * t * t * points[2] +
            t * t * t * points[3];
    }
}

