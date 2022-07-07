using UnityEngine;

public class PosIndicatorController : MonoBehaviour
{
    private static Renderer _renderer;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    
    private void Start() => _renderer = GetComponent<Renderer>();
    private void Update() => 
        _renderer.material = DragAndDropManager.SelectedObj.GetComponent<DraggableObjectController>().IsDropAllowed ? greenMaterial : redMaterial;
}