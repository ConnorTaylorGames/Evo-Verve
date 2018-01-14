using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMove : MonoBehaviour {

    public GameObject planet;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        Vector3 PlanetPoint = new Vector3(planet.transform.position.x, planet.transform.position.y + 5, planet.transform.position.z);
        if (planet)
        {
            transform.RotateAround(planet.transform.position, Vector3.up, 1 * Time.deltaTime);
            transform.LookAt(PlanetPoint);
        }
        else
        {
            Debug.Log("No planet to rotate around");
        }
    }
}
