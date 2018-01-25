using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using EvoVerve.Credits;
using EvoVerve.Clicker;

namespace EvoVerve.Ui
{
    public class UIManager : MonoBehaviour
    {

        public Text creditDisplayText;
        protected GameObject creditManager;
        public Button ExitScrollup;
        public Image quitBG;

        private bool menuOpened;
        private bool quitMenuOpened;

        private Animator menuSlideUp;
        private Animator quitCheck;


        // Use this for initialization
        void Start()
        {
            creditManager = GameObject.Find("CreditManager");
            menuOpened = false;
        }



        private void OnEnable()
        {
            CreditManager.UpdateUI += UpdateCreditDisplay;
            //ClickerManager.TappedUI += UpdateCreditDisplay;
            GameManager.Loaded += UpdateCreditDisplay;
        }

        private void OnDisable()
        {
            CreditManager.UpdateUI -= UpdateCreditDisplay;
            //ClickerManager.TappedUI -= UpdateCreditDisplay;
            GameManager.Loaded -= UpdateCreditDisplay;

        }


        // Update is called once per frame
        void Update()
        {

        }


        void UpdateCreditDisplay()
        {
            if (creditManager)
            {
                creditDisplayText.text = creditManager.GetComponent<CreditManager>().Credits.ToString();
            }
            else
            {
                Debug.Log("No Credit manager found");
            }
        }

        //
        //
        //Menu
        //
        //

        public void PlayMenuScrollUp()
        {
            if (ExitScrollup)
            {
                menuSlideUp = ExitScrollup.GetComponent<Animator>();

                if (!menuOpened)
                {
                    menuSlideUp.Play("MenuSlideUp");
                    menuOpened = true;
                }
                else
                {
                    menuSlideUp.Play("MenuSlideDown");
                    menuOpened = false;

                }
            }

        }

        //
        //
        //Quit check
        //
        //
        public void QuitCheck()
        {
            if (quitBG)
            {
                quitCheck = quitBG.GetComponent<Animator>();

                if (!quitMenuOpened)
                {
                    quitCheck.Play("QuitCheckAnim");
                    quitMenuOpened = true;
                }
                else
                {
                    quitCheck.Play("QuitCheckAnimReverse");
                    quitMenuOpened = false;

                }
            }
        }

        public void Exit()
        {
            DataManager.Save();
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }

        public void CancelQuit()
        {
            if (quitBG)
            {
                quitCheck = quitBG.GetComponent<Animator>();
                quitCheck.Play("QuitCheckAnimReverse");
                quitMenuOpened = false;
            }
        }
    }
}
