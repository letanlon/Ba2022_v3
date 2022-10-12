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
        [SerializeField] GameObject modelHolder = default;
        [SerializeField] TextMeshPro dialogText = default;
        [SerializeField] TextMeshPro errorText = default;
        [SerializeField] string modelNameToLoad = default;

        MenuHandler menuHandler;
        private float loadingProgress=0;
        private Transform defaultPosition;

        [SerializeField] string[] url;
        [SerializeField] int addressInUse;
        // Start is called before the first frame update
        void Awake()
        {
            addressInUse=0;
            // Make a web request to get info from the server
            // this will be a text response.
            // This will return/continue IMMEDIATELY, but the coroutine
            // will take several MS to actually get a response from the server.
            // makeGetRequest("/objectModels/", "nordservoold");


            
            //makePutRequest("http://localhost:3000/objectModels/", "005visemes");
            // StartCoroutine(PutWebData("http://localhost:3000/objectModels/005visemes"));
        }

        void Start()
        {
            menuHandler = GameObject.Find("UI_Handler").GetComponent<MenuHandler>();
            defaultPosition = modelHolder.transform;
        }
        public void test()
        {

        }

        public void makeGetRequest(string address, string slug){
            StartCoroutine( GetWebData(address, slug) );
        }

        public void makePutRequest(string address, string slug, string json){
            StartCoroutine( PutWebData(address, slug, json) );
        }

        IEnumerator GetWebData( string address, string slug )
        {
            string fullAddress=url[addressInUse]+address+slug;
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
                ProcessServerResponse(url[addressInUse]+address, www.downloadHandler.text);
                //putrequest
                //StartCoroutine(PutWebData("http://localhost:3000/objectModels/005visemes", www.downloadHandler.text));
            }
        }

        IEnumerator PutWebData( string address, string slug, string json)
        {
            string fullAddress=url[addressInUse]+address+slug;
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
            if(address==url[addressInUse]+"/objectModels/")
            {
                objectModelNode = JSON.Parse(rawResponse);
                Debug.Log(objectModelNode[1]);
            }
            else if (address==url[addressInUse]+"/tickets/")
            {
                ticketNode = JSON.Parse(rawResponse);
            }
        }

        //public void saveObjectModelPosition(float positionX, float positionY, float positionZ, float rotationX, float rotationY, float rotationZ, float scaleX, float scaleY, float scaleZ){
        public void saveObjectModelPosition(Transform trans){

            //save local position of calibrated model / saving relative position to QR Code
            objectModelNode["position"]["positionX"]=trans.localPosition.x;
            objectModelNode["position"]["positionY"]=trans.localPosition.y;
            objectModelNode["position"]["positionZ"]=trans.localPosition.z;

            objectModelNode["position"]["rotationX"]=trans.localEulerAngles.x;
            objectModelNode["position"]["rotationY"]=trans.localEulerAngles.y;
            objectModelNode["position"]["rotationZ"]=trans.localEulerAngles.z;

            objectModelNode["position"]["scaleX"]=trans.localScale.x;
            objectModelNode["position"]["scaleY"]=trans.localScale.y;
            objectModelNode["position"]["scaleZ"]=trans.localScale.z;

            //saving position in world, localScale and worldScale are identical
            objectModelNode["position"]["world_positionX"]=trans.position.x;
            objectModelNode["position"]["world_positionY"]=trans.position.y;
            objectModelNode["position"]["world_positionZ"]=trans.position.z;

            objectModelNode["position"]["world_rotationX"]=trans.eulerAngles.x;
            objectModelNode["position"]["world_rotationY"]=trans.eulerAngles.y;
            objectModelNode["position"]["world_rotationZ"]=trans.eulerAngles.z;


            //update not necessary, as data will be written to database when finish application properly
            //but saving now better, in case app crashs or user doesnt properly exit app
            string jsonstring = objectModelNode.ToString();
            Debug.Log(trans.localPosition.y);
            Debug.Log("jsonstring string: "+jsonstring);
            makePutRequest("/objectModels/", objectModelNode["slug"], jsonstring);
        }

        // This event is called when the model loading progress changes.
        // You can use this event to update a loading progress-bar, for instance.
        // The "progress" value comes as a normalized float (goes from 0 to 1).
        // Platforms like UWP and WebGL don't call this method at this moment, since they don't use threads.
            private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
        {
            loadingProgress = progress;
            string progressStatus = "Loading progress: "+progress;
            Debug.Log(progressStatus);
            menuHandler.setDialogDescription(progressStatus);

            if(progress==1.0f)
            {
                Debug.Log("LoadingDone");
                menuHandler.setDialogDescription("Loading done.");
                menuHandler.setDialogTitle("Dialog");
                menuHandler.deactivateUIElement("ButtonDialogLoadModel");
            }
        }

        // This event is called when there is any critical error loading your model.
        // You can use this to show a message to the user.
        private void OnError(IContextualizedError contextualizedError)
        {
            string errorStatus = "error: "+contextualizedError;
            Debug.Log(errorStatus);
            menuHandler.setDialogDescription("Error: "+errorStatus);
        }

        public void loadModel()//string modelname)
        {
            //in case that model was already loaded before
            destroyExistingModel();
            //Destroy Load Button
            menuHandler.deactivateUIElement("ButtonDialogLoadModel");

            loadingProgress=0;


            // request scanned model
            string qrCodeText = GameObject.FindGameObjectWithTag("QrCode").GetComponent<QRCode>().CodeText;
            //ServerCommunicator serverCommunicator =  GameObject.FindGameObjectWithTag("ServerCommunicator").GetComponent<ServerCommunicator>();
            makeGetRequest("/objectModels/", qrCodeText);
            //QRCodesManager qrCoderManager;
            //qrCoderManager = GameObject.Find("QRCodesManager").GetComponent<QRCodesManager>();
            //qrCoderManager.clearQrCodeList();


            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            var webRequest = AssetDownloader.CreateWebRequest(url[addressInUse]+"/objectModels/file/"+qrCodeText);

            // Important: If you're downloading models from files that are not Zipped, you must pass the model extension as the last parameter from this call (Eg: "fbx")
            // Begins the model downloading
            AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions, null, "obj");

            StartCoroutine(placeModelAR());
        }

        public void loadModelOnVr()//string modelname)
        {
            loadingProgress=0;
            destroyExistingModel();
            // if model already exist, destroy it first => call method again to update new position of model

            //update json data of model
            //makeGetRequest("/objectModels/", modelNameToLoad);

            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            var webRequest = AssetDownloader.CreateWebRequest(url[addressInUse]+"/objectModels/file/"+modelNameToLoad);
            //updating json model in case a different user recalibrated model in the meantime
            makeGetRequest("/objectModels/", modelNameToLoad);


            // Important: If you're downloading models from files that are not Zipped, you must pass the model extension as the last parameter from this call (Eg: "fbx")
            // Begins the model downloading.
            AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions, null, "obj");
            StartCoroutine(placeModelVR());
        }
        
        public void updateModelWorldPosition()//string modelname)
        {
            if(loadingProgress>=1)
            {
                GameObject _gameObject;
                _gameObject = GameObject.Find("1");
                //applying position in world
                _gameObject.transform.localScale = new Vector3(objectModelNode["position"][6],objectModelNode["position"][7],objectModelNode["position"][8]);
                _gameObject.transform.position = new Vector3(objectModelNode["position"][9],objectModelNode["position"][10],objectModelNode["position"][11]);
                _gameObject.transform.eulerAngles = new Vector3(objectModelNode["position"][12],objectModelNode["position"][13],objectModelNode["position"][14]);
            }

        }

        
        IEnumerator placeModelAR()
        {
            Debug.Log("Waiting for LoadingProgress to finish");
            yield return new WaitUntil(() => loadingProgress>=1);
            Debug.Log("LoadingProgress>=100. placeModel");

            setModelHolderToTrackedPosition();

            if (GameObject.Find("ModelCalibrator") != null)
            {
                GameObject go = GameObject.Find("ModelCalibrator");
                saveObjectModelPosition(go.transform);
            }

            attachComponents();
        }
        IEnumerator placeModelVR()
        {
            Debug.Log("Waiting for LoadingProgress to finish");
            yield return new WaitUntil(() => loadingProgress>=1);
            Debug.Log("LoadingProgress>=100. placeModel");
            updateModelWorldPosition();
            attachComponents();
        }

        private void attachComponents()
        {
            if (GameObject.Find("1") != null)
            {
                GameObject go = GameObject.Find("1").transform.GetChild(0).gameObject;
                go.AddComponent<MeshCollider>();
                go.GetComponent<MeshCollider>().convex=true;
                //go.AddComponent(typeof(Microsoft.MixedReality.Toolkit.UI.ObjectManipulator));
                //go.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().enabled=false;
                //go.AddComponent(typeof(Microsoft.MixedReality.Toolkit.Input.NearInteractionGrabbable));
            }
        }
            
        public void setModelHolderToTrackedPosition(){
            //reset all positions
            //resetPosition(modelCalibrator);
            //resetPosition(modelHolder);
            //Debug("Resetted Positions");

            // modelHolder.transform.position=pose.position;
            // modelHolder.transform.rotation=pose.rotation;
            //Move ModelHolder to tracked position
            //Pose pose = GameObject.FindGameObjectWithTag("QrCode").GetComponent<SpatialGraphNodeTracker>().getPose();

            //AttachModelAsChild of Calibrator
            attachModelAsChild();

            //set ModelHolder to QR Position
            Transform qrTransform = GameObject.FindGameObjectWithTag("QrCode").transform;

            Debug.Log("setModelHolderTo position: "+qrTransform.position.x+" "+qrTransform.position.y+" position: "+qrTransform.position);
            Debug.Log("setModelHolderTo rotation: "+qrTransform.rotation.x+" "+qrTransform.rotation.y);

            modelHolder.transform.position=qrTransform.position;
            modelHolder.transform.rotation=qrTransform.rotation;

            //set modelCalibrator to last saved position
            modelCalibrator.GetComponent<ModelCalibrator>().setToSavedPosition(objectModelNode["position"][0], objectModelNode["position"][1], objectModelNode["position"][2], objectModelNode["position"][3], objectModelNode["position"][4],objectModelNode["position"][5],objectModelNode["position"][6], objectModelNode["position"][7],objectModelNode["position"][8]);

            //update position for database

            //stop qr scanning
            QRCodesManager qrManager = GameObject.Find("QRCodesManager").GetComponent<QRCodesManager>();
            qrManager.StopQRTracking();
        }

    public void resetPosition(GameObject go)
    {
        go.transform.position=defaultPosition.position;
        go.transform.rotation=defaultPosition.rotation;
        go.transform.localScale=defaultPosition.localScale;
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
                Destroy(go);
            }
        }

        void attachModelAsChild(){
            //attach model to calibrator
            GameObject model;
            model = GameObject.Find("1");
            //positions need to be the same first
            model.transform.position=modelCalibrator.transform.position;
            model.transform.rotation=modelCalibrator.transform.rotation;
            //model.transform.eulerAngles=modelCalibrator.transform.eulerAngles;
            model.transform.localScale=modelCalibrator.transform.localScale;

            model.transform.parent = modelCalibrator.transform;

        }
    }
