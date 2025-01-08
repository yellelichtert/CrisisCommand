using System.Collections.Generic;
using UnityEngine;

public static class PathFinding
{
    private static Waypoint[] AllWaypoints;

    static PathFinding()
    {
        AllWaypoints = GameObject.FindObjectsOfType<Waypoint>();
    }
    
    public static List<Waypoint> CalculateRoute(Waypoint start, Waypoint end)
    {
        Dictionary<Waypoint, Waypoint> cameFrom = new Dictionary<Waypoint, Waypoint>();

        List<Waypoint> openSet = new List<Waypoint>();
        List<Waypoint> closedSet = new List<Waypoint>();

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
                List<Waypoint> path = new();

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



            if (currentWaypoint.linkedWaypoints.Count == 0 && currentWaypoint.nextWaypoint != null)
            {
                FindNextLink(currentWaypoint);
            }
            else if (currentWaypoint.nextWaypoint != null)
            {
                CheckWaypoint(currentWaypoint, currentWaypoint.nextWaypoint);
                CheckLinks(currentWaypoint);
            }
            else
            {
                CheckLinks(currentWaypoint);
            }
        }

        Debug.Log("Route not found.");
        return null;




        //local Functions
        void CheckWaypoint(Waypoint current, Waypoint next)
        {
            if (closedSet.Contains(next)) return;

            float tentativeGScore =
                current.gScore + Vector3.Distance(current.transform.position, next.transform.position);

            if (current.gScore < tentativeGScore)
            {
                next.gScore = tentativeGScore;
                next.hScore = Vector3.Distance(next.transform.position, end.transform.position);

                if (!openSet.Contains(next)) openSet.Add(next);
            }
        }


        void CheckLinks(Waypoint waypoint)
        {

            List<Waypoint> linkedWaypoints = waypoint.linkedWaypoints;
            Dictionary<Waypoint, Waypoint> endPoints = new();
            if (linkedWaypoints.Count != 0)
            {
                for (int i = 0; i < linkedWaypoints.Count; i++)
                {
                    Waypoint nextLink = linkedWaypoints[i].nextWaypoint;

                    bool endPointFound = false;
                    while (!endPointFound)
                    {
                        if (nextLink.nextWaypoint.isLink)
                        {
                            nextLink = nextLink.nextWaypoint;
                        }
                        else
                        {
                            Debug.Log("Issue is bij endpoints");
                            endPoints.Add(nextLink.nextWaypoint, nextLink);
                            endPointFound = true;
                        }
                    }
                }
            }

            foreach (var endpoint in endPoints)
            {
                
                Debug.Log("Issue is na endPoints");
                CheckWaypoint(waypoint, endpoint.Key);

                if (openSet.Contains(endpoint.Key))
                    cameFrom.Add(endpoint.Key, endpoint.Value);
                    
            }
        }


        void FindNextLink(Waypoint waypoint)
        {
            Waypoint nextLink = waypoint;
            while (nextLink.linkedWaypoints.Count == 0 && nextLink != end)
            {
                if (nextLink.nextWaypoint == null) return;
                nextLink = nextLink.nextWaypoint;
            }

            CheckWaypoint(waypoint, nextLink);
        }
    }

    public static Waypoint FindClosestWaypoint(Vector3 position)
    {
        Waypoint closestWaypoint = null;
        
        var shortestDistance = float.MaxValue;
        foreach (var waypoint in AllWaypoints)
        {
            var distance = Vector3.Distance(waypoint.transform.position, position);
            if (shortestDistance > distance)
            {
                closestWaypoint = waypoint;
                shortestDistance = distance;
            }
        }
        
        return closestWaypoint;
    }
}




