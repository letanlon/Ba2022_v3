using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuHandling : MonoBehaviour
{
    [SerializeField] GameObject remoteHandmenu;
    [SerializeField] GameObject onsiteHandmenu;
    [SerializeField] bool isRemote;
    [SerializeField] GameObject promptModeSelection;
    [SerializeField] TextMeshPro testTitle;


    // Start is called before the first frame update

    void Start()
    {
        //turn off menu by default
        remoteHandmenu.SetActive(false);
        onsiteHandmenu.SetActive(false);
        
        Debug.Log("system name: "+SystemInfo.deviceModel +" "+SystemInfo.deviceModel.Contains("HoloLens"));
            //for testing:
            testTitle.text=SystemInfo.deviceModel+" "+SystemInfo.deviceModel.Contains("HoloLens");
        //Hololens can be remote/onsite, everything else is always VR and VR is always remote
        if(!SystemInfo.deviceModel.Contains("HoloLens"))
        {
            

           //isRemote
            destroyPrompt("promptModeSelection");

            Debug.Log("Using remote user mode");
            toggleMenu(remoteHandmenu);
        }
        else
        {
            //is AR user
            // AR user can be either onsite oder remote-user
            Debug.Log("Select user mode");
        }
    }


    private void destroyPrompt(string tag){
            // destroy a model if it already exist
            if (GameObject.FindGameObjectsWithTag(tag) != null)
            {
                GameObject go = GameObject.Find(tag);
                Destroy(go, 1);
            }
        }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void toggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.active);
    }


    //for buttons in mod selection prompt
    public void useRemoteMenu()
    {
        remoteHandmenu.SetActive(true);
    }
    public void useOnsiteMenu()
    {
        onsiteHandmenu.SetActive(true);
    }
}
