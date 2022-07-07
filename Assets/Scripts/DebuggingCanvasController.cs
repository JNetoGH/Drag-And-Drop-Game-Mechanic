using TMPro;
using UnityEngine;

public class DebuggingCanvasController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameVar;
    [SerializeField] private TextMeshProUGUI isCollidingVar;
    [SerializeField] private TextMeshProUGUI isCollidingWithVar;
    [SerializeField] private TextMeshProUGUI isOnFloorVar;
    [SerializeField] private TextMeshProUGUI XVar;
    [SerializeField] private TextMeshProUGUI ZVar;
    
    void Update()
    {
        nameVar.text = DragAndDropManager.IsObjNull ? "null" : DragAndDropManager.SelectedObj.name;
        isCollidingVar.text = DragAndDropManager.IsObjNull ? "null" : DragAndDropManager.SelectedObjScript.IsCollingWithAnotherObj.ToString();
        isCollidingWithVar.text = DragAndDropManager.SelectedObjScript.IsCollingWithAnotherObj ? DragAndDropManager.SelectedObjScript.OtherObjName : "null";
        isOnFloorVar.text = DragAndDropManager.IsObjNull ? "null" : DragAndDropManager.SelectedObjScript.IsHittingTheGround.ToString();

        XVar.text = DragAndDropManager.IsObjNull ? "null" : DragAndDropManager.SelectedObj.transform.position.x.ToString();
        ZVar.text = DragAndDropManager.IsObjNull ? "null" : DragAndDropManager.SelectedObj.transform.position.z.ToString();
    }
}