using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    public string isGps;
    public string isGyro;
    public string isCompass;
    public string isCameras;
    public string isBackCamera = "Не фронтальная камера не обнаружена...";
    private string shift = "\n";

    public Text statusText;
    public GameObject StartButton;

    private bool okGps = false;
    private bool okGyro = false;
    private bool okCompass = false;
    private bool okBackCamera = false;

    private bool start = false;

    void Start()
    {
        if(Input.location.isEnabledByUser)
        {
            isGps = "Служба определения местоположения включена...";
            okGps = true;
        }
        else
        {
            isGps = "Служба определения местоположения не включена...";
        }

        if(SystemInfo.supportsGyroscope)
        {
            isGyro = "Гироскоп доступный...";
            okGyro = true;
        }
        else
        {
            isGyro = "Гироскоп не доступный...";
        }

        Input.compass.enabled = true;
        if(Input.compass.enabled)
        {
            isCompass = "Компас доступный...";
            okCompass = true;
        }
        else
        {
            isCompass = "Компас не доступный...";
        }

        WebCamDevice[] devices = WebCamTexture.devices;
        if(devices.Length == 0)
        {
            isCameras = "Не обнаружены камеры...";
        }
        else
        {
            isCameras = "Обнаружено количество камер: " + devices.Length.ToString();
        }

        if(devices.Length != 0)
        {
            for(int i = 0; i < devices.Length; i++)
            {
                if (!devices[i].isFrontFacing)
                {
                    isBackCamera = "Не фронтальная камера обнаружена...";
                    okBackCamera = true;
                }
            }
        }

        statusText.text = isGps + shift + isGyro + shift + isCompass + shift + isBackCamera + shift + isCameras;

        if(okGps && okGyro && okCompass && okBackCamera)
        {
            start = true;
        }
        else
        {
            start = false;
        }

        if(start)
        {
            StartButton.SetActive(true);
        }
        else
        {
            StartButton.SetActive(false);
        }
    }

    public void ClickGO()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
