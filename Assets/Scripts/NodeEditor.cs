using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [InitializeOnLoad]
    public class NodeEditor
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawGizmos(Waypoint waypoint, GizmoType gizmoType)
        {
            if ((gizmoType & gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = Color.red;
            }
        
            Gizmos.DrawSphere(waypoint.transform.position, .1f);
            
            
            if (waypoint.previousWaypoint != null && !waypoint.isLink)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(waypoint.transform.position, waypoint.previousWaypoint.transform.position);
            }
            
            if (waypoint.previousWaypoint != null && waypoint.isLink)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(waypoint.transform.position, waypoint.previousWaypoint.transform.position);
            }
            
        }
    }
}