using System;
using UnityEngine;

public class PosIndicatorController : MonoBehaviour
{
    private static Renderer _renderer;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    
    public static bool IsDropAllowed;
    public static bool IsCollingWithAnotherObj;
    public static bool IsHittingTheGround;
    public static string OtherObjName; // used only in DebuggingCanvas script
    
    private void AllowDrop(bool value)
    {
        IsDropAllowed = value;
        _renderer.material = value ? greenMaterial : redMaterial;
    }
    
    private void Start() => _renderer = GetComponent<Renderer>();

    private void OnTriggerStay(Collider other)
    {
        if (!DragAndDropManager.IsObjNull) // throws an exception if its null
        {
            if (other.gameObject.CompareTag("Drag") && ! other.name.Equals(DragAndDropManager.SelectedObj.name))
            {
                IsCollingWithAnotherObj = true;
                OtherObjName = other.gameObject.name;
            }
            if (other.gameObject.CompareTag("Ground")) IsHittingTheGround = true;   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!DragAndDropManager.IsObjNull) // throws an exception if its null
        {
            if (other.gameObject.CompareTag("Drag")) IsCollingWithAnotherObj = false;
            if (other.gameObject.CompareTag("Ground")) IsHittingTheGround = false;
        }
    }

    private void Update() 
    {
        if (IsHittingTheGround && !IsCollingWithAnotherObj) AllowDrop(true);
        else AllowDrop(false);
    }
    
    // called when the gmObj is dropped at a prohibited zone and repositioned at previousPos in DragAndDropController.cs
    // it needs to have those value to be set in order to allowDrop, otherwise it will bug
    public static void ResetCollisionChecker() 
    {
        IsHittingTheGround = true;
        IsCollingWithAnotherObj = false;
    }
}