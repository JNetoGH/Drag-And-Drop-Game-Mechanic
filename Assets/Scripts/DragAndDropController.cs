using UnityEngine;

public class DragAndDropController : MonoBehaviour {
    
    private enum DragOrDropCodes { Drag = 0, Drop = 1 }
    private static bool HasClickedBtn => Input.GetMouseButtonDown(0);
    private static bool HasReleasedBtn => Input.GetMouseButtonUp(0);
    
    private GameObject _selectedObj;
    [SerializeField] private float flyingHeight = 2f;
    [SerializeField] private float droppedHeight = 0.5f;
    private Vector3 _previousPos; // replaces the obj pos in case the player tries to drop it at a prohibited zone
    
    private void Update() 
    {                                                                                                       // 1) TRIES TO ASSIGN A GAME OBJECT WHEN MOUSE BUTTON CLICKED:
        RaycastHit hit = CastRay();                                                                         // 2) creates a RaycastHit based on the mouse click to check 
        if (_selectedObj == null && HasClickedBtn && HasHit(hit)) InitDragging(hit.collider.gameObject);    // 3) if the gmObj field haven't been initialized, the mouse button has been clicked and the casted ray hits a collider of a Drag tagged gmObj: the gmObj will be assigned for dragging
        if (_selectedObj != null) DragOrDrop(DragOrDropCodes.Drag);                                     // 4) if there is an obj assigned for dragging in the gmObj field: the assigned gmObj will be dragged
        if (_selectedObj != null && HasReleasedBtn) DragOrDrop(DragOrDropCodes.Drop);                   // 5) ... and if the dragging button has been released: drops the game obj and unassigns the field, if it is in a valid position, else, it will be returned to the _previousPos
    }                                                                                                
    
    private static RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);
        return hit;
    }

    private static bool HasHit(RaycastHit hit) => hit.collider != null && hit.collider.CompareTag("Drag");

    private void InitDragging(GameObject gmObj)
    {
        _selectedObj = gmObj;                           // 1) assigns the global field of the object 
        Cursor.visible = false;                         // 2) when its dragging it makes the cursor invisible
        PosIndicatorMaker.Create(_selectedObj);         // 3) turns indicator on: instantiates a new one
        _previousPos = _selectedObj.transform.position; // 4) stores the initial position
    }
     
    private void DragOrDrop(DragOrDropCodes code)
    {
        Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(_selectedObj.transform.position).z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        _selectedObj.transform.position = new Vector3(worldPos.x, code == DragOrDropCodes.Drag ? flyingHeight : droppedHeight, worldPos.z);
        if (code == DragOrDropCodes.Drop)       // what happens when you release the Drag tagged object
        {                                       // in case the player drops the obj in a not allowed position e.g red, it returns the obj to its initial pos
            if (!PosIndicatorCollisionChecker.IsDropAllowed) _selectedObj.transform.position = _previousPos; 
            _selectedObj = null;                 // unassings the obj
            Cursor.visible = true;               // sets the cursor to viseble
            PosIndicatorMaker.FinishIndicator(); // turns indicator off: destroys the current one
        }
    }
    
}