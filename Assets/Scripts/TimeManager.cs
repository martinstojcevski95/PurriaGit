using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public int canCount; //1 = true 0 = false 
    public float TimePassed; // in seconds
    public static TimeManager Instance;
    public int PassedDays;

    void Awake()
    {
        Instance = this;
        TimePassed = PlayerPrefs.GetFloat("TimePassed");
    }

    void Stat()
    {
        canCount = 0;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DATABASEURL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }



    void Update()
    {

        if (canCount == 1)
        {
            TimePassed += Time.deltaTime;

            if (TimePassed > 20) //86400)
            {
                // reset time to 0 and save it
                Debug.Log("day has passed");
                TimePassed = 0;
                canCount = 0;
                PlantsManager.PlantsManagerInstance.Test();
                PlantsManager.PlantsManagerInstance.IncrementGrowthDaysForEachPlansAndContract();
            //    PlantsManager.PlantsManagerInstance.IncrementCycleDayForContract();

            }

        }

    }

    void IncreaseDaysForEachContract()
    {
 
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("TimePassed", TimePassed);
        canCount = 0;
    }


    DatabaseReference reference;
    string DATABASEURL = "https://purriadb.firebaseio.com/";

    [Serializable]
    public class Contract
    {
        public int ContractId;
        public int ContractNumber;
        public string ContractDescription;
        public bool IsContractStarted;
        public int PlantCounterInContract;
        public int DroneCount;
        public string PlantsPlantedTimeForContract;
        public int CycleDays;
    }
}
