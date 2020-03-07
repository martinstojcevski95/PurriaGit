using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneFour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [Serializable]
    public class Drone
    {
        public int DroneID;
        public int ContactID;
        public bool isAssignedToWork;
    }
}
