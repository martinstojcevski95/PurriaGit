using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronesManager : MonoBehaviour
{
    public List<DroneOne> Drones = new List<DroneOne>();
    public List<DroneOne> AvailableDrones = new List<DroneOne>();
    public static DronesManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < Drones.Count; i++)
        {
            Drones[i].DroneID = i;

        }
    }


    public void CheckDronesAvaliability()
    {
        for (int i = 0; i < Drones.Count; i++)
        {
            if (Drones[i].drone != null)
            {
                if (Drones[i].drone.isAssignedToWork == false)
                    AvailableDrones.Add(Drones[i]);
            }

        }

    }

    public void RefreshDataForAllDrones()
    {
        for (int i = 0; i < Drones.Count; i++)
        {
            Drones[i].GetDataForDrone();
        }
    }

}
