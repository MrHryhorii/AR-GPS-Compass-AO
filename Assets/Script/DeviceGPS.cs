using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Linq;

using GpsToUnity;

public class DeviceGPS : MonoBehaviour
{
    public static DeviceGPS instance { get; private set; } // As a Singleton

    public double latitude = 46.6362163f;
    public double longitude = 32.6183742f;
    public double altitude = 19.62f;
    double lastLatitude;
    double lastLongitude;
    double lastAltitude;

    bool isGpsReady = false;

    [HideInInspector]public List<double>  latList;
    [HideInInspector]public List<double>  lonList;
    [HideInInspector]public List<double>  altList;

    public bool Debug = false;
    [HideInInspector]public bool createAR = false;
    [HideInInspector]public Vector3d rawUnityPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator Start()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            print("You need to activate geolocation");
            yield break;
        }

        Input.compass.enabled = true; // enable compass

        // Start service before querying location
        Input.location.Start(0.1f,0.1f);

        // Wait until service initializes
        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 10 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            isGpsReady = true;
        }
            if(isGpsReady)
            {
                latitude = (double)Input.location.lastData.latitude;
                longitude = (double)Input.location.lastData.longitude;
                altitude = (double)Input.location.lastData.altitude;

                rawUnityPosition = GetPos(latitude, longitude, altitude);
                createAR = true;

                InvokeRepeating("UpdateGPS", 1.0f, 0.2f);
            }
        
        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }

    void UpdateGPS()
    {              
            if (Input.location.status == LocationServiceStatus.Running)
            {
                lastLatitude = (double)Input.location.lastData.latitude;
                lastLongitude = (double)Input.location.lastData.longitude;
                //lastAltitude = (double)Input.location.lastData.altitude;
                lastAltitude = 19.0f;
            

                latList.Add(lastLatitude);
                lonList.Add(lastLongitude);
                altList.Add(lastAltitude);
                
                if(latList.Count.Equals(11)) latList.RemoveAt(0);
                if(lonList.Count.Equals(11)) lonList.RemoveAt(0);
                if(altList.Count.Equals(11)) altList.RemoveAt(0);

                latitude = Average(latList);
                longitude = Average(lonList);
                altitude = Average(altList);

                latitude = lastLatitude;
                longitude = lastLongitude;
                altitude = lastAltitude;
            }

            rawUnityPosition = GetPos(latitude, longitude, altitude);
    }

    double Average(List<double> arr)
    {
        double sum = 0d;
        double average = 0d;

        for( var i = 0; i < arr.Count; i++) 
        {
            sum += arr[i];
        }
        average = sum / arr.Count;

        return average;
    }

    void Update()
    {
        if(Debug)
        {
            rawUnityPosition = GetPos(latitude, longitude, altitude);
            createAR = true;
        }
    }

    private Vector3d GetPos(double lat, double lon, double alt)
    {
        List<double> dp = GetPOS.GTU(lat, lon, alt);
        
        return new Vector3d(dp[0],dp[1],dp[2]);
    }
}