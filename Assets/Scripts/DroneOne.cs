using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneOne : MonoBehaviour
{
    public Drone drone;
    public int DroneID;
    public Text droneText;
    public int PlantsForPlanting, DroneLevel, ContractDroneID;

    // Start is called before the first frame update
    void Start()
    {
        PlantsForPlanting = 0;
        DroneLevel = 0;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DATABASEURL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetDataForDrone()
    {
        FirebaseDatabase.DefaultInstance
           .GetReference("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + DroneID).Child("DRONE" + DroneID)
           .GetValueAsync().ContinueWith(task =>
           {
               if (task.IsFaulted)
               {
                   Debug.Log(task.IsFaulted);
               }
               else if (task.IsCompleted)
               {
                   DataSnapshot snapshot = task.Result;
                   Debug.Log("DRONE " + snapshot.GetRawJsonValue());
                   drone = JsonUtility.FromJson<Drone>(snapshot.GetRawJsonValue());
                   Debug.Log("");
                   DroneLevel = drone.DroneLevel;
                   PlantsForPlanting = drone.Plants;
                   ContractDroneID = drone.ContactID;
               }


           });
    }

    //public void IncrementGrowthDaysEacHDay(int contractID)
    //{
    //    Debug.Log("contract id " + contractID);
    //    for (int i = 0; i < PlantsManager.PlantsManagerInstance.FirstPlantsForContract.Count; i++)
    //    {
    //        int growthDayIncrementer = PlantsManager.PlantsManagerInstance.FirstPlantsForContract[0].plant.GrowthDays;

    //        Dictionary<string, object> lParameters = new Dictionary<string, object>();

    //        SinglePlant plant = new SinglePlant();

    //        lParameters.Add("GrowthDays", plant.GrowthDays = growthDayIncrementer += 1);
    //        lParameters.Add("isDroneAssigned", plant.isDroneAssigned = true);
    //        //    PlayerPrefs.SetInt("frPlants", plant.GrowthDays);
    //        reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + contractID).Child("PLANTS").Child("PLANT" + i).UpdateChildrenAsync(lParameters);
    //    }

    //}




    // yes no pop up, ako yes neka se stavi ako se stisne ne togas mzoes samo da go dodelis, ke pises id na kontrakt i ne se dodade. za proverk adali veke n atoj kontrakt ima dron ke proveram so if(drone is assigned)
    public void InspectDrone()
    {
        StartCoroutine(GetData());
    }

    public void ResetDroneDataOnDelete()
    {
        DroneLevel = 0;
        PlantsForPlanting = 0;
    }
    IEnumerator GetData()
    {

        FirebaseDatabase.DefaultInstance
      .GetReference("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + DroneID).Child("DRONE" + DroneID)
      .GetValueAsync().ContinueWith(task =>
      {
          if (task.IsFaulted)
          {
              Debug.Log(task.IsFaulted);
          }
          else if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;
              //  Debug.Log("DRONE " + snapshot.GetRawJsonValue());
              drone = JsonUtility.FromJson<Drone>(snapshot.GetRawJsonValue());


          }


      });
        yield return new WaitForSeconds(2f);

        if (drone != null)
        {
            UIManager.Instance.DroneID.text = "Drone ID " + drone.DroneID;
            UIManager.Instance.DroneActivePlants.text = "Active Plants " + drone.Plants;
            UIManager.Instance.DroneContractID.text = "Contract ID " + drone.ContactID;
            //droneText.text = "ID " + drone.DroneID;
        }


    }

    public void UpdateDrone()
    {
       if(DroneLevel <=2) // max lvl 3, each level can tend + 5  plants
        {
            DroneLevel = DroneLevel += 1;
            PlantsForPlanting = PlantsForPlanting += 5;
            AssignDroneToContract(ContractDroneID);
        }
    }

    public void AssignDroneToContract(int contractID)
    {

        Drone Newdrone = new Drone();
        Newdrone.isAssignedToWork = true;
        Newdrone.ContactID = contractID;
        Newdrone.Plants = PlantsForPlanting;
        Newdrone.DroneID = DroneID;
        Newdrone.DroneLevel = DroneLevel;
        string ToJson = JsonUtility.ToJson(Newdrone);
        Debug.Log(ToJson);
        reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" +contractID).Child("DRONE" + Newdrone.DroneID). SetRawJsonValueAsync(ToJson);

        if(contractID == 0)
        {
            for (int i = 0; i < PlantsForPlanting; i++)
            {
                PlantsManager.PlantsManagerInstance.FirstPlantsForContract[i].UpdateAndPlantThePlants(i);
            }
        }
        if (contractID == 1)
        {
            for (int i = 0; i < PlantsForPlanting; i++)
            {
                PlantsManager.PlantsManagerInstance.SecondPlantsForContract[i].UpdateAndPlantThePlants(i);
            }
        }

    }


    // kaj timerot koga ke pomine 24 saata so for za site d aja povikam funkcijata inspect. Pred da ja povikam da vidam prvo dali e assigned ako e top

    //PRIVATE VARIABLES
    DatabaseReference reference;
    string DATABASEURL = "https://purriadb.firebaseio.com/";
    int plantID;



    [Serializable]
    public class Drone
    {
        public int DroneID;
        public int ContactID;
        public bool isAssignedToWork;
        public int Plants;
        public int DroneLevel;
    }
    // on every 24 hours check each plant with for loop in each field that has drone assigned. Get all of the plants stats and check how are they.


}
