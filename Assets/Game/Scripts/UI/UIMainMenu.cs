using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;



public class UIMainMenu : MonoBehaviour {

    public GameObject deleteSection;
    private bool deleteMenuOpened;
    private bool optionOpened;

    private Animator deleteCheck;
    private Animator optionsOpen;

    public GameObject playButton;
    public GameObject optionsButton;
    public GameObject deleteButton;


    public void Play()
    {
        SceneManager.LoadScene("PrototypeScene_v1", LoadSceneMode.Single);    
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenFeedBack()
    {
        Application.OpenURL("https://goo.gl/forms/w45MaKmSlAeGT6s42");
    }

    public void DeleteSavedGame()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.evoverve"))
        {
            File.Delete(Application.persistentDataPath + "/PlayerData.evoverve");
            CPSManager.ResetCPS();
            CancelDelete();
        }
        else
        {
            Debug.Log("NO FILE FOUND");
            CancelDelete();
        }
    }

    public void CancelDelete()
    {
        if (deleteSection)
        {
            deleteCheck = deleteSection.GetComponent<Animator>();
            deleteCheck.Play("QuitCheckAnimReverse");
            UnHideAllUI();
            deleteMenuOpened = false;
        }
    }

    public void DeleteCheck()
    {
        if (deleteSection)
        {
            deleteCheck = deleteSection.GetComponent<Animator>();

            if (!deleteMenuOpened)
            {

                deleteCheck.Play("QuitCheckAnim");
                HideAllUI();
                deleteMenuOpened = true;


            }
            else
            {
                deleteCheck.Play("QuitCheckAnimReverse");
                UnHideAllUI();
                deleteMenuOpened = false;
            }
        }
    }

    public void OptionsOpen()
    {
        if (optionsButton != null)
        {
            optionsOpen = deleteButton.GetComponent<Animator>();
            if (!optionOpened)
            {
                optionsOpen.Play("MenuOptionsAnim");
                optionOpened = true;
            }
            else
            {
                optionsOpen.Play("MenuOptionsAnimReverse");
                optionOpened = false;
            }
        }
    }

    private void HideAllUI()
    {
        if (deleteMenuOpened)
        {
            deleteCheck.Play("QuitCheckAnimReverse");
            deleteMenuOpened = false;
        }

        if (playButton != null)
        {
            playButton.SetActive(false);
        }

    }

    private void UnHideAllUI()
    {
        if (playButton != null)
        {
            playButton.SetActive(true);
        }
    }
}
