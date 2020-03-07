using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateContract : MonoBehaviour
{
    public InputField ContractDescription;
    public event Action<int> OnContractCreatedPlantPlantss;
    public static CreateContract CreateContractInstance;

    void Awake()
    {
        CreateContractInstance = this;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreatingContract(int contractId)
    {
        if (OnContractCreatedPlantPlantss != null)
        {
            int numberGenerator = UnityEngine.Random.Range(100, 289);
            contractId = numberGenerator;
            OnContractCreatedPlantPlantss(contractId);


            Contract contract = new Contract();
            contract.ContractId = numberGenerator;
            contract.ContractNumber = numberGenerator;
            contract.ContractDescription = ContractDescription.text;
            contract.IsContractStarted = true;

            string ToJson = JsonUtility.ToJson(contract);
            reference.Child("USERS").Child(LogInAndRegister.Instance.UserName).Child("GAMESPACE").Child("CONTRACT" + contract.ContractId).SetRawJsonValueAsync(ToJson);
        }
    }




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
    }
}
