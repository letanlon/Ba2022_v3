using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWorld : MonoBehaviour
{

    [SerializeField] bool isHPReverb;

    [SerializeField] GameObject environmentPrefab =default;
    // Start is called before the first frame update
    void Start()
    {
        if(SystemInfo.deviceModel.Contains("HoloLens"))
        {
           isHPReverb=false;
        }

        if (isHPReverb)
        {
            Instantiate(environmentPrefab);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
