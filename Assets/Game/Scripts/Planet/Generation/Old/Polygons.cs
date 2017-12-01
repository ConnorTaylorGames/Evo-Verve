using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon
{
    public List<int> m_Vertices;
    public List<Polygon> m_Neighbors;

    public Polygon(int a, int b, int c)
    {
        m_Vertices = new List<int>() { a, b, c };
        m_Neighbors = new List<Polygon>();
    }
    public bool IsNeighborOf(Polygon other_poly)
    {
        int shared_vertices = 0;
        foreach (int vertex in m_Vertices)
        {
            if (other_poly.m_Vertices.Contains(vertex))
                shared_vertices++;
        }
        // A polygon and its neighbor will share exactly
        // two vertices. Ergo, if this poly shares two
        // vertices with the other, then they are neighbors.
        return shared_vertices == 2;
    }
}