using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using GpsToUnity;

public class ARControl : MonoBehaviour
{
    public static ARControl instance { get; set; } // As a Singleton
    [HideInInspector]public List<GameObject> shed;
    [HideInInspector]public List<Vector3> shedPos;
    [HideInInspector]public bool listReady = false;

    int listCount;
    [HideInInspector]public List<Vector3d> LatLonAlt;
    [HideInInspector]Vector3d rawWorldCenter;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(listReady)
        {
            if(shed.Count != 0)
            {
                rawWorldCenter = DeviceGPS.instance.rawUnityPosition;

                listCount = shed.Count;
                InvokeRepeating("CalcPos", 1.0f, 1.0f); // Get position each seconds
            }
            listReady = false;
        } 
       
        if(shedPos.Count == shed.Count)       
        {
            for(int i = 0; i < listCount; i++)
            {
                shed[i].transform.position = Vector3.Lerp(shed[i].transform.position, shedPos[i], 10f * Time.deltaTime);
            }
        }
    }

    void CalcPos()
    {
        for(int i = 0; i < listCount; i++)
        {
            Vector3d rawPos = GetPos(LatLonAlt[i].x, LatLonAlt[i].y, LatLonAlt[i].z);
            Vector3d pos = new Vector3d(rawPos.x - rawWorldCenter.x, rawPos.y - rawWorldCenter.y, rawPos.z - rawWorldCenter.z);
            shedPos.Add((Vector3)pos);
            //shedPos[i].position = pos;
        }    
    }

    private Vector3d GetPos(double lat, double lon, double alt)
    {
        List<double> dp = GetPOS.GTU(lat, lon, alt);
        
        return new Vector3d(dp[0],dp[1],dp[2]);
    }
}
