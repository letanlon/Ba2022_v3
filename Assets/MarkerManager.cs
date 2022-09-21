using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MRTK.Tutorials.MultiUserCapabilities;


public class MarkerManager : MonoBehaviour
{
   [SerializeField] GameObject cube;
    [SerializeField] GameObject laserpointer;
    private PhotonRoom photonRoom;



    // Start is called before the first frame update
    void Start()
    {
        photonRoom=GameObject.Find("NetworkRoom").GetComponent<PhotonRoom>();
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

    public void toggleLaserPointer()
    {
        if(GameObject.FindGameObjectWithTag("Laserpointer")!=null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Laserpointer"));
        }
        else
        {
            Instantiate(laserpointer); 
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            laserpointer.transform.position=camera.transform.position+camera.transform.forward*0.5f;
            PhotonRoom photonRoom =  GameObject.Find("NetworkRoom").GetComponent<PhotonRoom>();
            photonRoom.createSyncPointer();
        }
    }

    public void spawnArrow()
    {
        photonRoom.createArrow();
    }

        public void spawnMarker()
    {
        photonRoom.createMarker();
    }
}
