using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPointerAsParent : MonoBehaviour
{
    // Start is called before the first frame update
        private void Start()
    {

        if (GameObject.FindGameObjectWithTag("Pointer") != null)
        {
            GameObject pointer = GameObject.FindGameObjectWithTag("Pointer");

            this.transform.position=pointer.transform.position;
            this.transform.rotation=pointer.transform.rotation;
            transform.parent = GameObject.FindGameObjectWithTag("Pointer").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
