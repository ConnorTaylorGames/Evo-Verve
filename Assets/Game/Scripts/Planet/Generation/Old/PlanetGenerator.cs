using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetGenerator : MonoBehaviour {

    Planet planet;
    // Use this for initialization
    void Start () {
        planet = new Planet();
        planet.InitAsIcosohedron();
        planet.Subdivide(2);
        Debug.Log("Generation complete");
    }


}
