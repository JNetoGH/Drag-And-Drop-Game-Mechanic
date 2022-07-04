using UnityEngine;

public class DragAndDropManager : MonoBehaviour {
    
    private enum DragOrDropCodes { Drag = 0, Drop = 1 }
    private static bool Clicked => Input.GetMouseButtonDown(0);
    private static bool Released => Input.GetMouseButtonUp(0);
    private readonly MouseRayCaster _mouseRay = new MouseRayCaster();

    private GameObject _selectedObj;
    private bool IsObjNull => _selectedObj == null;
    private Vector3 _previousPos; // replaces the obj pos in case the player tries to drop it at a prohibited zone
    
    [SerializeField] private float flyingHeight = 2f;
    [SerializeField] private float droppedHeight = 0.5f;

    private void Update() 
    {
        _mouseRay.CastNewRay();
        if (IsObjNull && Clicked && _mouseRay.HasHitTag("Drag")) InitDrag(_mouseRay.hit.collider.gameObject); 
        if (!IsObjNull) DragOrDrop(DragOrDropCodes.Drag);                                     
        if (!IsObjNull && Released) DragOrDrop(DragOrDropCodes.Drop);                   
    }                                                                                                
    
    private void InitDrag(GameObject gmObj)
    { 
        _selectedObj = gmObj;                           // 1) assigns the global field of the object 
        Cursor.visible = false;                         // 2) when its dragging it makes the cursor invisible
        PosIndicatorMaker.Create(_selectedObj);         // 3) turns indicator on: instantiates a new one
        _previousPos = _selectedObj.transform.position; // 4) stores the initial position
    }
    
    private void EndDrag()
    {
        _selectedObj = null;                            // 1) unassings the obj
        Cursor.visible = true;                          // 2) sets the cursor to viseble
        PosIndicatorMaker.FinishIndicator();            // 3 turns indicator off: destroys the current one
    }

    private void DragOrDrop(DragOrDropCodes code)
    {
        Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(_selectedObj.transform.position).z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        _selectedObj.transform.position = new Vector3(worldPos.x, code == DragOrDropCodes.Drag ? flyingHeight : droppedHeight, worldPos.z);
        if (code == DragOrDropCodes.Drop)                       // what happens when you release the Drag tagged object
        {
            if (!PosIndicatorController.IsDropAllowed)          // in case the player drops the obj in a not allowed position (red indicator color)
            {   
                _selectedObj.transform.position = _previousPos; // 1) sets the gmObj back to the previous valid pos 
                PosIndicatorController.ResetCollisionChecker(); // 2) reset the collision checker (bcs this breaks in its pos breaks the Checker a lot)
            }
            EndDrag();                                          // simply ends dragging
        }
    }
    
}