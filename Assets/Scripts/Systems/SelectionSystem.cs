using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using Utils;

public class SelectionSystem : MonoBehaviour
{
    [SerializeField] GameObject markerPrefab;
    [SerializeField] private int markerPoolSize;
    
    private ObjectPool _objectPool;
    private Dictionary<Playable, GameObject>_selectedPlayables = new();

    private void Awake()
    {
        _objectPool = new ObjectPool(markerPrefab, markerPoolSize, true);
    }

    void Update()
    {
        
        bool mouseDown = Input.GetMouseButtonDown(0);
        bool shiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        
        if (!mouseDown) 
            return;
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        
        
        
        if (hit.transform == null || !hit.transform.CompareTag(Tags.Playable))
            return;

        
        Playable selection = hit.transform.GetComponent<Playable>();
        if (shiftDown && _selectedPlayables.ContainsKey(selection))
        {
            Debug.Log("Removing Selection");

            _objectPool.ReturnObject(_selectedPlayables[selection]);
            _selectedPlayables.Remove(selection);
        }
        else if (shiftDown)
        {
            Debug.Log("Adding to selection");
            
            _selectedPlayables.Add(selection, AddMarker(selection));
        }
        else
        {
            Debug.Log("Selected new selection");

            foreach (var playable in _selectedPlayables)
            {
                _objectPool.ReturnObject(playable.Value);
            }
            
            _selectedPlayables = new() { { selection, AddMarker(selection) } };
        }

       
    }
    
    
    private GameObject AddMarker(Playable selection)
    {
        GameObject marker = _objectPool.GetObject();
        
        Vector3 position = selection.transform.position;
        position.y += 1;
        
        marker.transform.position = position;
        marker.transform.parent = selection.transform;
        
        marker.SetActive(true);
        return marker;
    }
}

