﻿using UnityEngine;

public class DataSetConditionB : IDataSet
{

    private string _gameObjectName;
    
    public float TimeRoomEntered;
    public float TimeRoomExited;
    public float TimeSpentInRoom;
    
    private bool _attended;
    private float _timeAttended;
    public bool Collected;
    public float TimeCollected;

    public float TimePlayedTotal;
    
    public void FinalizeDataSet()
    {
        TimePlayedTotal = Time.time;
    }

    public string GetDataSetHeader()
    {
        return
            "GameObjectName," +
            "TimeRoomEntered," +
            "TimeRoomExited," +
            "TimeSpentInRoom," +
            "Attended," +
            "TimeAttended," +
            "Collected," +
            "TimeCollected," +
            "TimePlayedTotal";
    }

    public string GetDataSetData()
    {
        
        return _gameObjectName +
               "," + TimeRoomEntered +
               "," + TimeRoomExited +
               "," + TimeSpentInRoom +
               "," + _attended +
               "," + _timeAttended +
               "," + Collected +
               "," + TimeCollected +
               "," + TimePlayedTotal;
    }

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
}