using System;
using UnityEngine;

public class PosIndicatorCollisionChecker : MonoBehaviour
{
    
    // MATERIAL RELATED FIELDS
    [NonSerialized] private Renderer _renderer;
    [SerializeField] public Material greenMaterial;
    [SerializeField] public Material redMaterial;
    
    // CONTROL FIELDS
    [NonSerialized] public static bool IsDropAllowed;
    [NonSerialized] private static bool _isTouchingTheGround;
    [NonSerialized] private static bool _isCollidingWithOtherObjs;
    
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = true;
    }
    
    // DRAG TAG == ANOTHER GAME OBJECT
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Drag")) _isCollidingWithOtherObjs = true;
        if (other.CompareTag("Ground")) _isTouchingTheGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground")) _isTouchingTheGround = false;
        if (other.CompareTag("Drag")) _isCollidingWithOtherObjs = false;
    }
    
    private void SetIsDropAllowed(bool value)
    {
        IsDropAllowed = value;
        _renderer.material = value ? greenMaterial : redMaterial;
    }
    
    private void Update()
    {
        if (_isTouchingTheGround)
        {
            if (_isCollidingWithOtherObjs) SetIsDropAllowed(false);
            else SetIsDropAllowed(true);
        }
        else SetIsDropAllowed(false);
    }

}