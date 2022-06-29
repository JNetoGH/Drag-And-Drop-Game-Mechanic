using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

    private enum DragOrDropCodes
    {
        Drag = 0, 
        Drop = 1
    }
    
    private GameObject _selectedObject;
    [SerializeField] private float flyingHeight = 1.25f;
    [SerializeField] private float droppedHeight = 0.5f;
    
    // used to replace teh obj at the last position in case the player tries a prohibited zone
    // those zones are setted by the "Position Indicator Prefab" using the CheckPosIndicatorCollision.cs script
    private Vector3 _initialPos;

    // called once per frame by Unity3D
    private void Update()
    {
        // assigns the game object: runs when clicked and we don't have a obj assigned to the global variable yet 
        if (Input.GetMouseButtonDown(0) && _selectedObject == null)
        {
            RaycastHit hit = CastRay();
            if (hit.collider != null)
            {
                // if its not a drag tagged obj it exists the method ignoring the rest
                if (!hit.collider.CompareTag("Drag")) return; 
                
                // what happens whe the ray finds a Drag tagged object
                _selectedObject = hit.collider.gameObject;          // assigns the global field of the object 
                Cursor.visible = false;                             // when its dragging it makes the cursor invisible
                PosIndicatorMaker.Create(_selectedObject);          // turns indicator on: instantiates a new one
                _initialPos = _selectedObject.transform.position;   // gets the initial position
            }
        }

        // drags an object
        if (_selectedObject != null) DragOrDrop(DragOrDropCodes.Drag);
        // drops the game obj and unassigns the globar var: runs when we unclick and we have a obj assigned to the global variable
        if (_selectedObject != null  && Input.GetMouseButtonUp(0)) DragOrDrop(DragOrDropCodes.Drop);
    }

    private void DragOrDrop(DragOrDropCodes code)
    {
        Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(_selectedObject.transform.position).z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        _selectedObject.transform.position = new Vector3(worldPos.x, code == DragOrDropCodes.Drag ? flyingHeight : droppedHeight, worldPos.z);

        // what happens when you release the Drag tagged object
        if (code == DragOrDropCodes.Drop)
        {
            // in case the player drops the obj in a not allowed position e.g red, it returns the obj to its initial pos
            if (!CheckPosIndicatorCollision.IsDropAllowed)
                _selectedObject.transform.position = _initialPos;
            
            _selectedObject = null; // unassings the obj
            Cursor.visible = true; // sets the cursor to viseble
            PosIndicatorMaker.FinishIndicator();  // turns indicator off: destroys the current one
        }
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);
        return hit;
    }
    
}