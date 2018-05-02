using System.IO;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataRecorder : MonoBehaviour {
	
	public static DataRecorder Instance;

	public enum DataFallback
	{
		A,
		B
	};

	public DataFallback DefaultDataFallback = DataFallback.A;

	private readonly OrderedDictionary _dataSets = new OrderedDictionary();

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

	private void CreateNewDataSet(GameObject go, IDataSet dataSet)
	{
		_dataSets.Add(go, dataSet);
		((IDataSet)_dataSets[go]).GameObjectName = go.name;
	}
	
	public T GetOrCreateDataSet<T>(GameObject go)
	{

		if (_dataSets[go] == null)
		{
			if (typeof(DataSetConditionA) == typeof(T))
			{
				CreateNewDataSet(go, new DataSetConditionA());
			}
			else if (typeof(DataSetConditionB) == typeof(T))
			{
				CreateNewDataSet(go, new DataSetConditionB());
			}
			else
			{
				if (DefaultDataFallback == DataFallback.A)
				{
					CreateNewDataSet(go, new DataSetConditionA());
				}
				else
				{
					CreateNewDataSet(go, new DataSetConditionB());
				}
			}
			
		}
		
		return (T) _dataSets[go];
	}

	public void WriteDataToCsv()
	{

		string filePath = CreateNewCSVFile();
		StreamWriter writer = new StreamWriter (filePath);

		if (_dataSets.Count > 0)
		{
			writer.WriteLine (((IDataSet)_dataSets[0]).GetDataSetHeader());
		}
		
		for (int i = 0; i < _dataSets.Count; i++)
		{
			((IDataSet) _dataSets[i]).FinalizeDataSet();
			writer.WriteLine (((IDataSet)_dataSets[i]).GetDataSetData());
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
		WriteDataToCsv();
	}
	
}