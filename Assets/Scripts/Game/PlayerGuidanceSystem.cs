using System.Collections.Generic;
using UnityEngine;

public class PlayerGuidanceSystem : MonoBehaviour
{

	public GameObject Player;
	public GameObject Fence;

	public Vector3 PlayerStartPosition;
	public List<Vector3> MissionPoints;
	public List<bool> MissionSolved;

	private LineRenderer _lineRenderer;

	private int _currentIndex;

	void Start ()
	{
		Player.transform.position = PlayerStartPosition;
		
		_lineRenderer = GetComponent<LineRenderer>();
		_lineRenderer.positionCount = 2;
		
		SetLineRendererToCurrentMission();
		
		
	}
	
	void Update () {
		
		_lineRenderer.SetPosition(0, new Vector3(Player.transform.position.x, 0.2f, Player.transform.position.z));
		
		if (!IsWithinRadius() || MissionSolved[IndexOfClosestMission()])
		{
			_lineRenderer.enabled = true;
			Fence.GetComponent<MeshCollider>().enabled = false;
		}
		else
		{
			_lineRenderer.enabled = false;
			Fence.transform.position = VectorOfClosestMission();
			Fence.GetComponent<MeshCollider>().enabled = true;
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			MarkMissionAsSolved();
		}
	}

	private void MarkMissionAsSolved()
	{
		MissionSolved[_currentIndex] = true;
		if (_currentIndex < MissionPoints.Count - 1)
		{
			_currentIndex++;
		}
		SetLineRendererToCurrentMission();
	}

	public bool IsWithinRadius()
	{
		for (int i = 0; i < MissionPoints.Count; i++)
		{
			if (Vector3.Distance(Player.transform.position, MissionPoints[i]) < 12)
			{
				return true;
			}
		}
		return false;
	}

	private int IndexOfClosestMission()
	{
		for (int i = 0; i < MissionPoints.Count; i++)
		{
			if (Vector3.Distance(Player.transform.position, MissionPoints[i]) < 12)
			{
				return i;
			}
		}

		return 0;
	}
	
	public Vector3 VectorOfClosestMission()
	{
		for (int i = 0; i < MissionPoints.Count; i++)
		{
			if (Vector3.Distance(Player.transform.position, MissionPoints[i]) < 12)
			{
				return MissionPoints[i];
			}
		}

		return Vector3.positiveInfinity;
	}
	
	private void SetLineRendererToCurrentMission()
	{
		_lineRenderer.SetPosition(1, new Vector3(MissionPoints[_currentIndex].x, 0.2f, MissionPoints[_currentIndex].z));
	}

}
