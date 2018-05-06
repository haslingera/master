using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntensityManager : MonoBehaviour
{

	public ConditionType CurrentConditionType;
	
	public float CurrentIntensityAMin;
	public float CurrentIntensityAMax;
	public float CurrentIntensityBMin;
	public float CurrentIntensityBMax;
	
	private float _intensityAMin;
	private float _intensityAMax;
	private float _intensityBMin;
	private float _intensityBMax;

	public static IntensityManager Instance;

	void Awake ()   
	{
		if (Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy (gameObject);
		}
	}
	
	public enum ConditionType {None, A, B}

	public float IntensityMin
	{
		get
		{
			
			if(CheckCondition() == ConditionType.A)
			{
				if (_intensityAMin <= 0)
				{
					return 0.095f;
				}
				return _intensityAMin;
			}
			
			if(CheckCondition() == ConditionType.B)
			{
				if (_intensityBMin <= 0)
				{
					return 0.095f;
				}
				return _intensityBMin;
			}
			
			return 0f;
			
		}
	}
	
	public float IntensityMax
	{
		get
		{
			
			if(CheckCondition() == ConditionType.A)
			{
				if (_intensityAMax <= 0)
				{
					return 0.095f;
				}
				return _intensityAMax;
			}
			
			if(CheckCondition() == ConditionType.B)
			{
				if (_intensityBMax <= 0)
				{
					return 0.095f;
				}
				return _intensityBMax;
			}
			
			return 0f;
			
		}
	}

	public void AddGazeIntensitySampleMin(float value)
	{
		if (CheckCondition() == ConditionType.A)
		{
			_intensityAMin = value;
		}
		
		if (CheckCondition() == ConditionType.B)
		{
			_intensityBMin = value;
		}
	}
	
	public void AddGazeIntensitySampleMax(float value)
	{
		if (CheckCondition() == ConditionType.A)
		{
			_intensityAMax = value;
		}
		
		if (CheckCondition() == ConditionType.B)
		{
			_intensityBMax = value;
		}
	}

	private ConditionType CheckCondition()
	{
		
		if (SceneManager.GetActiveScene().name.Contains("A"))
		{
			 return ConditionType.A;
		}
		
		if (SceneManager.GetActiveScene().name.Contains("B"))
		{
			return ConditionType.B;
		}

		return ConditionType.None;

	}
	
	public float GetIntensityMin(ConditionType conditionType)
	{
			if(CheckCondition() == ConditionType.A)
			{
				return _intensityAMin;
			}
			
			if (CheckCondition() == ConditionType.B)
			{
				return _intensityBMin;
			}
			
			return 0f;
	}
	
	public float GetIntensityMax(ConditionType conditionType)
	{
		if(CheckCondition() == ConditionType.A)
		{
			return _intensityAMax;
		}
			
		if (CheckCondition() == ConditionType.B)
		{
			return _intensityBMax;
		}
			
		return 0f;
	}

	private void Update()
	{
		CurrentConditionType = CheckCondition();
		
		CurrentIntensityAMin = GetIntensityMin(ConditionType.A);
		CurrentIntensityAMax = GetIntensityMax(ConditionType.A);

		CurrentIntensityBMin = GetIntensityMin(ConditionType.B);
		CurrentIntensityBMax = GetIntensityMax(ConditionType.B);
	}

	public void ResetIntensities()
	{
		if (CheckCondition() == ConditionType.A)
		{
			_intensityAMax = 0;
			_intensityAMin = 0;
		}
		
		if (CheckCondition() == ConditionType.B)
		{
			_intensityBMax = 0;
			_intensityBMin = 0;
		}
	}
}
