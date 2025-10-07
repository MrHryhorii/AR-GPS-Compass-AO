using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCamera : MonoBehaviour
{
    private bool gyroEnabled;
    private Gyroscope gyro;

    private Quaternion rot;

    public bool isLowPassFilter = false;
    float lowPassFactor = 0.8f; //Value should be between 0.01f and 0.99f. Smaller value is more damping.
    bool init = true;

    // Start is called before the first frame update
    void Start()
    {
        gyroEnabled = EnableGyro();
    }

    private bool EnableGyro()
    {
        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            transform.rotation = Quaternion.Euler(90f,180f,0f);
            rot = new Quaternion(0, 0, 1, 0);

            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isLowPassFilter)
        {
            if(gyroEnabled)
            {
                Camera.main.transform.localRotation = lowPassFilterQuaternion(Camera.main.transform.localRotation , gyro.attitude * rot, lowPassFactor, init);
                init = false;
            }
        }
        else
        {
            if(gyroEnabled)
            {
                Camera.main.transform.localRotation = gyro.attitude * rot;
            }
        }
    }

    //Function:
    Quaternion lowPassFilterQuaternion(Quaternion intermediateValue, Quaternion targetValue, float factor, bool init)
    {
        //intermediateValue needs to be initialized at the first usage.
        if(init){		
            intermediateValue = targetValue;
        }
    
        intermediateValue = Quaternion.Lerp(intermediateValue, targetValue, factor);
        return intermediateValue;
    }
}
