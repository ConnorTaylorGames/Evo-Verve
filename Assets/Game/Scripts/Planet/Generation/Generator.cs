using UnityEngine;
using AccidentalNoise;
using System.Collections.Generic;

public class Generator : MonoBehaviour
{

    // Adjustable variables for Unity Inspector
    [SerializeField]
    int Seed;

    // Adjustable variables for Unity Inspector
    [Header("Generator Values")]
    [SerializeField]
    int Width = 512;
    [SerializeField]
    int Height = 512;

    [Header("Height Map")]
    [SerializeField]
    int TerrainOctaves = 6;
    [SerializeField]
    double TerrainFrequency = 1.25;
    [SerializeField]
    float DeepWater = 0.2f;
    [SerializeField]
    float ShallowWater = 0.4f;
    [SerializeField]
    float Sand = 0.5f;
    [SerializeField]
    float Grass = 0.7f;
    [SerializeField]
    float Forest = 0.8f;
    [SerializeField]
    float Rock = 0.9f;

    [Header("Heat Map")]
    [SerializeField]
    int HeatOctaves = 4;
    [SerializeField]
    double HeatFrequency = 3.0;
    [SerializeField]
    float ColdestValue = 0.05f;
    [SerializeField]
    float ColderValue = 0.18f;
    [SerializeField]
    float ColdValue = 0.4f;
    [SerializeField]
    float WarmValue = 0.6f;
    [SerializeField]
    float WarmerValue = 0.8f;

    [Header("Moisture Map")]
    [SerializeField]
    int MoistureOctaves = 4;
    [SerializeField]
    double MoistureFrequency = 3.0;
    [SerializeField]
    float DryerValue = 0.27f;
    [SerializeField]
    float DryValue = 0.4f;
    [SerializeField]
    float WetValue = 0.6f;
    [SerializeField]
    float WetterValue = 0.8f;
    [SerializeField]
    float WettestValue = 0.9f;

    // private variables
    ImplicitFractal HeightMap;
    ImplicitCombiner HeatMap;
    ImplicitFractal MoistureMap;

    MapData HeightData;
    MapData HeatData;
    MapData MoistureData;

    Tile[,] Tiles;

    List<TileGroup> Waters = new List<TileGroup>();
    List<TileGroup> Lands = new List<TileGroup>();

    // Our texture output gameobject
    protected MeshRenderer HeightMapRenderer;
    protected MeshRenderer HeatMapRenderer;
    protected MeshRenderer MoistureMapRenderer;
    protected MeshRenderer BiomeMapRenderer;

