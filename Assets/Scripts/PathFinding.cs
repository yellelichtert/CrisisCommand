using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class PathFinding
    {
        public static List<Waypoint> CalculateRoute(Waypoint start, Waypoint end)
        {
            Dictionary<Waypoint, Waypoint> cameFrom = new Dictionary<Waypoint, Waypoint>();
           
            List<Waypoint> openSet = new List<Waypoint>();
            List<Waypoint> closedSet = new List<Waypoint>();
            
            //Get all waypoints
            var allWaypoints = GameObject.FindObjectsOfType<Waypoint>();
            
            //Set the start waypoint
            start.gScore = 0;
            start.hScore = Vector3.Distance(start.transform.position, end.transform.position);
            
            openSet.Add(start);


            while (openSet.Count > 0)
            {
                //Find lowest fScore
                int lowestF = default;
                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].fScore < openSet[lowestF].fScore)
                    {
                        lowestF = i;
                    }
                }
                Waypoint currentWaypoint = openSet[lowestF];

                
                //Handle end waypoint found
                if (currentWaypoint == end)
                {
                    Debug.Log("End waypoint found");
                    List<Waypoint> path = new List<Waypoint>();
                    
                    path.Insert(0, end);

                    while (currentWaypoint != start)
                    {
                        if (cameFrom.TryGetValue(currentWaypoint, out Waypoint previous))
                        {
                            currentWaypoint = previous;
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

                
                Debug.Log("Current waypoint: " + currentWaypoint.name);
                //Handle waypoint without linked waypoints
                
                if (currentWaypoint.linkedWaypoints.Count == 0)
                {
                    Debug.Log("Waypoint without linked waypoints");
                    while (currentWaypoint.linkedWaypoints.Count == 0 && currentWaypoint != end)
                    { 
                        currentWaypoint = currentWaypoint.nextWaypoint;
                        Debug.Log("Moved to next waypoint without links: " + currentWaypoint.name);
                    }
                    
                    Debug.Log("Found next waypoint with links: " + currentWaypoint.name);
                    if (!openSet.Contains(currentWaypoint))
                    {
                        Debug.Log("Adding waypoint");
                        openSet.Add(currentWaypoint);
                    }
                }
                else //Handle waypoint with linked waypoints
                {
                    Debug.Log("Waypoint with linked waypoints");
                    
                    List<Waypoint> connections = currentWaypoint.linkedWaypoints;
                    
                    Debug.Log("CURRENT WAYPOINT: " + currentWaypoint.name);
                    
                    for (int i = 0; i < connections.Count; i++)
                    {
                        if (closedSet.Contains(connections[i])) continue;
                        
                        Debug.Log("Closed set does not contain: " + connections[i].name);

                        Waypoint nextPoint = null;
                        if (connections[i].isLink)
                        {
                            nextPoint = FindEndOfLink(connections[i]).nextWaypoint;
                        }
                        else
                        {
                            nextPoint = connections[i];
                        }
                        
                        currentWaypoint.gScore = Vector3.Distance(currentWaypoint.transform.position, currentWaypoint.previousWaypoint.transform.position);
                        nextPoint.gScore = Vector3.Distance(currentWaypoint.transform.position, nextPoint.transform.position);
                        nextPoint.hScore = Vector3.Distance(nextPoint.transform.position, end.transform.position);
                        
                        Debug.Log("currentWaypoint Gscore : " + currentWaypoint.gScore);
                        Debug.Log("hscore: " + nextPoint.hScore);
                        Debug.Log("gscore: " + nextPoint.gScore);
                        
                        var tentativeGScore = currentWaypoint.gScore + nextPoint.gScore;
                        Debug.Log("tentativeGScore: " + tentativeGScore);
                        
                        Debug.Log("nexpoint: " + nextPoint.name);
                        
                        if (tentativeGScore > currentWaypoint.gScore)
                        {
                            Debug.Log("Adding key " + nextPoint + " with value " + FindEndOfLink(connections[i]));;
                            cameFrom.Add(nextPoint, FindEndOfLink(connections[i]));
                            nextPoint.gScore = tentativeGScore;
                            nextPoint.hScore = Vector3.Distance(nextPoint.transform.position, end.transform.position);
                            
                            if (!openSet.Contains(nextPoint))
                            {
                                openSet.Add(nextPoint);
                            }
                        }
                    }

                    if (currentWaypoint.nextWaypoint != null)
                    {
                        Debug.Log("NEED TO FIX NEXT WAYPOINT");
                    }
                    
                }
            }
            
            //Route not found
            Debug.Log("Route not found.");
            return null;
        }

        
        
        
        private static Waypoint FindEndOfLink(Waypoint connection)
        {
            Debug.Log("Looking for end of link");
            Debug.Log("Connection: " + connection.name);
            while (connection.isLink)
            {
                connection = connection.nextWaypoint;
            }
            
            Debug.Log("Found end of link: " + connection.name);
            return connection;
        }
    }
}