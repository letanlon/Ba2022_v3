    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;
    using SimpleJSON;
    using TriLibCore;
    using TriLibCore.General;
    using QRTracking;
    using TMPro;


    public class ServerCommunicator : MonoBehaviour
    {
        JSONNode objectModelNode;
        JSONNode ticketNode;
        [SerializeField] GameObject modelCalibrator = default;
        [SerializeField] GameObject ModelHolder = default;
        [SerializeField] TextMeshPro dialogText = default;
        [SerializeField] TextMeshPro errorText = default;
        private float progressHelp;



        [SerializeField] string[] url;
        [SerializeField] int addrressInUse;
        // Start is called before the first frame update
        void Awake()
        {
            addrressInUse=0;
            // Make a web request to get info from the server
            // this will be a text response.
            // This will return/continue IMMEDIATELY, but the coroutine
            // will take several MS to actually get a response from the server.
            makeGetRequest("/objectModels/", "nordservoold");


            
            //makePutRequest("http://localhost:3000/objectModels/", "005visemes");
            // StartCoroutine(PutWebData("http://localhost:3000/objectModels/005visemes"));
        }

        public void test()
        {
            string jsonstring = objectModelNode.ToString();
            makePutRequest("/objectModels/", "nordservoold", jsonstring);
        }

        public void makeGetRequest(string address, string slug){
            StartCoroutine( GetWebData(address, slug) );
        }

        public void makePutRequest(string address, string slug, string json){
            StartCoroutine( PutWebData(address, slug, json) );
        }

        IEnumerator GetWebData( string address, string slug )
        {
            string fullAddress=url[addrressInUse]+address+slug;
            Debug.Log("GetRequest "+fullAddress);
            UnityWebRequest www = UnityWebRequest.Get(fullAddress);
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Something went wrong: " + www.error);
            }
            else
            {
                Debug.Log("GET response: "+ www.downloadHandler.text );
                ProcessServerResponse(url[addrressInUse]+address, www.downloadHandler.text);
                //putrequest
                //StartCoroutine(PutWebData("http://localhost:3000/objectModels/005visemes", www.downloadHandler.text));
            }
        }

        IEnumerator PutWebData( string address, string slug, string json)
        {
            string fullAddress=url[addrressInUse]+address+slug;
            Debug.Log("PutRequest "+fullAddress);
            UnityWebRequest www = UnityWebRequest.Put(fullAddress, json);
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
            if(address==url[addrressInUse]+"/objectModels/")
            {
                objectModelNode = JSON.Parse(rawResponse);
                Debug.Log(objectModelNode[1]);
            }
            else if (address==url[addrressInUse]+"/tickets/")
            {
                ticketNode = JSON.Parse(rawResponse);
            }
        }

        //public void saveObjectModelPosition(float positionX, float positionY, float positionZ, float rotationX, float rotationY, float rotationZ, float scaleX, float scaleY, float scaleZ){
        public void saveObjectModelPosition(Transform trans){

            //save local position of calibrated model / saving relative position to QR Code
            objectModelNode["position"][0]=trans.localPosition.x;
            objectModelNode["position"][1]=trans.localPosition.y;
            objectModelNode["position"][2]=trans.localPosition.z;

            objectModelNode["position"][3]=trans.localEulerAngles.x;
            objectModelNode["position"][4]=trans.localEulerAngles.y;
            objectModelNode["position"][5]=trans.localEulerAngles.z;

            objectModelNode["position"][6]=trans.localScale.x;
            objectModelNode["position"][7]=trans.localScale.y;
            objectModelNode["position"][8]=trans.localScale.z;

            //saving position in world, localScale and worldScale are identical
            objectModelNode["position"][9]=trans.position.x;
            objectModelNode["position"][10]=trans.position.y;
            objectModelNode["position"][11]=trans.position.z;

            objectModelNode["position"][12]=trans.eulerAngles.x;
            objectModelNode["position"][13]=trans.eulerAngles.y;
            objectModelNode["position"][14]=trans.eulerAngles.z;


            //update not necessary, as data will be written to database when finish application properly
            //but saving now better, in case app crashs or user doesnt properly exit app
            string jsonstring = objectModelNode.ToString();
            makePutRequest("/objectModels/", objectModelNode["slug"], jsonstring);
        }

        // This event is called when the model loading progress changes.
        // You can use this event to update a loading progress-bar, for instance.
        // The "progress" value comes as a normalized float (goes from 0 to 1).
        // Platforms like UWP and WebGL don't call this method at this moment, since they don't use threads.
            private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
        {
            progressHelp=progress;
            string progressStatus = "Progress: "+progress;
            Debug.Log(progressStatus);
            dialogText.text= progressStatus;

        }

        // This event is called when there is any critical error loading your model.
        // You can use this to show a message to the user.
        private void OnError(IContextualizedError contextualizedError)
        {
            string errorStatus = "error: "+contextualizedError;
            Debug.Log(errorStatus);
            dialogText.text= errorStatus;
        }

        public void loadModel()//string modelname)
        {
            //in case that model was already loaded before
            destroyExistingModel();
                                            // request scanned model
            string qrCodeText = GameObject.FindGameObjectWithTag("QrCode").GetComponent<QRCode>().CodeText;
            ServerCommunicator serverCommunicator =  GameObject.FindGameObjectWithTag("ServerCommunicator").GetComponent<ServerCommunicator>();
            serverCommunicator.makeGetRequest("/objectModels/", qrCodeText);
            QRCodesManager qrCoderManager;
            qrCoderManager = GameObject.Find("QRCodesManager").GetComponent<QRCodesManager>();
            //qrCoderManager.clearQrCodeList();


            // Creates an AssetLoaderOptions instance.
            // AssetLoaderOptions is a class used to configure many aspects of the loading process.
            // We won't change the default settings this time, so we can use the instance as it is.
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();

            // Creates the web-request.
            // The web-request contains information on how to download the model.
            // Let's download a model from the TriLib website.
            var webRequest = AssetDownloader.CreateWebRequest("http://192.168.1.201:3000/objectModels/file/"+qrCodeText);

            // Important: If you're downloading models from files that are not Zipped, you must pass the model extension as the last parameter from this call (Eg: "fbx")
            // Begins the model downloading.
            AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions, null, "obj");    
        }
        public void loadModelOnVr()//string modelname)
        {
            progressHelp=0;
            destroyExistingModel();
            // if model already exist, destroy it first => call method again to update new position of model

            string modelName = "nordservoold";
            //update json data of model
            makeGetRequest("/objectModels/", modelName);

            // Creates an AssetLoaderOptions instance.
            // AssetLoaderOptions is a class used to configure many aspects of the loading process.
            // We won't change the default settings this time, so we can use the instance as it is.
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();

            // Creates the web-request.
            // The web-request contains information on how to download the model.
            // Let's download a model from the TriLib website.
            var webRequest = AssetDownloader.CreateWebRequest("http://192.168.1.201:3000/objectModels/file/"+modelName);
            //updating json model in case a different user recalibrated model in the meantime
            makeGetRequest("/objectModels/", modelName);


            // Important: If you're downloading models from files that are not Zipped, you must pass the model extension as the last parameter from this call (Eg: "fbx")
            // Begins the model downloading.
            AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions, null, "obj");

        }
        
        public void updateModelWorldPosition()//string modelname)
        {
            if(progressHelp>=1)
            {
                GameObject _gameObject;
                _gameObject = GameObject.Find("1");
                //applying position in world
                _gameObject.transform.localScale = new Vector3(objectModelNode["position"][6],objectModelNode["position"][7],objectModelNode["position"][8]);
                _gameObject.transform.position = new Vector3(objectModelNode["position"][9],objectModelNode["position"][10],objectModelNode["position"][11]);
                _gameObject.transform.eulerAngles = new Vector3(objectModelNode["position"][12],objectModelNode["position"][13],objectModelNode["position"][14]);
            }
        
        }
            
        public void attachModelasChild()
        {


        }

        public void setModelHolderToTrackedPosition(){
            // ModelHolder.transform.position=pose.position;
            // ModelHolder.transform.rotation=pose.rotation;
            //Move ModelHolder to tracked position
            //Pose pose = GameObject.FindGameObjectWithTag("QrCode").GetComponent<SpatialGraphNodeTracker>().getPose();

            //AttachModelAsChild of Calibrator
            GameObject _gameObject;
            _gameObject = GameObject.Find("1");
            _gameObject.transform.parent = modelCalibrator.transform;

            //set ModelHolder to QR Position
            Transform qrTransform = GameObject.FindGameObjectWithTag("QrCode").transform;

            Debug.Log("setModelHolderTo position: "+qrTransform.position.x+" "+qrTransform.position.y+" position: "+qrTransform.position);
            Debug.Log("setModelHolderTo rotation: "+qrTransform.rotation.x+" "+qrTransform.rotation.y);

            ModelHolder.transform.position=qrTransform.position;
            ModelHolder.transform.rotation=qrTransform.rotation;

            //set modelCalibrator to last saved position
            modelCalibrator.GetComponent<ModelCalibrator>().setToSavedPosition(objectModelNode["position"][0], objectModelNode["position"][1], objectModelNode["position"][2], objectModelNode["position"][3], objectModelNode["position"][4],objectModelNode["position"][5],objectModelNode["position"][6], objectModelNode["position"][7],objectModelNode["position"][8]);
    }

        private void OnLoad(AssetLoaderContext assetLoaderContext)
        {
            // The root loaded GameObject is assigned to the "assetLoaderContext.RootGameObject" field.
            // If you want to make sure the GameObject will be visible only when all Materials and Textures have been loaded, you can disable it at this step.
            var myLoadedGameObject = assetLoaderContext.RootGameObject;
            myLoadedGameObject.SetActive(false);
        }
    
        // This event is called after OnLoad when all Materials and Textures have been loaded.
        // This event is also called after a critical loading error, so you can clean up any resource you want to.
        private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
        {
            // The root loaded GameObject is assigned to the "assetLoaderContext.RootGameObject" field.
            // You can make the GameObject visible again at this step if you prefer to.
            var myLoadedGameObject = assetLoaderContext.RootGameObject;
            myLoadedGameObject.SetActive(true);
        }

        private void destroyExistingModel(){
            // destroy a model if it already exist
            if (GameObject.Find("1") != null)
            {
                GameObject go = GameObject.Find("1");
                Destroy(go, 1);
            }
        }
    }
