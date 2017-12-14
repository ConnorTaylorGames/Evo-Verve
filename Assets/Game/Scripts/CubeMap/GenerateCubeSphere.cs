using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCubeSphere : MonoBehaviour {

    private float radius = 1.0f;
    public GameObject cubeObject;

	// Use this for initialization
	void Start ()
    {
        Generate();
	}

    void Generate()
    {

        float perlin = Mathf.PerlinNoise(128f, 128f);
        Vector3[] baseVertices = null;
        Mesh mesh = cubeObject.GetComponent<MeshFilter>().mesh;

        if (baseVertices == null)
        {
            baseVertices = mesh.vertices;
            Vector3[] vertices = new Vector3[baseVertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 vertex = baseVertices[i];
                vertex = vertex.normalized * (radius + Mathf.PerlinNoise(vertex.x * 1.8f, vertex.y * 1.8f) * 1.1f);
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


