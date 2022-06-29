using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPosIndicatorCollision : MonoBehaviour
{
    [SerializeField] public Material greenMaterial;
    [SerializeField] public Material redMaterial;
    [NonSerialized] public static bool IsDropAllowed;
    
    private Renderer _renderer;
    private static readonly Color Green = new Color(95, 185, 67);

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = true;
    }

    // it turns red if collides with another object or stops colling with teh ground
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            _renderer.material = greenMaterial;
            IsDropAllowed = true;
        }
        else
        {
            IsDropAllowed = false;
            _renderer.material = redMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            IsDropAllowed = false;
            _renderer.material = redMaterial;
        }
        else
        {
            IsDropAllowed = true;
            _renderer.material = greenMaterial;
        }
    }
}