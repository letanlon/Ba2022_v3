using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;

public class Pointer : MonoBehaviour
{
    public float defaultLength = 8.0f;
    public GameObject pointer;
    [SerializeField] MixedRealityInputAction pointerAction = MixedRealityInputAction.None;

    private LineRenderer lineRenderer = default;

    void Awake()
    {
        lineRenderer=GetComponent<LineRenderer>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        //Use default or distance
        float targetLength = defaultLength;

        //Raycast
        RaycastHit hit = CreateRaycast(targetLength);


        //Default
        Vector3 endPosition = transform.position + (transform.forward * targetLength);
        //Or based on hit
        if (hit.collider !=null)
        {
            endPosition = hit.point;
        }
        // Set position of the dot
        pointer.transform.position = endPosition;
        // Set linerenderer
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    public void OnActionEnded(BaseInputEventData eventData)
    {
        Debug.Log("Action stopped: " + eventData.MixedRealityInputAction.Description);
        if(eventData.MixedRealityInputAction == pointerAction)
        {
            //StopDraw();
        }
    }

    public void OnActionStarted(BaseInputEventData eventData)
    {
        
        Debug.Log("Action started: " + eventData.MixedRealityInputAction.Description);
        if(eventData.MixedRealityInputAction == pointerAction)
        {
            //StartDraw();
        }
    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength);

        return hit;
    }
}
