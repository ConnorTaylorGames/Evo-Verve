using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Polygon m_InnerPoly; //The Poly we are looking to manipulate.
    public Polygon m_OuterPoly; //The Poly we don't want to manipulate.

    public List<int> m_SharedVerts; //The vertices along this edge.
    public Edge(Polygon inner_poly, Polygon outer_poly)
    {
        m_InnerPoly = inner_poly;
        m_OuterPoly = outer_poly;
        m_SharedVerts = new List<int>();
        foreach (int vertex in inner_poly.m_Vertices)
        {
            if (outer_poly.m_Vertices.Contains(vertex))
                m_SharedVerts.Add(vertex);
        }
    }
}