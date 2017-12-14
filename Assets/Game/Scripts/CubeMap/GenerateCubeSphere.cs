using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;

public class GenerateCubeSphere : MonoBehaviour {

    private float radius = 20.0f;
    public GameObject cubeObject;

	// Use this for initialization
	void Start ()
    {
        Generate();
	}

    void Generate()
    {

        Perlin noise = new Perlin();
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
                vertex = vertex.normalized * (float)(radius + noise.GetValue(pos.x, pos.y, pos.z) * 2f);
                vertices[i] = vertex;

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


}


