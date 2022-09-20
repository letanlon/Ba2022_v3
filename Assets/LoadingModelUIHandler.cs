using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingModelUIHandler : MonoBehaviour
{
    [SerializeField] TextMeshPro description;
    GameObject buttonLoadModel;
    bool scanSuccess;
    // Start is called before the first frame update
    void Awake(){

    }
    void Start()
    {
        description = GameObject.Find("DialogDescription").GetComponent<TextMeshPro>();
        scanSuccess=false;
        buttonLoadModel=GameObject.Find("buttonLoadModel");
    }

    // Update is called once per frame
    void Update()
    {
        if (scanSuccess=false && GameObject.Find("QRCode") != null)
        {  
                scanSuccess=true;
                description.text="QR Code detected."+"\n"+"Press 'Load' to Load Model to the scene.";
                buttonLoadModel.SetActive(true);
        }
        
        
        if (GameObject.Find("1") != null)
        {  
                description.text="Loading Done.";
        }
    }

}
