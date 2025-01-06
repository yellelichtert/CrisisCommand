using System;
using DefaultNamespace;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

public class WaypointManager : EditorWindow
{
    public int nodeDistance;
    public Transform waypointRoot;
    [CanBeNull] private Waypoint _selectedWaypoint;

    private void OnEnable()
        => Selection.selectionChanged += OnSelectionChanged;
    

    [MenuItem("Tools/Waypoint Editor")]
     public static void Open()
     {
         GetWindow<WaypointManager>();
     }

    private void OnSelectionChanged()
    {
        Waypoint waypoint = Selection.activeGameObject?.GetComponent<Waypoint>();
        _selectedWaypoint = waypoint ? waypoint : null;
        Repaint();
    }

    
    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);
        
        EditorGUILayout.PropertyField(obj.FindProperty(nameof(waypointRoot)));
        
        if (!waypointRoot)
        {
            EditorGUILayout.HelpBox("No root selected", MessageType.Warning);
        }
        else if (_selectedWaypoint)
        {
            SerializedObject waypointObj = new SerializedObject(_selectedWaypoint);
         
            EditorGUILayout.Space(width: 20);
            EditorGUILayout.LabelField(_selectedWaypoint.name, EditorStyles.boldLabel);
            
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(waypointObj.FindProperty(nameof(_selectedWaypoint.previousWaypoint)));
            EditorGUILayout.PropertyField(waypointObj.FindProperty(nameof(_selectedWaypoint.nextWaypoint)));
            EditorGUILayout.EndVertical();
        }
        else
        {
            EditorGUILayout.HelpBox("No waypoint selected", MessageType.Info);
        }

        if (WaypointEditor.SelectedRoute != null)
        {
            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Selected Route Points", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginScrollView(new Vector2(0, 0));
            foreach(Waypoint waypoint in WaypointEditor.SelectedRoute)
            {
                EditorGUILayout.LabelField(waypoint.name);
            }
            EditorGUILayout.EndScrollView();
        }
        
        DrawButtons();
        obj.ApplyModifiedProperties();
    }
    
    private void DrawButtons()
    {
        var selection = Selection.objects;
        if (selection.Length == 2 && selection[0].GameObject().GetComponent<Waypoint>() && selection[1].GameObject().GetComponent<Waypoint>())
        {
            if (GUILayout.Button("Connect waypoints"))
            {
                ConnectWayPoints();
            }
            if (GUILayout.Button("Connect as link"))
            {
                ConnectWayPoints(true);
            }
        }

        if (_selectedWaypoint && GUILayout.Button("Create Next Waypoint")) 
        {
            CreateNextWayPoint();
        }

        if (_selectedWaypoint && GUILayout.Button(_selectedWaypoint.isLink ? "Continue link" : "Create link" ))
        {
            CreateLink(_selectedWaypoint.isLink);
        }
        
        if (waypointRoot && GUILayout.Button("Create Unlinked Waypoint"))
        {
            CreateUnLinkedWaypoint();
        }

        if(Selection.objects.Length == 2 && Selection.objects[0].GameObject().GetComponent<Waypoint>() && Selection.objects[1].GameObject().GetComponent<Waypoint>() && GUILayout.Button("Caluclate Route"))
        {
            WaypointEditor.SelectedRoute = PathFinding.CalculateRoute(Selection.objects[0].GameObject().GetComponent<Waypoint>(),
                Selection.objects[1].GameObject().GetComponent<Waypoint>());
        }
        
        if (WaypointEditor.SelectedRoute != null && GUILayout.Button("Clear Route"))
        {
            WaypointEditor.SelectedRoute = null;
        }
    }


    
    private void ConnectWayPoints(bool isLink = false)
    {
        Waypoint waypoint0 = Selection.objects[0].GameObject().GetComponent<Waypoint>();
        Waypoint waypoint1 = Selection.objects[1].GameObject().GetComponent<Waypoint>();
        
        waypoint0.nextWaypoint = waypoint1;
        waypoint1.previousWaypoint = waypoint0;

        if (isLink && waypoint0.isLink)
        {
            waypoint0.nextWaypoint = waypoint1;
            waypoint1.previousWaypoint = waypoint0;
        }
    }
    
    
    private Waypoint CreateWaypoint()
    {
         GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
         waypointObject.transform.SetParent(waypointRoot, false);

         return waypointObject.GetComponent<Waypoint>();
    }
    
         private void CreateNextWayPoint()
     {
         Waypoint waypoint = CreateWaypoint();
         
         if (waypointRoot.childCount > 1)
         {
             waypoint.previousWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
             waypoint.previousWaypoint.nextWaypoint = waypoint;
             waypoint.transform.position = waypoint.previousWaypoint.transform.position;
             waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
         }

         Selection.activeGameObject = waypoint.gameObject;
     }
    
         
    private void CreateUnLinkedWaypoint()
    {
        Waypoint waypoint = CreateWaypoint();
         
        if (waypointRoot.childCount > 1)
        {
             
            waypoint.transform.position = waypointRoot.GetChild(waypointRoot.childCount - 2).position + new Vector3(1,0,0);
            waypoint.transform.forward = waypointRoot.GetChild(waypointRoot.childCount - 2).forward;
        }

        Selection.activeGameObject = waypoint.gameObject;
    }

    private void CreateLink(bool isExistingLink)
    {
        Waypoint waypoint = CreateWaypoint();
        waypoint.isLink = true;
        
        if (waypointRoot.childCount > 1)
        {
            waypoint.previousWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
            waypoint.transform.position = waypoint.previousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
        }

        if (!isExistingLink)
        {
            waypoint.previousWaypoint.linkedWaypoints.Add(waypoint);
        }
        else
        {
            waypoint.previousWaypoint.nextWaypoint = waypoint;
        }

        Selection.activeGameObject = waypoint.gameObject;
    }
    
    
}
