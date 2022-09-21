using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] GameObject remoteHandmenu;
    [SerializeField] GameObject onsiteHandmenu;
    [SerializeField] GameObject markerMenu;

    [SerializeField] bool isRemote;
    [SerializeField] GameObject dialog;
    [SerializeField] TextMeshPro dialogTitle;
    [SerializeField] TextMeshPro dialogDescription;

    GameObject buttonDialogLoadModel;
    GameObject buttonDialogRemote;   
    GameObject buttonDialogOnsite;
    GameObject buttonDialogSavePosition;

    bool scanSuccess;




    // Start is called before the first frame update
    void Start()
    {   
        buttonDialogRemote = GameObject.Find("ButtonDialogRemote");
        buttonDialogOnsite = GameObject.Find("ButtonDialogOnsite");

        buttonDialogSavePosition = GameObject.Find("ButtonDialogSavePosition");
        buttonDialogLoadModel = GameObject.Find("ButtonDialogLoadModel");

        
        // deactivate buttons
        buttonDialogLoadModel.SetActive(false);
        buttonDialogSavePosition.SetActive(false);


        //turn off menu by default
        remoteHandmenu.SetActive(false);
        onsiteHandmenu.SetActive(false);
        markerMenu.SetActive(false);


        // findDialog
        dialogTitle = GameObject.Find("DialogTitle").GetComponent<TextMeshPro>();
        dialogDescription = GameObject.Find("DialogDescription").GetComponent<TextMeshPro>();

        scanSuccess=true;

        //automaticly set vr users as remote users, because vr users are always remote
        //Hololens can be remote/onsite, everything else is always VR and VR is always remote
        if(SystemInfo.deviceModel.Contains("HoloLens")) //for testing ar in editor
        //if(!SystemInfo.deviceModel.Contains("HoloLens"))
        {
            Debug.Log("Using remote user mode");
           //isRemote; clear dialog and destroy buttons
            buttonDialogOnsite.SetActive(false);
            buttonDialogRemote.SetActive(true);
            dialogTitle.text="Dialog";

            useRemoteMenu();
            dialogDescription.text="Raise your left hand palm up, to access the menu.";

        }
        else
        {
            //is AR user
            // AR user can be either onsite oder remote-user
            Debug.Log("Select user mode");
            dialogDescription.text="HoloLens detected. Please choose your desired user-mode.";
        }
    }


    public void destroyUIElement(string tag){
        // destroy a model if it already exist
        if (GameObject.FindGameObjectsWithTag(tag) != null)
        {
            GameObject go = GameObject.Find(tag);
            Destroy(go, 1);
        }
    }

    public void deactivateUIElement(string name){
        // destroy a model if it already exist
        if (GameObject.Find(name) != null)
        {
            GameObject go = GameObject.Find(name);

            for (int i = 0; i < go.transform.childCount; i++)
            {
                go.transform.GetChild(i).gameObject.SetActive(false);
            }

            go.SetActive(false);

        }
    }

    public void activateUIElement(string name){
        // destroy a model if it already exist
        if (GameObject.Find(name) != null)
        {
            GameObject go = GameObject.Find(name);

            for (int i = 0; i < go.transform.childCount; i++)
            {
                go.transform.GetChild(i).gameObject.SetActive(true);
            }

            go.SetActive(true);
        }
        else
        {
            Debug.Log(name+ " not found");
        }
    }

    public void setDialogDescription(string str)
    {
        dialogDescription.text=str;
    }

    public void setDialogTitle(string str)
    {
        dialogTitle.text=str;
    }

    // Update is called once per frame
    void Update()
    {

        //for updating DialogText about QR scan status
        if (!scanSuccess && GameObject.FindGameObjectWithTag("QrCode") != null)
        {  
            Debug.Log("QRCode detected");
            scanSuccess=true;
            dialogDescription.text="QR Code detected."+"\n"+"Press 'Load' to load model to the scene or scan a new QR-Code.";
            buttonDialogLoadModel.SetActive(true);
        }
    }


    //for buttons in mod selection prompt
    public void useRemoteMenu()
    {
        remoteHandmenu.SetActive(true);
        buttonDialogOnsite.SetActive(false);
        buttonDialogRemote.SetActive(false);
        dialogDescription.text="Lift your left hand palms up, to access to menu.";
    }
    public void useOnsiteMenu()
    {
        onsiteHandmenu.SetActive(true);
        buttonDialogOnsite.SetActive(false);
        buttonDialogRemote.SetActive(false);
        setDialogDescription("Raise your left hand palm up, to access the menu.");
        setDialogTitle("Dialog");
    }
    
    public void startScanning()
    {
        Debug.Log("scanSuccess = false");
        scanSuccess=false;
        //update() starts looking for detected QR-Codes
    }

    public void toggleMarkerMenu()
    {
        markerMenu.SetActive(true);
        markerMenu.SetActive(markerMenu.active);
    }
}
