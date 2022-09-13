using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftCamera : MonoBehaviour
{
    public bool onHoloLens;
    [SerializeField] bool isHPReverb;


    // Start is called before the first frame update
    // Shift camera offset because hp reverb starts in a different position compared to hololens
    void Awake()
    {
        if(SystemInfo.deviceModel.Contains("HoloLens"))
        {
            isHPReverb=false;
        }

        if (isHPReverb)
        {
            Vector3 position = new Vector3(0f, -1.61f, 0f); // 3f is actually too high, just for testing purposes...

            this.transform.position = position;

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
