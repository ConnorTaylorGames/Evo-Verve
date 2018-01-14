using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMoveScript : MonoBehaviour
{
    public GameObject planet;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (planet)
        {
            transform.RotateAround(planet.transform.position, Vector3.up, 15 * Time.deltaTime);
            transform.LookAt(planet.transform);
        }
        else
        {
            Debug.Log("No planet to rotate around");
        }
    }
}
