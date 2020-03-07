using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantForContractOne : MonoBehaviour
{
    public SinglePlantOne plant;
    public int id;
    public int ContractID;
    Contract contract;
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DATABASEURL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }



    public void GetDBDataForPlant()
    {
        FirebaseDatabase.DefaultInstance
             .GetReference("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + ContractID).Child("PLANTS").Child("PLANT" + id)
             .GetValueAsync().ContinueWith(task =>
             {
                 if (task.IsFaulted)
                 {
                     // Handle the error...
                 }
                 else if (task.IsCompleted)
                 {
                     DataSnapshot snapshot = task.Result;
                     //   Debug.Log("data for plant " + snapshot.GetRawJsonValue());

                     plant = JsonUtility.FromJson<SinglePlantOne>(snapshot.GetRawJsonValue());

                 }
             });
    }
    public void SetEachPlantDataInDbAfterCreatingContract()
    {
        plant = new SinglePlantOne();
        plant.ContractID = ContractID;
        plant.isPlantInContract = true;
        plant.isPlantPlanted = false;// true;
        plant.isDroneAssigned = false;
        plant.FieldID = id;

        string ToJson = JsonUtility.ToJson(plant);
        reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + plant.ContractID).Child("PLANTS").Child("PLANT" + plant.FieldID).SetRawJsonValueAsync(ToJson);


        //Dictionary<string, object> lParameters = new Dictionary<string, object>();
        //Contract contract = new Contract();

        //lParameters.Add("PlantCounterInContract", contract.PlantCounterInContract = contract.PlantCounterInContract += 1);
        //reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + ContractID).UpdateChildrenAsync(lParameters);
    }

    public void UpdateAndPlantThePlants(int i)
    {

        Dictionary<string, object> lParameters = new Dictionary<string, object>();

        SinglePlantOne plant = new SinglePlantOne();

        lParameters.Add("isPlantPlanted", plant.isPlantPlanted = true);
        reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + ContractID).Child("PLANTS").Child("PLANT" + i).UpdateChildrenAsync(lParameters);
    }


    // Update is called once per frame
    void Update()
    {

    }

    //PRIVATE VARIABLES
    DatabaseReference reference;
    string DATABASEURL = "https://purriadb.firebaseio.com/";
    int plantID;

    [Serializable]
    public class SinglePlantOne
    {
        public bool isDroneAssigned;
        public bool isPlantPlanted;
        public bool isPlantInContract;
        public int FieldID;
        public int ContractID;
        public int GrowthDays;
    }

    [Serializable]
    public class Contract
    {
        public int ContractId;
        public int ContractNumber;
        public string ContractDescription;
        public bool IsContractStarted;
        public int PlantCounterInContract;
    }
}
