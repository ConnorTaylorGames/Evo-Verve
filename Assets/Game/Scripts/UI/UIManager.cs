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
        public GameObject shopHandler;
        public Button ExitScrollup;
        public Image quitBG;
        public Image shop;
        public Image meteorImage;
        public bool meteorSelected;

        private bool menuOpened;
        private bool quitMenuOpened;
        private bool isShopActivated;
  

        private Animator menuSlideUp;
        private Animator quitCheck;
        private Animator shopAnim;


        // Use this for initialization
        void Start()
        {
            creditManager = GameObject.Find("CreditManager");
            menuOpened = false;
            isShopActivated = false;
            meteorSelected = false;
        }



        private void OnEnable()
        {
            CreditManager.UpdateUI += UpdateCreditDisplay;
            //ClickerManager.TappedUI += UpdateCreditDisplay;
            GameManager.Loaded += LoadInitialCreditsValue;
        }

        private void OnDisable()
        {
            CreditManager.UpdateUI -= UpdateCreditDisplay;
            //ClickerManager.TappedUI -= UpdateCreditDisplay;
            GameManager.Loaded -= LoadInitialCreditsValue;

        }


        public void PlayShopOpen()
        {
            if (shop)
            {
                shopAnim = shop.GetComponent<Animator>();

                if (!isShopActivated)
                {
                    shopAnim.Play("ShopActivate");
                    CloseAllOpenedUI();
                    isShopActivated = true;
                    if (shop.GetComponent<ShopHandler>().HasSelectedItem())
                    {
                        shop.GetComponent<ShopHandler>().toolTip.SetActive(true);
                    }
                    else
                    {
                        shop.GetComponent<ShopHandler>().toolTip.SetActive(false);
                    }
                }
                else
                {
                    shopAnim.Play("ShopActivateReverse");
                    isShopActivated = false;
                    if (shop.GetComponent<ShopHandler>().toolTip.activeSelf)
                    {
                        shop.GetComponent<ShopHandler>().toolTip.SetActive(false);
                    }
                }
            }
            
        }

        void LoadInitialCreditsValue(PlayerData data)
        {
            creditDisplayText.text = data.credits.ToString();
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
                    CloseAllOpenedUI();
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
                    CloseAllOpenedUI();
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

        public void EnableMeteors()
        {
            meteorSelected = !meteorSelected;
            if (meteorSelected)
            {
                if (shopHandler.GetComponent<ShopHandler>().HasSelectedItem())
                {
                    shopHandler.GetComponent<ShopHandler>().ClearSelectedItem();
                }
                meteorImage.color = Color.red;
            }
            else
            {
                meteorImage.color = Color.white;
            }
        }


        private void CloseAllOpenedUI()
        {
            if (quitMenuOpened)
            {
                quitCheck.Play("QuitCheckAnimReverse");
                quitMenuOpened = false;
            }

            if (menuOpened)
            {
                menuSlideUp.Play("MenuSlideDown");
                menuOpened = false;
            }

            if (isShopActivated)
            {
                shopAnim.Play("ShopActivateReverse");
                isShopActivated = false;
            }
        }
    }
}
