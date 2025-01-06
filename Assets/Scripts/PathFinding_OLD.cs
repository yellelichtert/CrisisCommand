using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class PathFinding_OLD
    {
       public static List<Waypoint> CalculateRoute(Waypoint start, Waypoint end)
       {
           Dictionary<Waypoint, Waypoint> cameFrom = new Dictionary<Waypoint, Waypoint>();
           
           List<Waypoint> openSet = new List<Waypoint>();
           List<Waypoint> closedSet = new List<Waypoint>();
           

           var allWaypoints = GameObject.FindObjectsOfType<Waypoint>();
           for (int i = 0; i < allWaypoints.Length; i++)
           {
               allWaypoints[i].gScore = float.MaxValue;
           }
           
           
           start.gScore = 0;
           start.hScore = Vector3.Distance(start.transform.position, end.transform.position);
           openSet.Add(start);
           
           
           
           while (openSet.Count > 0)
           {
               int lowestF = default;
               for (int i = 0; i < openSet.Count; i++)
               {
                   if (openSet[i].fScore < openSet[lowestF].fScore)
                   {
                       lowestF = i;
                   }
               }
               
               
               Waypoint currentWaypoint = openSet[lowestF];
               
               if (currentWaypoint == end)
               {
                   //Hier loopt iets fout me de dictionary, uitzoeke wa en why
                   List<Waypoint> path = new List<Waypoint>();
                   
                   path.Insert(0, end);

                   while (currentWaypoint != start)
                   {
                       Debug.Log("Dictionary: " + cameFrom[currentWaypoint]);
                       
                       if (cameFrom.ContainsKey(currentWaypoint))
                       {
                           currentWaypoint = cameFrom[currentWaypoint];
                       }
                       else
                       {
                           currentWaypoint = currentWaypoint.previousWaypoint;
                       }
                       
                       path.Add(currentWaypoint);
                   }
                   
                   path.Reverse();
                   return path;
               }

               openSet.Remove(currentWaypoint);
               closedSet.Add(currentWaypoint);


               if (currentWaypoint.linkedWaypoints.Count > 0)
               {
                   List<Waypoint> connections = GetConnections(currentWaypoint);
                   for (int i = 0; i < connections.Count; i++)
                   {
                       if (closedSet.Contains(connections[i])) continue;

                       float tentativeGScore = currentWaypoint.gScore +
                                               Vector3.Distance(currentWaypoint.transform.position,
                                                   connections[i].transform.position);

                       if (tentativeGScore < connections[i].gScore)
                       {
                           Debug.Log("Addin key " + connections[i] + " with value " + currentWaypoint);

                           cameFrom.Add(currentWaypoint, connections[i]);
                           connections[i].gScore = tentativeGScore;
                           connections[i].hScore =
                               Vector3.Distance(connections[i].transform.position, end.transform.position);

                           if (!openSet.Contains(connections[i]))
                           {
                               openSet.Add(connections[i]);
                           }
                       }
                   }
               }
               else
               {
                   Waypoint NexLink = null;
                   while (NexLink == null)
                   {
                       if (currentWaypoint.linkedWaypoints.Count == 0 )
                       {
                           currentWaypoint = currentWaypoint.nextWaypoint;
                       }
                       else
                       {
                            NexLink = currentWaypoint;
                       }
                       openSet.Add(currentWaypoint);
                   }
               }


           }
           
           Debug.Log("Route not found");
           return null;
       }
       
       
       
       private static List<Waypoint> GetConnections(Waypoint currentWaypoint)
       {
           List<Waypoint> linkedWaypoints = currentWaypoint.linkedWaypoints;
           List<Waypoint> endPoints = new List<Waypoint>();
           if (linkedWaypoints.Count != 0)
           {
               for (int i = 0; i < linkedWaypoints.Count; i++)
               {
                   Waypoint nextLink = linkedWaypoints[i];

                   bool endPointFound = false;
                   while (!endPointFound)
                   {
                       if (nextLink.nextWaypoint.isLink)
                       {
                           nextLink = nextLink.nextWaypoint;
                       }
                       else
                       {
                           endPoints.Add(nextLink.nextWaypoint);
                           endPointFound = true;
                       }
                   }
               }
           }
           
           if (currentWaypoint.nextWaypoint != null)
           {
               endPoints.Add(currentWaypoint.nextWaypoint);
           }

           return endPoints;
       }
       
    }
}