using System;
using UnityEngine;

public class PosIndicatorController : MonoBehaviour
{
    private static Renderer _renderer;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    
    public static bool IsDropAllowed;
    private static bool _isCollingWithAnotherObj;
    private static bool _isHittingTheGround;
    
    private void AllowDrop(bool value)
    {
        IsDropAllowed = value;
        _renderer.material = value ? greenMaterial : redMaterial;
    }
    
    private void Start() => _renderer = GetComponent<Renderer>();

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Drag")) _isCollingWithAnotherObj = true;
        if (other.gameObject.CompareTag("Ground")) _isHittingTheGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Drag")) _isCollingWithAnotherObj = false;
        if (other.gameObject.CompareTag("Ground")) _isHittingTheGround = false;
    }

    private void Update()
    {
        if (_isHittingTheGround && !_isCollingWithAnotherObj) AllowDrop(true);
        else AllowDrop(false);
        Debug.Log($"hit ground = {_isHittingTheGround} | hit gmObj = {_isCollingWithAnotherObj}  ");
    }
    
    // called when the gmObj is dropped at a prohibited zone and repositioned at previousPos in DragAndDropController.cs
    // it needs to have those value to be set in order to allowDrop, otherwise it will bug
    public static void ResetCollisionChecker() 
    {
        _isHittingTheGround = true;
        _isCollingWithAnotherObj = false;
    }
}