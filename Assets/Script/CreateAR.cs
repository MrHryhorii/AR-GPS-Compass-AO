using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using GpsToUnity;

public class CreateAR : MonoBehaviour
{
    Vector3d rawWorldCenter;

    public List<GameObject> AR;
    public List<Vector3d> LatLonAlt;

    bool update = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(update)
        {
            if(DeviceGPS.instance.Debug)
            {
                if(DeviceGPS.instance.createAR)
                {
                    rawWorldCenter = DeviceGPS.instance.rawUnityPosition;

                    ArCreater();
                    update = false;
                }
            }
            else
            {
                if(DeviceGPS.instance.createAR)
                {
                    rawWorldCenter = DeviceGPS.instance.rawUnityPosition;

                    ArCreater();
                    update = false;
                }
            }
        }
    }

    void ArCreater()
    {
        int arCount = AR.Count;
        int LatLonAltCount = LatLonAlt.Count;
        bool create = true;

        if(arCount != LatLonAltCount) 
        {
            print("AR size is not equel LatLonAlt size");
            create = false;
        }

        if(arCount == 0)
        {
            create = false;
        }

        if(create == true)
        {
            for(int i = 0; i < arCount; i++)
            {
                Vector3d rawPos = GetPos(LatLonAlt[i].x, LatLonAlt[i].y, LatLonAlt[i].z);
                Vector3d pos = new Vector3d(rawPos.x - rawWorldCenter.x, rawPos.y - rawWorldCenter.y, rawPos.z - rawWorldCenter.z);
                GameObject ar = Instantiate(AR[i], (Vector3)pos, Quaternion.Euler(0,0,0));
                ar.layer = 10;

                ARControl.instance.shed.Add(ar);

                if(i == arCount -1)
                {
                    ARControl.instance.LatLonAlt = LatLonAlt;
                    ARControl.instance.listReady = true;
                }
            }
        }
    }

    private Vector3d GetPos(double lat, double lon, double alt)
    {
        List<double> dp = GetPOS.GTU(lat, lon, alt);
        
        return new Vector3d(dp[0],dp[1],dp[2]);
    }
}
