using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class ServerTalker : MonoBehaviour
{
    JSONNode objectModelNode;
    JSONNode ticketNode;
    // Start is called before the first frame update
    void Awake()
    {

        // Make a web request to get info from the server
        // this will be a text response.
        // This will return/continue IMMEDIATELY, but the coroutine
        // will take several MS to actually get a response from the server.
        makeGetRequest("http://192.168.1.201:3000/objectModels/", "nordservoold");


        
        //makePutRequest("http://localhost:3000/objectModels/", "005visemes");
        // StartCoroutine(PutWebData("http://localhost:3000/objectModels/005visemes"));
    }

    void Start()
    {
        Debug.Log(objectModelNode[1]);
        //saveObjectModelPosition(1f,1f,1f,1f,1f,1f,1f,1f,1f);
        string jsonstring = objectModelNode.ToString();
        makePutRequest("http://192.168.1.201:3000/objectModels/", "nordservoold", jsonstring);
    }

    void makeGetRequest(string address, string slug){
        StartCoroutine( GetWebData(address, slug) );
    }

    void makePutRequest(string address, string slug, string json){
        StartCoroutine( PutWebData(address, slug, json) );
    }

    IEnumerator GetWebData( string address, string myID )
    {
        Debug.Log("GetRequest "+address+myID);
        UnityWebRequest www = UnityWebRequest.Get(address + myID);
        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Something went wrong: " + www.error);
        }
        else
        {
            Debug.Log("GET response: "+ www.downloadHandler.text );
            ProcessServerResponse(address, www.downloadHandler.text);
            //putrequest
            //StartCoroutine(PutWebData("http://localhost:3000/objectModels/005visemes", www.downloadHandler.text));
        }
    }

     IEnumerator PutWebData( string address, string slug, string json)
    {
        Debug.Log("PutRequest "+address);
        //string json="bla";
        address=address+slug;
        string jsonstring = objectModelNode.ToString();
        UnityWebRequest www = UnityWebRequest.Put(address, json);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Something went wrong: " + www.error);
        }
        else
        {
            Debug.Log("Put request succes");
            Debug.Log( www.downloadHandler.text );

          //  ProcessServerResponse(www.downloadHandler.text);
        }
    }

    void ProcessServerResponse( string address, string rawResponse )
    {
        if(address=="http://localhost:3000/objectModels/")
        {
            objectModelNode = JSON.Parse(rawResponse);
        }
        else if (address=="http://localhost:3000/tickets/")
        {
            ticketNode = JSON.Parse(rawResponse);
        }
        // That text, is actually JSON info, so we need to 
        // parse that into something we can navigate.

        // Output some stuff to the console so that we know
        // that it worked.

        //Debug.Log("fileName: " + node[0][1]);
        //Debug.Log("Misc Data: " + node["someArray"][1]["name"] + " = " + node["someArray"][1]["value"]);
    }

    void saveObjectModelPosition(float positionX, float positionY, float positionZ, float rotationX, float rotationY, float rotationZ, float scaleX, float scaleY, float scaleZ){
        objectModelNode["position"][0]=positionX;
        objectModelNode["position"][1]=positionY;
        objectModelNode["position"][2]=positionZ;

        objectModelNode["position"][3]=rotationX;
        objectModelNode["position"][4]=rotationY;
        objectModelNode["position"][5]=rotationZ;

        objectModelNode["position"][6]=scaleX;
        objectModelNode["position"][7]=scaleY;
        objectModelNode["position"][8]=scaleZ;
    }
}