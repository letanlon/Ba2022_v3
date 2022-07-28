using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class ServerTalker : MonoBehaviour
{
    [SerializeField] string serveradr;
    // Start is called before the first frame update
    void Start()
    {
        // Make a web request to get info from the server
        // this will be a text response.
        // This will return/continue IMMEDIATELY, but the coroutine
        // will take several MS to actually get a response from the server.
        StartCoroutine(GetWebData("http://192.168.1.201:3000/objectModels/", "servomotor"));

    }

    IEnumerator GetWebData(string address, string myID)
    {
        UnityWebRequest www = UnityWebRequest.Get(address + myID);
        Debug.Log(address + myID);
        yield return www.SendWebRequest();


        /*
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.result);
            Debug.LogError("Something went wrong: " + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            Debug.Log(www.downloadHandler.text);

            ProcessServerResponse(www.downloadHandler.text);

        } */
        Debug.Log(www.downloadHandler.text);

        ProcessServerResponse(www.downloadHandler.text);
    }

    void ProcessServerResponse(string rawResponse)
    {
        // That text, is actually JSON info, so we need to 
        // parse that into something we can navigate.

        JSONNode node = JSON.Parse(rawResponse);

        // Output some stuff to the console so that we know
        // that it worked.

        Debug.Log("Name: " +node["objectModel"]["name"]);
    }


}