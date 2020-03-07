using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public string ContractID;
    public Text contractIDText;
    public Text ContractDrones;
    public Text PlantedTime, PassedTime;
    public static ContractInfo Instance;
    public Contract FullContract;
    public int StaticContractID;
    public Text ActivePlants, ActiveDrones, Contractid;
    public DateTime PassedTimeSincePlanting;
    DateTime newDate;
    TimeSpan difference;
    void Awake()
    {
        Instance = this;
    }




    //public void GetDBDataForPlant()
    //{
    //    FirebaseDatabase.DefaultInstance
    //         .GetReference("USERS").Child("Martin").Child("GAMESPACE").Child("CONTRACT" + ContractID).Child("PLANTS").Child("PLANT" + id)
    //         .GetValueAsync().ContinueWith(task =>
    //         {
    //             if (task.IsFaulted)
    //             {
    //                 // Handle the error...
    //             }
    //             else if (task.IsCompleted)
    //             {
    //                 DataSnapshot snapshot = task.Result;
    //                 Debug.Log("data for plant " + snapshot.GetRawJsonValue());
    //                 plant = JsonUtility.FromJson<SinglePlant>(snapshot.GetRawJsonValue());
    //                 // StartCoroutine(GetTheDataFromTheDbAndPopulateThePlant(snapshot.GetRawJsonValue()));

    //             }
    //         });
    //}
    void Start()
    {
        // ContractID = PlayerPrefs.GetInt("contractidUI");

    }
    public void SaveTheContractID(int contractid)
    {
        //  ContractID = contractid;
        // PlayerPrefs.SetInt("contractidUI", ContractID);

    }


    public void GetDataForContract()
    {
        FirebaseDatabase.DefaultInstance
             .GetReference("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + StaticContractID)
             .GetValueAsync().ContinueWith(task =>
             {
                 if (task.IsFaulted)
                 {
                     Debug.Log(task.IsFaulted);
                 }
                 else if (task.IsCompleted)
                 {
                     DataSnapshot snapshot = task.Result;
                     //  Debug.Log("data for contracts " + snapshot.GetRawJsonValue());

                     FullContract = JsonUtility.FromJson<Contract>(snapshot.GetRawJsonValue());

                     var plantedTime = DateTime.Parse(FullContract.PlantsPlantedTimeForContract);



                 }

             });

    }

    public void GetPassedData()
    {
        if (FullContract.IsContractStarted)
        {
            newDate = Convert.ToDateTime(FullContract.PlantsPlantedTimeForContract);
            Debug.Log(string.Format("{0}{1}{2}", newDate.Hour, newDate.Minute, newDate.Second));
            difference = newDate - DateTime.Now;
            Debug.Log(string.Format("Passed Time is -  Hours {0} Minutes {1} Seconds {2}", difference.Hours, difference.Minutes, difference.Seconds));
        }
        else
        {
            Debug.Log("no active contract");
        }

    }


    void Update()
    {
        if (FullContract != null)
        {
            if (FullContract.IsContractStarted)
            {
                contractIDText.text = "contract id " + FullContract.ContractId;
                gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                gameObject.GetComponent<Button>().interactable = false;
                contractIDText.text = "no cotract ";
            }
        }

    }


    public void DisplayDataAboutContract(ContractInfo contractInfo)
    {
        GetPassedData();
        if (contractInfo.FullContract != null)
        {
            ActivePlants.text = "Active Plants " + contractInfo.FullContract.PlantCounterInContract.ToString();
            ActiveDrones.text = "Active drones 0";
            Contractid.text = "ContractID " + contractInfo.FullContract.ContractId.ToString();
            ContractDrones.text = "Active Drones " + contractInfo.FullContract.DroneCount.ToString();
            PlantedTime.text = "PlantedTime " + contractInfo.FullContract.PlantsPlantedTimeForContract;
            PassedTime.text = "PassedTime " + (string.Format(" Hours {0} Minutes {1} Seconds {2}", difference.Hours, difference.Minutes, difference.Seconds));
        }

    }

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {

    }


    //PRIVATE VARIABLES 
    DatabaseReference reference;
    string DATABASEURL = "https://purriadb.firebaseio.com/";


    //CLASSES
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
    }
}
