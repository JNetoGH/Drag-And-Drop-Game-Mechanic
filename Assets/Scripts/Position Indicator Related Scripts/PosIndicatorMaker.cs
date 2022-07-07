using UnityEngine;

public class PosIndicatorMaker : MonoBehaviour
{
    [SerializeField] private float distanceFromGround = 0.01f;
    private static GameObject _gmObj;
    public GameObject indicatorPlanePrefab;
    private GameObject _planeInstance;
    private static bool _hasInstantiated = false;
    
    public static void Create(GameObject obj) =>  _gmObj = obj;
    public static void FinishIndicator() => _gmObj = null;

    private void Indicate()
    {
        if (_gmObj != null)
        {
            if (!_hasInstantiated)
            {
                _planeInstance = Instantiate(indicatorPlanePrefab);
                _hasInstantiated = true;
            }
            Vector3 objPos = _gmObj.transform.position;
            _planeInstance.transform.position = new Vector3(objPos.x, distanceFromGround, objPos.z);
        }
        else if (_hasInstantiated)
        {
            Destroy(_planeInstance);
            _hasInstantiated = false;
        }
    }
    
    private void Update()
    {
        Indicate();
    }
}