using System.Collections;
using System.IO;
using UnityEngine;

public class DataRecorder : MonoBehaviour {
	
	public static DataRecorder Instance;
	
	private ArrayList _dataSets = new ArrayList();
	private static int _counter = -1;

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

	public void CreateNewDataSet()
	{
		_counter++;
		DataSet dataSet = new DataSet();
		_dataSets.Add(dataSet);
	}
	
	public DataSet GetCurrentDataSet()
	{
		return (DataSet) _dataSets[_counter];
	}

	public void FinalizeCurrentDataSet()
	{
		DataSet dataSet = (DataSet) _dataSets[_counter];
		dataSet.name = "Enemy " + _counter;
		dataSet.TimeAppearedToAttend = dataSet.TimeAttended - dataSet.TimeAppeared;
		dataSet.TimeAppearedToTrigger = dataSet.TimeShotTriggered - dataSet.TimeAppeared;
		dataSet.TimeAppearedUntilShot = dataSet.TimeShot - dataSet.TimeAppeared;
		dataSet.TimeAttendedUntilShot = dataSet.TimeAppearedToAttend - dataSet.TimeAppearedUntilShot;
		dataSet.TimeAttendedUntilTriggered = dataSet.TimeAppearedToAttend - dataSet.TimeAppearedToTrigger;
	}

	public void WriteDataToCsv()
	{
		
		string filePath = getPath ();
		StreamWriter writer = new StreamWriter (filePath);
		
		writer.WriteLine ("Name,TimeAppeared,Attended,TimeAttended,ShotTriggered,TimeShotTriggered,Shot,TimeShot,TimeAppearedToAttend,TimeAppearedToTrigger,TimeAppearedUntilShot,TimeAttendedUntilShot,TimeAttendedUntilTriggered");

		for (int i = 0; i < _dataSets.Count; i++)
		{
			DataSet dataSet = (DataSet) _dataSets[i];
			
			writer.WriteLine (dataSet.name + 
			                  "," + dataSet.TimeAppeared +
			                  "," + dataSet.Attended +
			                  "," + dataSet.TimeAttended +
			                  "," + dataSet.ShotTriggered +
			                  "," + dataSet.TimeShotTriggered +
			                  "," + dataSet.Shot +
			                  "," + dataSet.TimeShot +
			                  "," + dataSet.TimeAppearedToAttend +
			                  "," + dataSet.TimeAppearedToTrigger +
			                  "," + dataSet.TimeAppearedUntilShot +
			                  "," + dataSet.TimeAttendedUntilShot +
			                  "," + dataSet.TimeAttendedUntilTriggered);
		}
		
		writer.Flush ();
		writer.Close();
	}
	
	private string getPath () {
		
		Debug.Log(Application.dataPath + "/CSV/" + "SaveData.csv");
		
		#if UNITY_EDITOR
		return Application.dataPath + "/CSV/" + "SaveData.csv";
        #else
        return Application.dataPath +"/"+"Saved_Inventory.csv";
		#endif
	}
	
	private void OnApplicationQuit()
	{
		DataRecorder.Instance.WriteDataToCsv();
	}
	
}
