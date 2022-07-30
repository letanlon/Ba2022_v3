using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class MenuLoginByUserIdLogic : MonoBehaviour
{
    private string userIdInput;
    public GameObject inputPreviewTMP;

    //public TMP_Settings textInput;
    public TextMeshPro textInput;

    // Start is called before the first frame update
    void Start()
    {
        pressedOne();
    }

    // Update is called once per frame

     public void pressedEnter()
    {
        //action??
    }

     public void pressedDelete()
    {
        userIdInput="";
        updatePreviewText();
    }

        public void pressedOne()
    {
        userIdInput=userIdInput+"1";
        updatePreviewText();
    }

    void updatePreviewText()
    {
        inputPreviewTMP = GameObject.FindGameObjectWithTag("InputPreview");
        textInput = inputPreviewTMP.GetComponent<TextMeshPro>();
        textInput.text=userIdInput;
    }
    
}
