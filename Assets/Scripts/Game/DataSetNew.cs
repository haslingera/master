using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSetNew
{
	public float Time;
	public string Name;
	public int Id;
	public string Action;

	public DataSetNew(float time, string name, int id, DataRecorderNew.Action action)
	{
		Time = time;
		Name = name;
		Id = id;
		Action = action.ToString();
	}
	
	public string GetDataSetHeader()
	{
		return "Time,Name,Id,Action";
	}

	public string GetDataSetData()
	{
		return Time + "," + Name + "," + Id + "," + Action;
	}
	
}
