using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;

public class GenerateCubeSphere : MonoBehaviour {

    private float radius = 20.0f;
    public GameObject cubeObject;

    private int seed;
    public int Seed { get { return seed; } set { seed = value; } }

    private bool hasDataFile;

    private void OnEnable()
    {
        GameManager.Loaded += Generate;
    }

    private void OnDisable()
    {
        GameManager.Loaded -= Generate;

    }

    void Generate()
    {

        Perlin noise = new Perlin();
        if (Seed == 0)
        {
            SetSeed();
        }
        noise.Seed = Seed;
        noise.OctaveCount = 8;
        noise.Frequency = 2;
        noise.Lacunarity = 6;
        noise.Persistence = 0.5f;
        Vector3[] baseVertices = null;
        Mesh mesh = cubeObject.GetComponent<MeshFilter>().mesh;

        if (baseVertices == null)
        {
            baseVertices = mesh.vertices;
            Vector3[] vertices = new Vector3[baseVertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 vertex = baseVertices[i];
                Vector3 pos = vertex.normalized * radius;
                vertex = vertex.normalized * (float)(radius + noise.GetValue(pos.x, pos.y, pos.z) * 1.5f);
                vertices[i] = vertex;

                if (noise.GetValue(pos.x, pos.y, pos.z) > 0.5f)
                {
                    //Debug.Log("Point " + vertex + " is above 0.5");
                }

            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            DestroyImmediate(cubeObject.GetComponent<Collider>());
            cubeObject.AddComponent<MeshCollider>();

        }

        DestroyImmediate(cubeObject.GetComponent<Collider>());
        cubeObject.AddComponent<MeshCollider>();
    }

    void SetSeed()
    {
        Seed = Random.Range(1, 100000);
    }


}


