using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIMainMenu : MonoBehaviour {

    public void Play()
    {
        SceneManager.LoadScene("PrototypeScene_v1", LoadSceneMode.Single);    
    }

    public void Exit()
    {
        Application.Quit();
    }


}
