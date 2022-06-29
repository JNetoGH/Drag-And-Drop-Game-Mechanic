using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosIndicatorMaker : MonoBehaviour
{
    
    // IT NEEDS TO BE TOUCHING THE GROUND: CheckPosIndicatorCollision.cs depends on it,
    // so it cant be too high because it wont cause no collision with the ground
    [SerializeField] private float distanceFromGround = 0.03f;
    
    private static GameObject building;
    public GameObject indicatorPlanePrefab;
    private GameObject _planeInstance;
    private static bool _hasInstantiated = false;
    
    public static void Create(GameObject building)
    {
        PosIndicatorMaker.building = building;
    }

    public static void FinishIndicator()
    {
        PosIndicatorMaker.building = null;
    }

    private void Indicate()
    {
        if (building != null)
        {
            if (!_hasInstantiated)
            {
                _planeInstance = Instantiate(indicatorPlanePrefab);
                _hasInstantiated = true;
            }
            Vector3 buildingPos = building.transform.position;
            _planeInstance.transform.position = new Vector3(buildingPos.x, distanceFromGround, buildingPos.z);
        }
        else if (_hasInstantiated)
        {
            Destroy(_planeInstance);
            _hasInstantiated = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Indicate();
    }
}