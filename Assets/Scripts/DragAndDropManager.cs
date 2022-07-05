using UnityEngine;

public class DragAndDropManager : MonoBehaviour {
    
    // DRAG N' DROP AND RAY CAST RELATED
    private enum DragOrDropCodes { Drag = 0, Drop = 1 }
    private static bool Clicked => Input.GetMouseButtonDown(0);
    private static bool Released => Input.GetMouseButtonUp(0);
    private readonly MouseRayCaster _mouseRay = new MouseRayCaster();
    
    // GAME OBJECT RELATED
    public static GameObject SelectedObj;
    public static bool IsObjNull => SelectedObj == null;
    private Vector3 _previousPos; // repositions the gmObj in case the player tries to drop it at a prohibited zone
    [SerializeField] private float flyingHeight = 2f;
    [SerializeField] private float droppedHeight = 0.5f;

    private void Update() 
    {                                                                                           // TRIES TO ASSIGN A GM_OBJ WHEN MOUSE BTN IS CLICKED, WHEN BTN IS RELEASED, GM_OBJ IS DROPPED
        _mouseRay.CastNewRay();                                                                 // 2) creates a RaycastHit based on the mouse click to check if it has hit anything
        if (IsObjNull && Clicked && _mouseRay.HasHitTag("Drag")) InitDrag(_mouseRay.gmObjHit);  // 3) if gmObj isn't assigned, mouse btn clicked and the ray has hit a Grag tagged gmObj: sets up things for dragging it
        if (!IsObjNull) DragOrDrop(DragOrDropCodes.Drag);                                   // 4) if gmObj is assigned: drags it   
        if (!IsObjNull && Released) DragOrDrop(DragOrDropCodes.Drop);                       // 4) 3) if gmObj is assigned anf mouse btn released: stops dragging and drops it
    }                                                                                                
    
    private void InitDrag(GameObject gmObj)
    { 
        SelectedObj = gmObj;                           // 1) assigns the global field of the object 
        Cursor.visible = false;                         // 2) when its dragging it makes the cursor invisible
        PosIndicatorMaker.Create(SelectedObj);         // 3) turns indicator on: instantiates a new one
        _previousPos = SelectedObj.transform.position; // 4) stores the initial position
    }
    
    private void EndDrag()
    {
        SelectedObj = null;                            // 1) unassings the obj
        Cursor.visible = true;                          // 2) sets the cursor to viseble
        PosIndicatorMaker.FinishIndicator();            // 3 turns indicator off: destroys the current one
    }

    private void DragOrDrop(DragOrDropCodes code)
    {
        Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(SelectedObj.transform.position).z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        SelectedObj.transform.position = new Vector3(worldPos.x, code == DragOrDropCodes.Drag ? flyingHeight : droppedHeight, worldPos.z);
      
        if (code == DragOrDropCodes.Drop)                       // WHAT HAPPENS WHEN YOU RELEASE THE DRAG TAGGED OBJECT
        {
            if (!PosIndicatorController.IsDropAllowed)          // in case the player drops the obj in a not allowed position (red indicator color):
            {   
                SelectedObj.transform.position = _previousPos; // 1) sets the gmObj back to the previous valid pos 
                PosIndicatorController.ResetCollisionChecker(); // 2) reset the collision checker (bcs this breaks in its pos breaks the Checker a lot)
            }
            EndDrag();                                          // simply ends dragging
        }
    }
    
}