using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour
{
    // Start is called before the first frame update
    public SinglePlant plant;
    public int id;
    public int ContractID;


    void Start()
    {

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DATABASEURL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        //OVA DA SE SLUCUVA KOG AKE STISNES INSPECT NA KONTRAKTOT TOGAS DA SE EME ZA SITE 15 ZA TOJ KONTRAKT  GetDBDataForPlant();
    }


    void OnEnable()
    {


    }
    void OnDisable()
    {
       // ContractManager.ContractManagerInstance.OnContractCreatedPlantPlants -= OnContractCreated;
    }
    //public void AsignEventOnContractCreation()
    //{
    //    if (!plant.isPlantInContract)
    //    {
    //        ContractManager.ContractManagerInstance.OnContractCreatedPlantPlants += OnContractCreated;
    //        Debug.Log("ke se staba vo kontrakt");
    //    }
    //    else
    //    {
    //        ContractManager.ContractManagerInstance.OnContractCreatedPlantPlants -= OnContractCreated;
    //        Debug.Log("vo kontrakt e ");
    //    }

    //}
    //ISTO VAKA I CONTRACT DA ZEMAM

    // tuka da predadam so parametar koga ke stisnam na inspect contract za koj kontrakt id da zeme data od plants


    public void GetDBDataForPlant()
    {
        FirebaseDatabase.DefaultInstance
             .GetReference("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT"+ContractID).Child("PLANTS").Child("PLANT" + id)
             .GetValueAsync().ContinueWith(task =>
             {
                 if (task.IsFaulted)
                 {
                      // Handle the error...
                  }
                 else if (task.IsCompleted)
                 {
                     DataSnapshot snapshot = task.Result;
                     plant = JsonUtility.FromJson<SinglePlant>(snapshot.GetRawJsonValue());
                     Debug.Log("data for plant  for contract id " +plant.ContractID);
                     // StartCoroutine(GetTheDataFromTheDbAndPopulateThePlant(snapshot.GetRawJsonValue()));

                 }
             });
    }


    public void SetEachPlantDataInDbAfterCreatingContract()
    {
        plant = new SinglePlant();
        plant.ContractID = ContractID;
        plant.isPlantInContract = true;
        plant.isPlantPlanted = true;
        plant.isDroneAssigned = false;
        plant.FieldID = id;
        ContractID = 0;

        string ToJson = JsonUtility.ToJson(plant);
        reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT"  + ContractID).Child("PLANTS").Child("PLANT" + plant.FieldID).SetRawJsonValueAsync(ToJson);
    }

    //// BEZ OBIEKTI DA PROBMSA D AZAPISAM WTFFF
    //IEnumerator PlantingThePlant(int contract)
    //{

    //    yield return new WaitForSeconds(1f);
    //    plant = new SinglePlant();
    //    plant.ContractID = ContractID;
    //    plant.isPlantInContract = true;
    //    plant.isPlantPlanted = true;
    //    plant.isDroneAssigned = false;
    //    plant.FieldID = id;
    //    ContractID = contract;
    //  //  PlayerPrefs.SetInt("contract", ContractID);

    //    string ToJson = JsonUtility.ToJson(plant);
    //    reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + ContractID).Child("PLANTS").Child("PLANT" + plant.FieldID).SetRawJsonValueAsync(ToJson);

    // //   PlayerPrefs.SetInt("plantid", id);
    //}

    //private void OnContractCreated(int ContractID)
    //{
    //    StartCoroutine(PlantingThePlant(ContractID));
    //}

    // Update is called once per frame
    void Update()
    {
 
    }



    //PRIVATE VARIABLES
    DatabaseReference reference;
    string DATABASEURL = "https://purriadb.firebaseio.com/";
    int plantID;

    [Serializable]
    public class SinglePlant
    {
        public bool isDroneAssigned;
        public bool isPlantPlanted;
        public bool isPlantInContract;
        public int FieldID;
        public int ContractID;
        public int GrowthDays;
    }
}
