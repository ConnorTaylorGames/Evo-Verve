using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorHandler : MonoBehaviour
{
    private Vector3 targetLocation;
    private Vector3 targetNormal;
    private Vector3 spawnLocation;

    public GameObject meteor;
    public GameObject planet;

    public MeteorSelector meteorSelector;

    public void AssignMeteorTarget(Vector3 location,List<GameObject> itemsInCollider)
    {
        Vector3 dir = location - planet.transform.position;
        dir.Normalize();
        spawnLocation = dir * 150;
        RaycastHit hit;

        if (Physics.Raycast(spawnLocation, dir * -150, out hit))
        {
            targetNormal = dir;
            targetLocation = hit.point;
            SpawnMeteor(itemsInCollider);
        }

    }

    public void SpawnMeteor(List<GameObject> itemsInCollider)
    {
        GameObject go = Instantiate(meteor, spawnLocation, Quaternion.identity);
        go.transform.rotation = Quaternion.LookRotation(go.transform.up, targetNormal);
        go.GetComponent<Meteor>().Init(targetNormal * -1, itemsInCollider, meteorSelector);    
    }

}
