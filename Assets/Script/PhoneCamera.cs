using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCamera;
    private Texture defaultBackground;

    public GameObject background;
    public AspectRatioFitter fit;
    
    void Start()
    {
        defaultBackground = background.GetComponent<Renderer>().material.mainTexture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            Debug.Log("No camera detected");
            camAvailable = false;
            return;
        }

        for(int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCamera = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCamera == null)
        {
            Debug.Log("Unable to find back camera");
            return;
        }

        backCamera.Play();
        background.GetComponent<Renderer>().material.mainTexture = backCamera;

        camAvailable = true;
    }

    void Update()
    {
        if(!camAvailable)
            return;

        float ratio = (float)backCamera.width / (float)backCamera.height;
        fit.aspectRatio = ratio;

        float scaleY = backCamera.videoVerticallyMirrored ? -1f: 1f;
        background.transform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCamera.videoRotationAngle;
        background.transform.localEulerAngles = new Vector3(0, 0, orient);
    }
}
