using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[InitializeOnLoad]
public class WaypointEditor
{
    public static WaypointEditor Instance;
    
    WaypointEditor()
    {
        Instance = this;
    }
    
    public static List<Waypoint> SelectedRoute;
    
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)] 
    public static void OnDrawGizmos(Waypoint waypoint, GizmoType gizmoType)
    {
        if ((gizmoType & gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.red;
        }
        
        Gizmos.DrawSphere(waypoint.transform.position, .1f);
        
        
        if (waypoint.previousWaypoint != null && waypoint.isLink)
        {
            Gizmos.color = Color.blue;
            foreach (var link in waypoint.linkedWaypoints)
            {
                Gizmos.DrawLine(waypoint.transform.position, link.transform.position);
            }
        }
        else if (waypoint.previousWaypoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(waypoint.transform.position, waypoint.previousWaypoint.transform.position);
        }
        
        if (waypoint.nextWaypoint && waypoint.isLink && !waypoint.nextWaypoint.isLink)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(waypoint.transform.position, waypoint.nextWaypoint.transform.position);
        }


        if (SelectedRoute != null)
        {
            for (int i = 0; i < SelectedRoute.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(SelectedRoute[i].transform.position, .3f);
            }
        }
    }
}
