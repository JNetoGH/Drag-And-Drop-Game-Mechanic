using UnityEngine;

public class DragAndDrop : MonoBehaviour {
    
    private enum DragOrDropCodes { Drag = 0, Drop = 1 }
    
    // GAME OBJECT RELATED FIELDS:
    private GameObject _selectedObj;
    [SerializeField] private float flyingHeight = 2f;
    [SerializeField] private float droppedHeight = 0.5f;
    private Vector3 _previousPos; // replaces the obj last pos in case the player tries to drop it at a prohibited zone
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _selectedObj == null)    // tries to assign the game object when mouse button clicked and we don't have a obj assigned to the global variable yet 
        {
            RaycastHit hit = CastRay();                             // 1) creates a RayCastHit based on the mouse click
            if (hit.collider != null && hit.collider.tag == "Drag") // 2) if the ray hits a collider and its a Drag obj
            {                                                       // THAT'S WHAT HAPPENS WHEN THE RAYS HITS A DRAG OBJ:
                _selectedObj = hit.collider.gameObject;             // 3) assigns the global field of the object 
                Cursor.visible = false;                             // 4) when its dragging it makes the cursor invisible
                PosIndicatorMaker.Create(_selectedObj);             // 5) turns indicator on: instantiates a new one
                _previousPos = _selectedObj.transform.position;     // 6) gets the initial position
            }
        }
        if (_selectedObj != null) DragOrDrop(DragOrDropCodes.Drag); // drags an object
        if (_selectedObj != null && Input.GetMouseButtonUp(0)) DragOrDrop(DragOrDropCodes.Drop); // drops the game obj and unassigns the globar var: runs when we unclick and we have a obj assigned to the global variable
    }
    
    private void DragOrDrop(DragOrDropCodes code)
    {
        Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(_selectedObj.transform.position).z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        _selectedObj.transform.position = new Vector3(worldPos.x, code == DragOrDropCodes.Drag ? flyingHeight : droppedHeight, worldPos.z);
        if (code == DragOrDropCodes.Drop)       // what happens when you release the Drag tagged object
        {   // in case the player drops the obj in a not allowed position e.g red, it returns the obj to its initial pos
            if (!PosIndicatorCollisionChecker.IsDropAllowed) _selectedObj.transform.position = _previousPos; 
            _selectedObj = null;                 // unassings the obj
            Cursor.visible = true;               // sets the cursor to viseble
            PosIndicatorMaker.FinishIndicator(); // turns indicator off: destroys the current one
        }
    }
    
    private static RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);
        Debug.DrawRay(worldMousePosNear, worldMousePosFar - worldMousePosNear, Color.magenta); // draws the ray on scene view
        return hit;
    }
    
}