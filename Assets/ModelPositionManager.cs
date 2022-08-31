using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelPositionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTransformByPose (Pose pose)
    {
        transform.position=pose.position;
        transform.rotation=pose.rotation;
    }
}
