using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class ModelCalibrator : MonoBehaviour
{
    public ObjectManipulator objManipulator;
    [SerializeField] GameObject serverCommunicator;
    [SerializeField] GameObject modelCalibrator;
    // Start is called before the first frame update
    void Start()
    {
        objManipulator = modelCalibrator.GetComponent<ObjectManipulator>();
        objManipulator.enabled = false;
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
            serverCommunicator.GetComponent<ServerCommunicator>().saveObjectModelPosition(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z,transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z, transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void setToSavedPosition(float px, float py, float pz, float rx, float ry, float rz, float sx, float sy, float sz)
    {
        Debug.Log("SetToSavedPosition: "+px+" "+py+" "+pz+" "+rx+" "+ry+" "+rz+" "+sz+" "+sy+" "+sz+" ");
        modelCalibrator.transform.localPosition = new Vector3(px,py,pz);
        modelCalibrator.transform.localEulerAngles = new Vector3(rx,ry,rz);
        modelCalibrator.transform.localScale = new Vector3(sx,sy,sz);

    }
}
