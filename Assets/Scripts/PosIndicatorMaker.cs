using UnityEngine;

public class PosIndicatorMaker : MonoBehaviour
{
    
    /* IT NEEDS TO BE TOUCHING THE GROUND: CheckPosIndicatorCollision.cs depends on it,
     * so it cant be too high because it wont cause no collision with the ground */
    [SerializeField] private float distanceFromGround = 0.01f;
    
    private static GameObject _building;
    public GameObject indicatorPlanePrefab;
    private GameObject _planeInstance;
    private static bool _hasInstantiated = false;
    
    public static void Create(GameObject building) { _building = building; }
    public static void FinishIndicator() { _building = null; }

    private void Indicate()
    {
        if (_building != null)
        {
            if (!_hasInstantiated)
            {
                _planeInstance = Instantiate(indicatorPlanePrefab);
                _hasInstantiated = true;
            }
            Vector3 buildingPos = _building.transform.position;
            _planeInstance.transform.position = new Vector3(buildingPos.x, distanceFromGround, buildingPos.z);
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