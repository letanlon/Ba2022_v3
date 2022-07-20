using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToScene : MonoBehaviour
{

    public void SwitchToSessionScene()
    {
        SceneManager.LoadScene("PUN testscene 2", LoadSceneMode.Single);
    }
}
