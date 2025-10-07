using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTexture : MonoBehaviour
{
    public Camera camera;
    WebCamTexture webCamTexture;

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);

        renderer.material.mainTexture = webCamTexture;

        webCamTexture.requestedWidth = camera.pixelWidth / camera.pixelHeight;

        webCamTexture.Play();

        //transform.localScale = new Vector3(1, (float)webCamTexture.height/(float)webCamTexture.width, 1);
    }

    // Update is called once per frame
    void Update()
    {
        float orW = 1;
        float orH = 1;
        float camAsp = camera.aspect;

        float nW = camAsp;
        float diff = nW - orW;
        float nH = camAsp * diff;

        if(Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            transform.localRotation = Quaternion.Euler(90f, -90f, 90f);
            transform.localScale = new Vector3(nW, 1f, nH);
        }

        if(Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            transform.localRotation = Quaternion.Euler(270f, -90f, 90f);
            transform.localScale = new Vector3(nW, 1f, nH);
        }

        if(Screen.orientation == ScreenOrientation.Portrait) 
        {
            transform.localRotation = Quaternion.Euler(0f, -90f, 90f);
            transform.localScale = new Vector3(-nW / diff, 1f, 1f);
        }

        if(Screen.orientation == ScreenOrientation.PortraitUpsideDown) 
        {
            transform.localRotation = Quaternion.Euler(180f, -90f, 90f);
            transform.localScale = new Vector3(-nW / diff, 1f, 1f);
        }
    }
}
