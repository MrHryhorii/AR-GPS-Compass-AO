using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class gpsui : MonoBehaviour
{
    [HideInInspector]public Image pointerImage;
    [HideInInspector]public GameObject pointerImageObject;

    public Text gpsText;
    public GameObject gpsTextObject;

    public Text disText;
    public GameObject disTextObject;
    public float deltaTime;

    [HideInInspector]public Transform target;

    [HideInInspector]private bool isHasTarget = false;
    [HideInInspector]public bool isInfo = false;

    double lat, lon, alt;
    // Start is called before the first frame update
    void Start()
    {
        lat = DeviceGPS.instance.latitude;
        lon = DeviceGPS.instance.longitude;
        alt = DeviceGPS.instance.altitude;

        InvokeRepeating("UpdateUI", 1.0f, 1.0f); // Get position each seconds
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        
        if(ARControl.instance.shed.Count > 0) isHasTarget = true; else isHasTarget = false;

        if(isHasTarget)
        {
            NearTarget();

            RotatePointer(target);
        }

        if (isInfo == false)
        {
            pointerImageObject.SetActive(false);
            gpsTextObject.SetActive(false);
            disTextObject.SetActive(false);
        }
        else
        {
            pointerImageObject.SetActive(true);
            gpsTextObject.SetActive(true);
            disTextObject.SetActive(true); 
        }
        
    }

    void UpdateUI()
    {
        lat = DeviceGPS.instance.latitude;
        lon = DeviceGPS.instance.longitude;
        alt = DeviceGPS.instance.altitude;

        if(isInfo)
        {
            gpsText.text = "LAT:" + lat.ToString() +"\n"+ "LON:" + lon.ToString() +"\n"+ "ALT:" + alt.ToString();

            if(target)
            {
                float fps = 1.0f / deltaTime;

                float dist = Vector3.Distance(transform.position, target.position);
                dist = dist/10;
                disText.text = "Distance:" + dist.ToString("F2") + " " + "fps:" + Mathf.Ceil (fps).ToString ();
            }
        }
    }

    void NearTarget()
    {
        float distance = Mathf.Infinity;
        Vector3 ourPosition = transform.position;

        foreach (GameObject item in ARControl.instance.shed)
        {
            Vector3 diff = item.transform.position - ourPosition;
            float curDistance = diff.sqrMagnitude;

            if(curDistance < distance)
            {
                target = item.transform;
                distance = curDistance;
            }
        }
    }

    void RotatePointer(Transform target)
    {
        float camAngle = CamAngle(Camera.main.transform.eulerAngles.y);
        float targetAngle = AngleVector2(new Vector2(target.position.x,target.position.z));

        float pointerAngle = NewAngle(camAngle, targetAngle);

        pointerImage.rectTransform.rotation = Quaternion.Euler(0f, 0f, pointerAngle);
    }

    public static float AngleVector2(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    public static float CamAngle(float angle)
    {
        if(angle < 0)
        {
            return 360 + angle;
        }
        else
        {
            if(angle > 360)
            {
                return angle - 360;
            }
        }
        return angle;
    }

    public static float NewAngle(float ang1, float ang2)
    {
        float diff = ang1 - ang2;
        if(diff < 0)
        {
            return 360 + diff;
        }
        else
        {
            if(diff > 360)
            {
                return diff - 360;
            }
        }
        return diff;
    }

    public void Button()
    {
        isInfo = !isInfo;
    }
}
