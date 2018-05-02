
public interface IDataSet
{	
	void FinalizeDataSet();
	string GetDataSetHeader();
	string GetDataSetData();
	
	bool Attended{set;}
	float TimeAttended{set;}
	
	string GameObjectName{set;}
}