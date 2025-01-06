using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Waypoint : MonoBehaviour
{
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;
    
    public float gScore;
    public float hScore;
    public float fScore => gScore + hScore;
    
    public List<Waypoint> linkedWaypoints = new List<Waypoint>();
    public bool isLink;
    
    public void Start()
        => gameObject.tag = Tags.Waypoint;
    

    public Vector3 GetPosition()
        => transform.position;
}

