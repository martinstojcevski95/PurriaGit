using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantForContractTwo : MonoBehaviour
{
    public SinglePlantTwo plant;
    public int id;
    public int ContractID;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DATABASEURL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // BEZ OBIEKTI DA PROBMSA D AZAPISAM WTFFF
    IEnumerator PlantingThePlant(int contract)
    {
        ContractID = contract;
        yield return new WaitForSeconds(1f);
        plant = new SinglePlantTwo();
        plant.ContractID = ContractID;
        plant.isPlantInContract = true;
        plant.isPlantPlanted = true;
        plant.isDroneAssigned = false;
        plant.FieldID = id;
        //ContractID = contract;
       // PlayerPrefs.SetInt("contract", ContractID);

        string ToJson = JsonUtility.ToJson(plant);
        reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + ContractID).Child("PLANTS").Child("PLANT" + plant.FieldID).SetRawJsonValueAsync(ToJson);

      //  PlayerPrefs.SetInt("plantid", id);
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
                     plant = JsonUtility.FromJson<SinglePlantTwo>(snapshot.GetRawJsonValue());
                     // StartCoroutine(GetTheDataFromTheDbAndPopulateThePlant(snapshot.GetRawJsonValue()));

                 }
             });
    }
    public void SetEachPlantDataInDbAfterCreatingContract()
    {
        plant = new SinglePlantTwo();
        plant.ContractID = ContractID;
        plant.isPlantInContract = true;
        plant.isPlantPlanted = false;//true;
        plant.isDroneAssigned = false;
        plant.FieldID = id;
        //  PlayerPrefs.SetInt("contract", ContractID);

        string ToJson = JsonUtility.ToJson(plant);
        reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + plant.ContractID).Child("PLANTS").Child("PLANT" + plant.FieldID).SetRawJsonValueAsync(ToJson);
    }

    public void UpdateAndPlantThePlants(int i)
    {

        Dictionary<string, object> lParameters = new Dictionary<string, object>();

        SinglePlantTwo plant = new SinglePlantTwo();

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
    public class SinglePlantTwo
    {
        public bool isDroneAssigned;
        public bool isPlantPlanted;
        public bool isPlantInContract;
        public int FieldID;
        public int ContractID;
        public int GrowthDays;
    }
}
