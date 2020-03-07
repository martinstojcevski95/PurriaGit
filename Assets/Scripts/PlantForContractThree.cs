using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantForContractThree : MonoBehaviour
{
    public SinglePlantThree plant;
    public int id;
    public int ContractID;

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
                     //Debug.Log("data for plant " + snapshot.GetRawJsonValue());
                     plant = JsonUtility.FromJson<SinglePlantThree>(snapshot.GetRawJsonValue());

                 }
             });
    }
    public void SetEachPlantDataInDbAfterCreatingContract()
    {
        plant = new SinglePlantThree();
        plant.ContractID = ContractID;
        plant.isPlantInContract = true;
        plant.isPlantPlanted = true;
        plant.isDroneAssigned = false;
        plant.FieldID = id;

        string ToJson = JsonUtility.ToJson(plant);
        reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + plant.ContractID).Child("PLANTS").Child("PLANT" + plant.FieldID).SetRawJsonValueAsync(ToJson);
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
    public class SinglePlantThree
    {
        public bool isDroneAssigned;
        public bool isPlantPlanted;
        public bool isPlantInContract;
        public int FieldID;
        public int ContractID;
        public int GrowthDays;
    }
}
