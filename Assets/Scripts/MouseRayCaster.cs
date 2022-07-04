using UnityEngine;

public class MouseRayCaster
{
    public RaycastHit hit;
    public void CastNewRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);
    }
    public bool HasHitTag(string tag)
    {
        return hit.collider != null && hit.collider.CompareTag("Drag");
    }
}
