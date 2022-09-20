using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerHandler : MonoBehaviour
{
    [SerializeField] GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnCube()
    {
        GameObject spawn = Instantiate(cube);
        spawn.transform.position= new Vector3 (0.5f,0.1f,0.5f);
    }
}
