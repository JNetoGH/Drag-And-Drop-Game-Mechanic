using System;
using UnityEngine;

public class DraggableObjectController : MonoBehaviour
{
    
    [NonSerialized] public bool IsDropAllowed;
    [NonSerialized] public bool IsCollingWithAnotherObj;
    [NonSerialized] public bool IsHittingTheGround;
    [NonSerialized] public string OtherObjName; // used only in DebuggingCanvas script
    [NonSerialized] public bool AmITheOne = false; // am i the selected one to be dragged
    
    private void AllowDrop(bool value) => IsDropAllowed = value;

    // called when the gmObj is dropped at a prohibited zone and repositioned at previousPos in DragAndDropController.cs
    // it needs to have those value to be set in order to allowDrop, otherwise it will bug a lot
    public void ResetCollisionChecker()
    {
        IsHittingTheGround = true;
        IsCollingWithAnotherObj = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (AmITheOne)
        {
            if (other.gameObject.CompareTag("Drag"))
            {
                IsCollingWithAnotherObj = true;
                OtherObjName = other.gameObject.name;
            }
            if (other.gameObject.CompareTag("Ground")) IsHittingTheGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (AmITheOne)
        {
            if (other.gameObject.CompareTag("Drag")) IsCollingWithAnotherObj = false;
            if (other.gameObject.CompareTag("Ground")) IsHittingTheGround = false;
        }
    }


    // =================================================================================================================
    // =================================================================================================================

    
    private float _flyingHeight;
    private float _dropHeight;
    private Vector3 _previousPos;
    
    Vector3 screenPos;
    Vector3 worldPos;
    
    // INITS OR SETS ALL DEPENDENCIES FOR THE GAME OBJ BE DRAGGED
    public void Init(float flyingHeight, float dropHeight)
    {
        _flyingHeight = flyingHeight;
        _dropHeight = dropHeight;
        _previousPos = transform.position; 
        AmITheOne = true; 
        Cursor.visible = false;
        PosIndicatorMaker.Create(this.gameObject);
    }
    
    private void SetPos(Vector3 pos) => GetComponent<Rigidbody>().position = pos;

    private void FixedUpdate()
    {
        if (AmITheOne)
        {
            if (IsHittingTheGround && !IsCollingWithAnotherObj) AllowDrop(true);
            else AllowDrop(false);

            if (Input.GetMouseButton(0))
            {
                // moves it
                screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
                worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                SetPos(new Vector3(worldPos.x, _flyingHeight, worldPos.z));
            }
            else
            {
                if (!IsDropAllowed)                                                      // 1) in case the player drops the obj in a not allowed position (red indicator color):
                {
                    SetPos(_previousPos);                                                // 2) sets the gmObj back to the previous valid pos 
                    ResetCollisionChecker();                                             // 3) reset the collision checker (bcs this breaks in its pos breaks the Checker a lot)
                }
                else
                {
                    SetPos(new Vector3(worldPos.x, _dropHeight, worldPos.z));           // 4) in case not, drops it where it was released
                }
                
                AmITheOne = false;
                Cursor.visible = true;
                PosIndicatorMaker.FinishIndicator();
                DragAndDropManager.EndSelectedObj();                                        // 5) simply cuts the link to teh manager
            }
        }
    }
    
}