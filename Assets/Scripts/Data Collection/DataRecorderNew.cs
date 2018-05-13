using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataRecorderNew : MonoBehaviour {

	public static DataRecorderNew Instance;

	public bool CollectData = true;

	public enum Action
	{
		Appeared,
		MarkedAsPOI,
		UnmarkedAsPOI,
		Attended,
		Unattended,
		Shot,
		ShotTriggered,
		
		RoomEnter,
		RoomExit,
		Collected,
		GameExited,
		GameStarted,
		ModulationIntensity,
		GameEnded
	};
	
	private List<DataSetNew> _dataSets = new List<DataSetNew>();

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		} else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void AddNewDataSet(float time, GameObject go, Action action)
	{
		_dataSets.Add(new DataSetNew(time, go.name, go.GetInstanceID(), action));
	}

	public void WriteDataToCsv()
	{
		
		List<DataSetNew> sortedList = _dataSets.OrderBy(o=>o.Time).ToList();

		string filePath = CreateNewCSVFile();
		StreamWriter writer = new StreamWriter (filePath);

		if (sortedList.Count > 0)
		{
			writer.WriteLine (sortedList[0].GetDataSetHeader());
		}
		
		for (int i = 0; i < sortedList.Count; i++)
		{
			writer.WriteLine (sortedList[i].GetDataSetData());
		}
		
		writer.Flush ();
		writer.Close();
	}

	private string CreateNewCSVFile()
	{
		
		if(!Directory.Exists(Application.dataPath + "/CSV/"))
		{
			Directory.CreateDirectory(Application.dataPath + "/CSV/");
		}
		
		int counter = 1;
		
		while (File.Exists(Application.dataPath + "/CSV/" + SceneManager.GetActiveScene().name + "Data" + counter + ".csv"))
		{
			counter++;
		}
				
		File.Create(Application.dataPath + "/CSV/" + SceneManager.GetActiveScene().name + "Data" + counter + ".csv").Close();
		return Application.dataPath + "/CSV/" + SceneManager.GetActiveScene().name + "Data" + counter + ".csv";

	}
	
	private void OnApplicationQuit()
	{
		if (CollectData)
		{
			AddNewDataSet(Time.time, gameObject, Action.GameExited);
			WriteDataToCsv();
		}
	}
	
}
