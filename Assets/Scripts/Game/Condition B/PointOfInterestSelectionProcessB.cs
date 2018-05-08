using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestSelectionProcessB : MonoBehaviour, IPointOfInterestSelectionProcess
{
    public string CurrentRoom;

    public float CheckDistance = 9.0f;
    public float CoolOff = 2f;
    public LayerMask RaycastLayers;

    private float _currenTimeCooloff;

    private GameObject _newPOI;
    private GameObject _currentPOI;

    public GameObject GetPointOfInterest(PointsOfInterest pois)
    {
        float smallestDistance = 200f;
        int smallestDistanceCounter = -1;

        for (int i = 0; i < pois.PoiEssential.Count; i++)
        {
            if (IsCandidate(pois.PoiEssential[i]))
            {
                float distance = Vector3.Distance(Camera.main.transform.position,
                    pois.PoiEssential[i].transform.position);

                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    smallestDistanceCounter = i;
                }
            }
        }

        if (smallestDistanceCounter != -1)
        {
            _newPOI = pois.PoiEssential[smallestDistanceCounter];

            if (_newPOI != _currentPOI && _currenTimeCooloff >= CoolOff)
            {
                if (_currentPOI && PointIsWithinFieldOfView(_currentPOI.transform.position))
                {
                    return _currentPOI;
                }

                if (_currentPOI)
                {
                    DataRecorderNew.Instance.AddNewDataSet(Time.time, _currentPOI,
                        DataRecorderNew.Action.UnmarkedAsPOI);
                }

                DataRecorderNew.Instance.AddNewDataSet(Time.time, _newPOI, DataRecorderNew.Action.MarkedAsPOI);
                _currentPOI = _newPOI;
            }
            else if (_newPOI == _currentPOI)
            {
                _currenTimeCooloff = 0;
            }

            return _currentPOI;
        }

        if (_currentPOI != null)
        {
            DataRecorderNew.Instance.AddNewDataSet(Time.time, _currentPOI, DataRecorderNew.Action.UnmarkedAsPOI);
            _currentPOI = null;
        }

        return null;
    }

    private bool IsCandidate(GameObject toTest)
    {
        if (toTest.transform.parent.name == CurrentRoom)
        {
            if (Vector3.Distance(Camera.main.transform.position, toTest.transform.position) < CheckDistance)
            {
                if (PointIsWithinFieldOfView(toTest.transform.position))
                {
                    RaycastHit hit;

                    if (Physics.Linecast(Camera.main.transform.position, toTest.transform.position, out hit,
                        RaycastLayers))
                    {
                        return false;
                    }

                    return true;
                }
            }
        }

        return false;
    }

    private bool PointIsWithinFieldOfView(Vector3 point)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(point);
        return viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 &&
               viewportPoint.y < 1;
    }

    private void Update()
    {
        _currenTimeCooloff += Time.deltaTime;
    }
}