    protected BiomeType[,] BiomeTable = new BiomeType[6, 6] {   
		//COLDEST        //COLDER          //COLD                  //HOT                          //HOTTER                       //HOTTEST
		{ BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYEST
		{ BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYER
		{ BiomeType.Ice, BiomeType.Tundra, BiomeType.Woodland,     BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //DRY
		{ BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //WET
		{ BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.SeasonalForest,      BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest },  //WETTER
		{ BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.TemperateRainforest, BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest }   //WETTEST
	};


    void Start()
    {
        Seed = UnityEngine.Random.Range(0, int.MaxValue);

        HeightMapRenderer = transform.Find("HeightTexture").GetComponent<MeshRenderer>();
        HeatMapRenderer = transform.Find ("HeatTexture").GetComponent<MeshRenderer> ();
        MoistureMapRenderer = transform.Find("MoistureTexture").GetComponent<MeshRenderer>();
        BiomeMapRenderer = transform.Find("BiomeTexture").GetComponent<MeshRenderer>();


        Initialize();
        GetData();
        LoadTiles();

        UpdateNeighbors();
        //Uncomment if you want bitmasks shown
        //UpdateBitmasks();
        FloodFill();

        GenerateBiomeMap();
        //Uncomment if you want bitmasks shown
        //UpdateBiomeBitmask();


        HeightMapRenderer.materials[0].mainTexture = TextureGenerator.GetHeightMapTexture(Width, Height, Tiles);
        HeatMapRenderer.materials[0].mainTexture = TextureGenerator.GetHeatMapTexture(Width, Height, Tiles);
        MoistureMapRenderer.materials[0].mainTexture = TextureGenerator.GetMoistureMapTexture(Width, Height, Tiles);
        BiomeMapRenderer.materials[0].mainTexture = TextureGenerator.GetBiomeMapTexture(Width, Height, Tiles, ColdestValue, ColderValue, ColdValue);

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Initialize();
            GetData();
            LoadTiles();
            UpdateNeighbors();
            FloodFill();

            HeightMapRenderer.materials[0].mainTexture = TextureGenerator.GetHeightMapTexture(Width, Height, Tiles);
            HeatMapRenderer.materials[0].mainTexture = TextureGenerator.GetHeatMapTexture(Width, Height, Tiles);
            MoistureMapRenderer.materials[0].mainTexture = TextureGenerator.GetMoistureMapTexture(Width, Height, Tiles);
            BiomeMapRenderer.materials[0].mainTexture = TextureGenerator.GetBiomeMapTexture(Width, Height, Tiles, ColdestValue, ColderValue, ColdValue);

        }
    }

    public BiomeType GetBiomeType(Tile tile)
    {
        return BiomeTable[(int)tile.MoistureType, (int)tile.HeatType];
    }

    private void GenerateBiomeMap()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {

                if (!Tiles[x, y].Collidable) continue;

                Tile t = Tiles[x, y];
                t.BiomeType = GetBiomeType(t);
            }
        }
    }

    private void UpdateBiomeBitmask()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tiles[x, y].UpdateBiomeBitmask();
            }
        }
    }

    private void Initialize()
    {
        // Initialize the HeightMap Generator
        HeightMap = new ImplicitFractal(FractalType.MULTI,
                                       BasisType.SIMPLEX,
                                       InterpolationType.QUINTIC,
                                       TerrainOctaves,
                                       TerrainFrequency,
                                       UnityEngine.Random.Range(0, int.MaxValue));

        // Initialize the Heat map
        ImplicitGradient gradient = new ImplicitGradient(1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1);
        ImplicitFractal heatFractal = new ImplicitFractal(FractalType.MULTI,
                                                          BasisType.SIMPLEX,
                                                          InterpolationType.QUINTIC,
                                                          HeatOctaves,
                                                          HeatFrequency,
                                                          Seed);

        HeatMap = new ImplicitCombiner(CombinerType.MULTIPLY);
        HeatMap.AddSource(gradient);
        HeatMap.AddSource(heatFractal);

        //moisture map
        MoistureMap = new ImplicitFractal(FractalType.MULTI,
                                           BasisType.SIMPLEX,
                                           InterpolationType.QUINTIC,
                                           MoistureOctaves,
                                           MoistureFrequency,
                                           Seed);
    }


    // Extract data from a noise module
    private void GetData()
    {
        HeightData = new MapData(Width, Height);
        HeatData = new MapData(Width, Height);
        MoistureData = new MapData(Width, Height);

        // loop through each x,y point - get height value
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                // WRAP ON BOTH AXIS
                // Noise range
                float x1 = 0, x2 = 2;
                float y1 = 0, y2 = 2;
                float dx = x2 - x1;
                float dy = y2 - y1;

                // Sample noise at smaller intervals
                float s = x / (float)Width;
                float t = y / (float)Height;


                // Calculate our 4D coordinates
                float nx = x1 + Mathf.Cos(s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
                float ny = y1 + Mathf.Cos(t * 2 * Mathf.PI) * dy / (2 * Mathf.PI);
                float nz = x1 + Mathf.Sin(s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
                float nw = y1 + Mathf.Sin(t * 2 * Mathf.PI) * dy / (2 * Mathf.PI);

                float heightValue = (float)HeightMap.Get(nx, ny, nz, nw);
                float heatValue = (float)HeatMap.Get(nx, ny, nz, nw);
                float moistureValue = (float)MoistureMap.Get(nx, ny, nz, nw);


                // keep track of the max and min values found
                if (heightValue > HeightData.Max) HeightData.Max = heightValue;
                if (heightValue < HeightData.Min) HeightData.Min = heightValue;


                if (heatValue > HeatData.Max) HeatData.Max = heatValue;
                if (heatValue < HeatData.Min) HeatData.Min = heatValue;

                if (moistureValue > MoistureData.Max) MoistureData.Max = moistureValue;
                if (moistureValue < MoistureData.Min) MoistureData.Min = moistureValue;

                HeightData.Data[x, y] = heightValue;
                HeatData.Data[x, y] = heatValue;
                MoistureData.Data[x, y] = moistureValue;

            }
        }
    }


    // Build a Tile array from our data
    private void LoadTiles()
    {
        Tiles = new Tile[Width, Height];

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tile t = new Tile();
                t.X = x;
                t.Y = y;

                //set heightmap value
                float heightValue = HeightData.Data[x, y];
                heightValue = (heightValue - HeightData.Min) / (HeightData.Max - HeightData.Min);
                t.HeightValue = heightValue;


                if (heightValue < DeepWater)
                {
                    t.HeightType = HeightType.DeepWater;
                    t.Collidable = false;
                }
                else if (heightValue < ShallowWater)
                {
                    t.HeightType = HeightType.ShallowWater;
                    t.Collidable = false;
                }
                else if (heightValue < Sand)
                {
                    t.HeightType = HeightType.Sand;
                    t.Collidable = true;
                }
                else if (heightValue < Grass)
                {
                    t.HeightType = HeightType.Grass;
                    t.Collidable = true;
                }
                else if (heightValue < Forest)
                {
                    t.HeightType = HeightType.Forest;
                    t.Collidable = true;
                }
                else if (heightValue < Rock)
                {
                    t.HeightType = HeightType.Rock;
                    t.Collidable = true;
                }
                else
                {
                    t.HeightType = HeightType.Snow;
                    t.Collidable = true;
                }


                //adjust moisture based on height
                if (t.HeightType == HeightType.DeepWater)
                {
                    MoistureData.Data[t.X, t.Y] += 8f * t.HeightValue;
                }
                else if (t.HeightType == HeightType.ShallowWater)
                {
                    MoistureData.Data[t.X, t.Y] += 3f * t.HeightValue;
                }
                else if (t.HeightType == HeightType.Shore)
                {
                    MoistureData.Data[t.X, t.Y] += 1f * t.HeightValue;
                }
                else if (t.HeightType == HeightType.Sand)
                {
                    MoistureData.Data[t.X, t.Y] += 0.2f * t.HeightValue;
                }

                //Moisture Map Analyze	
                float moistureValue = MoistureData.Data[x, y];
                moistureValue = (moistureValue - MoistureData.Min) / (MoistureData.Max - MoistureData.Min);
                t.MoistureValue = moistureValue;

                //set moisture type
                if (moistureValue < DryerValue) t.MoistureType = MoistureType.Dryest;
                else if (moistureValue < DryValue) t.MoistureType = MoistureType.Dryer;
                else if (moistureValue < WetValue) t.MoistureType = MoistureType.Dry;
                else if (moistureValue < WetterValue) t.MoistureType = MoistureType.Wet;
                else if (moistureValue < WettestValue) t.MoistureType = MoistureType.Wetter;
                else t.MoistureType = MoistureType.Wettest;


                // Adjust Heat Map based on Height - Higher == colder
                if (t.HeightType == HeightType.Forest)
                {
                    HeatData.Data[t.X, t.Y] -= 0.1f * t.HeightValue;
                }
                else if (t.HeightType == HeightType.Rock)
                {
                    HeatData.Data[t.X, t.Y] -= 0.25f * t.HeightValue;
                }
                else if (t.HeightType == HeightType.Snow)
                {
                    HeatData.Data[t.X, t.Y] -= 0.4f * t.HeightValue;
                }
                else
                {
                    HeatData.Data[t.X, t.Y] += 0.01f * t.HeightValue;
                }

                // Set heat value
                float heatValue = HeatData.Data[x, y];
                heatValue = (heatValue - HeatData.Min) / (HeatData.Max - HeatData.Min);
                t.HeatValue = heatValue;

                // set heat type
                if (heatValue < ColdestValue) t.HeatType = HeatType.Coldest;
                else if (heatValue < ColderValue) t.HeatType = HeatType.Colder;
                else if (heatValue < ColdValue) t.HeatType = HeatType.Cold;
                else if (heatValue < WarmValue) t.HeatType = HeatType.Warm;
                else if (heatValue < WarmerValue) t.HeatType = HeatType.Warmer;
                else t.HeatType = HeatType.Warmest;

                Tiles[x, y] = t;
            }
        }
    }


    private void UpdateNeighbors()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tile t = Tiles[x, y];

                t.Top = GetTop(t);
                t.Bottom = GetBottom(t);
                t.Left = GetLeft(t);
                t.Right = GetRight(t);
            }
        }
    }

    //Update bitmasks
    private void UpdateBitmasks()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tiles[x, y].UpdateBitmask();
            }
        }
    }


    private void FloodFill()
    {
        // Use a stack instead of recursion
        Stack<Tile> stack = new Stack<Tile>();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {

                Tile t = Tiles[x, y];

                //Tile already flood filled, skip
                if (t.FloodFilled) continue;

                // Land
                if (t.Collidable)
                {
                    TileGroup group = new TileGroup();
                    group.Type = TileGroupType.Land;
                    stack.Push(t);

                    while (stack.Count > 0)
                    {
                        FloodFill(stack.Pop(), ref group, ref stack);
                    }

                    if (group.Tiles.Count > 0)
                        Lands.Add(group);
                }
                // Water
                else
                {
                    TileGroup group = new TileGroup();
                    group.Type = TileGroupType.Water;
                    stack.Push(t);

                    while (stack.Count > 0)
                    {
                        FloodFill(stack.Pop(), ref group, ref stack);
                    }

                    if (group.Tiles.Count > 0)
                        Waters.Add(group);
                }
            }
        }
    }


    private void FloodFill(Tile tile, ref TileGroup tiles, ref Stack<Tile> stack)
    {
        // Validate
        if (tile.FloodFilled)
            return;
        if (tiles.Type == TileGroupType.Land && !tile.Collidable)
            return;
        if (tiles.Type == TileGroupType.Water && tile.Collidable)
            return;

        // Add to TileGroup
        tiles.Tiles.Add(tile);
        tile.FloodFilled = true;

        // floodfill into neighbors
        Tile t = GetTop(tile);
        if (!t.FloodFilled && tile.Collidable == t.Collidable)
            stack.Push(t);
        t = GetBottom(tile);
        if (!t.FloodFilled && tile.Collidable == t.Collidable)
            stack.Push(t);
        t = GetLeft(tile);
        if (!t.FloodFilled && tile.Collidable == t.Collidable)
            stack.Push(t);
        t = GetRight(tile);
        if (!t.FloodFilled && tile.Collidable == t.Collidable)
            stack.Push(t);
    }


    //Get tiles around current tile
    private Tile GetTop(Tile t)
    {
        return Tiles[t.X, MathHelper.Mod(t.Y - 1, Height)];
    }
    private Tile GetBottom(Tile t)
    {
        return Tiles[t.X, MathHelper.Mod(t.Y + 1, Height)];
    }
    private Tile GetLeft(Tile t)
    {
        return Tiles[MathHelper.Mod(t.X - 1, Width), t.Y];
    }
    private Tile GetRight(Tile t)
    {
        return Tiles[MathHelper.Mod(t.X + 1, Width), t.Y];
    }
}