using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UnityEngine;
using UnityEngine.UI;

public class ContractManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static ContractManager ContractManagerInstance;
    public event Action<int> OnContractCreatedPlantPlants;
    public List<Plant> Plants = new List<Plant>();
    public InputField ContractDescription;

    public List<ContractInfo> ContractIDUIInfo = new List<ContractInfo>();
    public List<ContractInfo> StartedContracts = new List<ContractInfo>();
    public int contractIDiNcrementor;

    public List<string> ContractIDS = new List<string>();
    public Text PopUpText;
    public GameObject PopUp;
    ContractIDNumber StaticContractID;
    public int MaxPlantNumber;
    int plantsCount;
    int dronesCount;

    void Awake()
    {
        ContractManagerInstance = this;

    }





    //ZA PLANTED PLANTS KE STORIRAM EDN INTEGER SAMO I NA SEKOJ PLANTING KE KAZUVMA KOLKU E I KE GO CUVMA VO PLAYE RPREFS. TAKA KE MOZOAM NA DASHBOARDSDA ZNAM KOLKU VKUPNO IMA
    // ili get active plants momentalno koj se aktivni vo scena



    // d aprobam isto vaka so event d aslusat 15 rastenija za sekoj kontract  posebna lista na 15 rastenija za da znam koj se na kade dodeleni. poso ke ima event da go instanciram cel
    // prefat so 15 rastenija i samo togas  ke slusaat na event samo tie instancirani drugi aktivni sto slusaat ke nema.
    // za posle toa da ne slusaat event ke napravam unscribe i ke provram dali veke tie se vo contract, ako se neka ne slusaat ako ne se neka slusaats
    // od koga ke izbrisma contract prvo go brisam pa po nekoja sekunda disabble pravam na parentot na rastenijata 


    // POP UP ZA SEKOJ PRV KONTRAKT STOE KREIRAN I ZA SEKO IZBRISAN, A NAREDNO LOGIRANJE KE SI GI ZIMA OD NA CLICK NA CONTRACT KOPCETO KE ZIMA DATA ZA SITE KREIRANI


    void Start()
    {

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DATABASEURL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        MaxPlantNumber = 15;
        contractIDiNcrementor = -1;

        //InvokeRepeating("GetDataOnEveryXSeconds", 5f, 15f);
        //   Invoke("CreateContractAndActivateParentForPlants", 3f);

   
    }


    public void CreateContract()
    {
        int numberGenerator = UnityEngine.Random.Range(100, 289);
        //   ContractID = numberGenerator;
        //    OnContractCreatedPlantPlants(ContractID);


        Contract contract = new Contract();
        contract.ContractId = numberGenerator;
        contract.ContractNumber = numberGenerator;
        contract.ContractDescription = ContractDescription.text;
        contract.IsContractStarted = true;

        string ToJson = JsonUtility.ToJson(contract);
        reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + contract.ContractId).SetRawJsonValueAsync(ToJson);

        // PlantsManager.PlantsManagerInstance.CallPlantsPlanting();
    }



    public void ContractCreatedPlantPlants()
    {
    // check if contract already exists before creating it twice //rewriting data

        Contract contract = new Contract();
        contract.PlantsPlantedTimeForContract =  DateTime.Now.ToShortTimeString();
    //    contract.PlantsPlantedTimeForContract = string.Format(" {0}-{1}-{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        contract.ContractId = int.Parse(ContractDescription.text);
        contract.ContractNumber = int.Parse(ContractDescription.text);
        contract.ContractDescription = ContractDescription.text;
        contract.IsContractStarted = true;
        contract.DroneCount = 1;
        contract.PlantCounterInContract = 15;
        Debug.Log(contract.PlantsPlantedTimeForContract);
        string ToJson = JsonUtility.ToJson(contract);
        reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + contract.ContractId).SetRawJsonValueAsync(ToJson);


        Debug.Log(" contract id is  " + int.Parse(ContractDescription.text));
        PlantsManager.PlantsManagerInstance.CallPlantsPlanting(int.Parse(ContractDescription.text));
        ContractData();
        DronesManager.Instance.Drones[int.Parse(ContractDescription.text)].AssignDroneToContract(int.Parse(ContractDescription.text));
        TimeManager.Instance.canCount = 1;
    }

    public void ContractData()
    {
        StartCoroutine(GetData());
    }

    public IEnumerator GetData()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < ContractIDUIInfo.Count; i++)
        {
            UIManager.Instance.ActiveContracts.text = ContractIDUIInfo.Count.ToString();
            ContractIDUIInfo[i].GetDataForContract();
        }
        PlantsManager.PlantsManagerInstance.Test();
    }

 

    public void DeleteContract(InputField contractIDForDeleteing)
    {
        int id = int.Parse(contractIDForDeleteing.text);
        reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + id).RemoveValueAsync();
        ContractData();
        DronesManager.Instance.Drones[id].ResetDroneDataOnDelete();
        PlantsManager.PlantsManagerInstance.ActiveAndDoneContractsUI();
        // StartCoroutine(GetData());
    }

    void PopulateContractIDForUi(int CONTRACTID)
    {
        if (contractIDiNcrementor <= 4)
        {
            contractIDiNcrementor = contractIDiNcrementor += 1;
            //  ContractIDUIInfo[contractIDiNcrementor].ContractID = CONTRACTID;
            ContractIDUIInfo[contractIDiNcrementor].SaveTheContractID(CONTRACTID);

        }

    }

    public void GetCurrentPassedTimeOnClick()
    {
        for (int i = 0; i < ContractIDUIInfo.Count; i++)
        {
            ContractIDUIInfo[i].GetPassedData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(StartedContracts.Count >=0)
        {
            UIManager.Instance.ActiveContracts.text = StartedContracts.Count.ToString();
            UIManager.Instance.PlantedPlants.text = plantsCount.ToString();
            UIManager.Instance.AvailableDrones.text = dronesCount.ToString();
        }
    }


    public void ActiveContractsDashBoard()
    {
        StartCoroutine(SearchforStartedContracts());  
    }

    IEnumerator SearchforStartedContracts()
    {
        StartedContracts.Clear();

        plantsCount =0;
        dronesCount= 0;
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < ContractIDUIInfo.Count; i++)
        {
            if (ContractIDUIInfo[i].FullContract != null)
            {
                if (ContractIDUIInfo[i].FullContract.IsContractStarted == true)
                {
                    StartedContracts.Add(ContractIDUIInfo[i]);

                    //plantsCount += StartedContracts[i].FullContract.PlantCounterInContract;
                    //dronesCount += StartedContracts[i].FullContract.DroneCount;
                }
            }
        }
        for (int i = 0; i < StartedContracts.Count; i++)
        {

            plantsCount += StartedContracts[i].FullContract.PlantCounterInContract;
            dronesCount += StartedContracts[i].FullContract.DroneCount;
        }
    }





    //    public void GetDataForContractsAfterLogIn()
    //{
    //    StartCoroutine(GetDelayedData());
    //}
    //public void ReloadDataAfterFakeSignOut()
    //{
    //    StartCoroutine(AassignContractIDFromDBOnContractCreation());
    //}


    //IEnumerator GetDelayedData()
    //{
    //    yield return new WaitForSeconds(1f);
    //    GetFullDataForAllContracts();
    //    StartCoroutine(AassignContractIDFromDBOnContractCreation());
    //    yield return new WaitForSeconds(3f);
    //}
    //void GetDataOnEveryXSeconds()
    //{
    //    GetFullDataForAllContracts();
    //    StartCoroutine(AassignContractIDFromDBOnContractCreation());
    //}
    //public IEnumerator PopUpOnCreatingContract(int contractID)
    //{

    //    PopUp.SetActive(true);
    //    PopUpText.text = "Creating contract ...";
    //    yield return new WaitForSeconds(1f);
    //    GetFullDataForAllContracts();
    //    StartCoroutine(AassignContractIDFromDBOnContractCreation());
    //    yield return new WaitForSeconds(3f);
    //    PopUpText.text = "Created contract " + contractID;
    //}


    public void ClosePopUp()
    {
        PopUp.SetActive(false);
    }



    public void GetFullDataForAllContracts()
    {
        FirebaseDatabase.DefaultInstance
             .GetReference("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE")
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
                     string[] x = snapshot.GetRawJsonValue().Split('"');
                     for (int i = 0; i < x.Length; i++)
                     {
                       //  Debug.Log("splited string " + x[i]);
                         if (x[i].Contains("CONTRACT"))
                         {
                          //   Debug.Log("contract id " + x[i]);
                             ContractIDS.Add(x[i]);


                         }

                     }

                     //plant = JsonUtility.FromJson<SinglePlant>(snapshot.GetRawJsonValue());
                     //StartCoroutine(GetTheDataFromTheDbAndPopulateThePlant(snapshot.GetRawJsonValue()));

                 }


             });

        //   StartCoroutine(GetData());
    }

    //IEnumerator AassignContractIDFromDBOnContractCreation()
    //{
    //    yield return new WaitForSeconds(1f);
    //    AssignContractIDToEachContract();
    //    StartCoroutine(GetData());
    //}



    //public void AssignContractIDToEachContract()
    //{

    //    for (int i = 0; i < ContractIDS.Count; i++)
    //    {
    //        ContractIDUIInfo[i].FullContract.ContractId = ContractIDS[i];
    //        ContractIDUIInfo[i].FullContract.ContractId = ContractIDUIInfo[i].FullContract.ContractId.Substring(ContractIDUIInfo.Count + 2);
    //    }
    //    ContractIDS.Clear();
    //}


    //PRIVATE VARIABLES
    int ContractUniqueNumber;
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
    [Serializable]
    public class ContractIDNumber
    {
        public int ContractStaticID;
    }

}
