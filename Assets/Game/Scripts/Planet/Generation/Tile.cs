using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HeightType
{
    DeepWater = 1,
    ShallowWater = 2,
    Shore = 3,
    Sand = 4,
    Grass = 5,
    Forest = 6,
    Rock = 7,
    Snow = 8,
    River = 9
}

public enum HeatType
{
    Coldest = 0,
    Colder = 1,
    Cold = 2,
    Warm = 3,
    Warmer = 4,
    Warmest = 5
}

public enum MoistureType
{
    Wettest = 5,
    Wetter = 4,
    Wet = 3,
    Dry = 2,
    Dryer = 1,
    Dryest = 0
}

/*
public enum BiomeType
{
    Desert,
    Savanna,
    TropicalRainforest,
    Grassland,
    Woodland,
    SeasonalForest,
    TemperateRainforest,
    BorealForest,
    Tundra,
    Ice
}
*/

public class Tile
{

    public HeightType HeightType;
    public HeatType HeatType;
    public MoistureType MoistureType;
    public BiomeType BiomeType;

    public float HeightValue { get; set; }
    public float HeatValue { get; set; }
    public float MoistureValue { get; set; }
    public float BiomeValue { get; set; }


    public int X, Y;
    public int Bitmask;
    public int BiomeBitmask;

    public Tile Left;
    public Tile Right;
    public Tile Top;
    public Tile Bottom;

    public bool Collidable;
    public bool FloodFilled;

    public Tile()
    {
    }

    public void UpdateBiomeBitmask()
    {
        int count = 0;

        if (Collidable && Top != null && Top.BiomeType == BiomeType)
            count += 1;
        if (Collidable && Bottom != null && Bottom.BiomeType == BiomeType)
            count += 4;
        if (Collidable && Left != null && Left.BiomeType == BiomeType)
            count += 8;
        if (Collidable && Right != null && Right.BiomeType == BiomeType)
            count += 2;

        BiomeBitmask = count;
    }

    public void UpdateBitmask()
    {
        int count = 0;

        if (Collidable && Top.HeightType == HeightType)
            count += 1;
        if (Collidable && Right.HeightType == HeightType)
            count += 2;
        if (Collidable && Bottom.HeightType == HeightType)
            count += 4;
        if (Collidable && Left.HeightType == HeightType)
            count += 8;

        Bitmask = count;
    }

}
