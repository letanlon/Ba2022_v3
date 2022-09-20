using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWorld : MonoBehaviour
{

    [SerializeField] bool isHPReverb;

    [SerializeField] GameObject environmentPrefab =default;
    bool environmentActive;
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
            environmentActive = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleWorld()
    {
        environmentActive = !environmentActive;

        if(environmentActive)
        {
            Instantiate(environmentPrefab);
        }
        else
        {
            GameObject go = GameObject.FindGameObjectWithTag("World");
            Destroy(go, 1);
        }
    }
}
