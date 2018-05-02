public class DataSetConditionA : IDataSet {

	private string _gameObjectName;
	private bool _attended;
	private float _timeAttended;
	public float TimeAppeared;
	public bool ShotTriggered;
	public float TimeShotTriggered;
	public bool Shot;
	public float TimeShot;
	private float _timeAppearedToAttend;
	private float _timeAppearedToTrigger;
	private float _timeAppearedUntilShot;
	private float _timeAttendedUntilShot;
	private float _timeAttendedUntilTriggered;
	
	public bool Attended
	{
		set { _attended = value; }
	}

	public float TimeAttended
	{
		set { _timeAttended = value; }
	}
	
	public string GameObjectName
	{
		set { _gameObjectName = value; }
	}

	public void FinalizeDataSet()
	{
		_timeAppearedToAttend = _timeAttended - TimeAppeared;
		_timeAppearedToTrigger = TimeShotTriggered - TimeAppeared;
		_timeAppearedUntilShot = TimeShot - TimeAppeared;
		_timeAttendedUntilShot = _timeAppearedToAttend - _timeAppearedUntilShot;
		_timeAttendedUntilTriggered = _timeAppearedToAttend - _timeAppearedToTrigger;
	}

	public string GetDataSetHeader()
	{
		return
			"GameObjectName," +
			"TimeAppeared," +
			"Attended," +
			"TimeAttended," +
			"Shot," +
			"TimeShot," +
			"ShotTriggered," +
			"TimeShotTriggered," +
			"TimeAppearedToAttend," +
			"TimeAppearedToTrigger," +
			"TimeAppearedUntilShot," +
			"TimeAttendedUntilShot," +
			"TimeAttendedUntilTriggered";
	}

	public string GetDataSetData()
	{
		return
			_gameObjectName +
			"," + TimeAppeared +
			"," + _attended +
			"," + _timeAttended +
		    "," + Shot +
		    "," + TimeShot +
			"," + ShotTriggered +
			"," + TimeShotTriggered +
			"," + _timeAppearedToAttend +
			"," + _timeAppearedToTrigger +
			"," + _timeAppearedUntilShot +
			"," + _timeAttendedUntilShot +
			"," + _timeAttendedUntilTriggered;
	}

}
