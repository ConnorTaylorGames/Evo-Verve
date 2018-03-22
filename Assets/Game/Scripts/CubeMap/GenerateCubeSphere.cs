using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;

public class GenerateCubeSphere : MonoBehaviour
{

    private float radius = 30.0f;
    public GameObject cubeObject;

    public int resolutionx = 32;
    public int resolutiony = 32;

    private Texture2D texture;

    public float sampleSizeX = 4.0f; // perlin sample size
    public float sampleSizeY = 4.0f; // perlin sample size

    public float sampleOffsetX = 4.0f; // to tile, add size to the offset. eg, next tile across would be 6.0f
    public float sampleOffsetY = 1.0f; // to tile, add size to the offset. eg, next tile up would be 5.0f

    public float baseflatFrequency = 2.0f;

    public float flatScale = 0.125f;
    public float flatBias = -0.75f;

    public float terraintypeFrequency = 1.7f;
    public float terraintypePersistence = 1.25f;

    public float terrainSelectorEdgeFalloff = 0.125f;

    public float finalterrainFrequency = 6.0f;
    public float finalterrainPower = 0.125f;

    public float south = -90.0f;
    public float north = 90.0f;

    public float west = -180.0f;
    public float east = 180.0f;


    private int seed;
    public int Seed { get { return seed; } set { seed = value; } }

    private void OnEnable()
    {
       // GameManager.Loaded += Generate;
    }

    private void OnDisable()
    {
        //GameManager.Loaded -= Generate;

    }

    void Generate()
    {
        

        // ------------------------------------------------------------------------------------------


        // - Mountain Terrain
        RidgedMultifractal mountainTerrain = new RidgedMultifractal();

        // ------------------------------------------------------------------------------------------


        // - Base Flat Terrain
        Billow baseFlatTerrain = new Billow();
        baseFlatTerrain.Frequency = baseflatFrequency;

        // ------------------------------------------------------------------------------------------


        // - Flat Terrain
        ScaleBias flatTerrain = new ScaleBias(flatScale, flatBias, baseFlatTerrain); // scale, bias, input

        // ------------------------------------------------------------------------------------------


        // - Terrain Type
        //Generate Perlin Noise
        Perlin noise = new Perlin();

        if (Seed == 0)
        {
            SetSeed();
        }

        noise.Seed = Seed;
        Vector3[] baseVertices = null;

        noise.Frequency = terraintypeFrequency;
        noise.Persistence = terraintypePersistence;

        // ------------------------------------------------------------------------------------------

        // - Terrain Selector
        Select terrainSelector = new Select(flatTerrain, mountainTerrain, noise); // input A, input B, Controller


        terrainSelector.SetBounds(0.0, 100.0);

        terrainSelector.FallOff = terrainSelectorEdgeFalloff;

        // ------------------------------------------------------------------------------------------


        // - Final Terrain -
        Turbulence finalTerrain = new Turbulence(terrainSelector);

        finalTerrain.Frequency = finalterrainFrequency;

        finalTerrain.Power = finalterrainPower;


        // ------------------------------------------------------------------------------------------


        //Store Sphere Mesh
        Mesh mesh = cubeObject.GetComponent<MeshFilter>().mesh;

        // - Compiled Terrain -
        //Create ModuleBase
        ModuleBase myModule = finalTerrain;

        Noise2D heightMap;

        heightMap = new Noise2D(resolutionx, resolutiony, myModule);
        heightMap.GenerateSpherical(south, north, west, east);
        texture = heightMap.GetTexture(GradientPresets.Terrain2);

        texture.filterMode = FilterMode.Point;
        cubeObject.GetComponent<MeshRenderer>().material.mainTexture = texture;


        // ------------------------------------------------------------------------------------------


        //Apply noise to vertices
        if (baseVertices == null)
        {
            baseVertices = mesh.vertices;
            Vector3[] vertices = new Vector3[baseVertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 vertex = baseVertices[i];
                Vector3 pos = vertex.normalized * radius;
                //vertex = vertex.normalized * (float)(radius + myModule.GetValue(pos.x, pos.y, pos.z) * 2.0f);
                vertex = vertex.normalized * (float)(radius + heightMap.Height);

                vertices[i] = vertex;
               
                Debug.Log(myModule.GetValue(pos.x, pos.y, pos.z));

            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            DestroyImmediate(cubeObject.GetComponent<Collider>());
            cubeObject.AddComponent<MeshCollider>();
            texture.Apply();

        }



        DestroyImmediate(cubeObject.GetComponent<Collider>());
        cubeObject.AddComponent<MeshCollider>();

    }

    void SetSeed()
    {
        Seed = Random.Range(1, 100000);
    }
}

