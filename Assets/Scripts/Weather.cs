using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Weather : MonoBehaviour
{
    public AllWeatherStats allWeatherStats;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetWeatherData()
    {
        StartCoroutine(GetRequest("https://api.openweathermap.org/data/2.5/weather?q=London,uk&APPID=511efb82bba4378564f71de9d47f6fae"));
    }


    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            string json = uwr.downloadHandler.text;
            allWeatherStats = JsonUtility.FromJson<AllWeatherStats>(json);
            UIManager.Instance.WeatherInfo.text = "The weathercast for today is : " + allWeatherStats.weather[0].main + " with " + allWeatherStats.weather[0].description + "if you use this feature you won't need to add more water to the plants for this day";
        }
    }



    [Serializable]
    public class Coord
    {
        public double lon;
        public double lat;
    }
    [Serializable]
    public class Weather_
    {
        public int id;
        public string main;
        public string description;
        public string icon;
    }
    [Serializable]
    public class Main
    {
        public double temp;
        public int pressure;
        public int humidity;
        public double temp_min;
        public double temp_max;
    }
    [Serializable]
    public class Wind
    {
        public double speed;
        public int deg;
    }
    [Serializable]
    public class Rain
    {
        public double __invalid_name__1h;
    }
    [Serializable]
    public class Clouds
    {
        public int all;
    }
    [Serializable]
    public class Sys
    {
        public int type;
        public int id;
        public double message;
        public string country;
        public int sunrise;
        public int sunset;
    }
    [Serializable]
    public class AllWeatherStats
    {
        public Coord coord;
        public List<Weather_> weather;
        public string @base;
        public Main main;
        public int visibility;
        public Wind wind;
        public Rain rain;
        public Clouds clouds;
        public int dt;
        public Sys sys;
        public int timezone;
        public int id;
        public string name;
        public int cod;
    }
}
