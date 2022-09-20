using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class ModelCalibrator : MonoBehaviour
{
    public ObjectManipulator objManipulator;
    [SerializeField] GameObject serverCommunicator;
    [SerializeField] GameObject modelCalibrator;
    [SerializeField] bool modelVisibility;
    [SerializeField] GameObject buttonDialogSavePosition;

    MenuHandler menuHandler;


    // Start is called before the first frame update
    void Start()
    {
        objManipulator = modelCalibrator.GetComponent<ObjectManipulator>();
        objManipulator.enabled = false;
        modelVisibility = true;

        menuHandler = GameObject.Find("UI_Handler").GetComponent<MenuHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleCalibrationMode()
    {
        objManipulator.enabled = !objManipulator.enabled;
        /* this is to send position of model to server, but we want to save the position of the calibrator
         GameObject go;
        go = GameObject.Find("1");
        Debug.Log(go.transform.position);
        Debug.Log(go.transform.rotation.x);
        Debug.Log(go.transform.localScale.x);

        //saving calibrated positions
        if(objManipulator.enabled==false)
        {
            serverCommunicator.GetComponent<ServerCommunicator>().saveObjectModelPosition(go.transform.position.x,go.transform.position.y,go.transform.position.z,go.transform.rotation.x, go.transform.rotation.y, go.transform.rotation.z, go.transform.localScale.x, go.transform.localScale.y, go.transform.localScale.z);
        }
        */

        //save position of calibrator:
        if(objManipulator.enabled==false)
        {
            //serverCommunicator.GetComponent<ServerCommunicator>().saveObjectModelPosition(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z,transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z, transform.localScale.x, transform.localScale.y, transform.localScale.z);
            serverCommunicator.GetComponent<ServerCommunicator>().saveObjectModelPosition(transform);
            menuHandler.setDialogDescription("Saved new position.");
            menuHandler.setDialogTitle("Calibrating Mode (OFF)");
            buttonDialogSavePosition.SetActive(false);
        }
        else
        {
            string description = "Use your hands to set the model to the desired position"+"\n"+"Repress 'Model Calibration' to save the new model position.";
            menuHandler.setDialogDescription(description);
            menuHandler.setDialogTitle("Calibrating Mode (ON)");
            buttonDialogSavePosition.SetActive(true);
        }
    }

    public void setToSavedPosition(float px, float py, float pz, float rx, float ry, float rz, float sx, float sy, float sz)
    {
        Debug.Log("SetToSavedPosition: "+px+" "+py+" "+pz+" "+rx+" "+ry+" "+rz+" "+sz+" "+sy+" "+sz+" ");
        modelCalibrator.transform.localPosition = new Vector3(px,py,pz);
        modelCalibrator.transform.localEulerAngles = new Vector3(rx,ry,rz);
        modelCalibrator.transform.localScale = new Vector3(sx,sy,sz);

    }

    public void setToSavedWorldPosition(float px, float py, float pz, float rx, float ry, float rz, float sx, float sy, float sz)
    {
        Debug.Log("SetToSavedPosition: "+px+" "+py+" "+pz+" "+rx+" "+ry+" "+rz+" "+sz+" "+sy+" "+sz+" ");
        modelCalibrator.transform.position = new Vector3(px,py,pz);
        modelCalibrator.transform.eulerAngles = new Vector3(rx,ry,rz);
        modelCalibrator.transform.localScale = new Vector3(sx,sy,sz);
    }

    public void toggleVisibility(){
        Debug.Log("toggledVisiblity");
        if (GameObject.Find("1") != null)
        {
            //it exists
            Debug.Log("it exists");
            foreach (Transform child in GameObject.Find("1").transform) {
            child.gameObject.GetComponent< Renderer >().enabled = !modelVisibility;
            }
            modelVisibility=!modelVisibility;
        }
        
    }
}
