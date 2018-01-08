using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using EvoVerve.Credits;
using EvoVerve.Clicker;

namespace EvoVerve.Ui
{
    public class UIManager : MonoBehaviour
    {

        public Text creditDisplayText;
        protected GameObject creditManager;

        // Use this for initialization
        void Start()
        {
            creditManager = GameObject.Find("CreditManager");
        }

        private void OnEnable()
        {
            ClickerManager.Tapped += UpdateCreditDisplay;
        }

        private void OnDisable()
        {
            ClickerManager.Tapped -= UpdateCreditDisplay;
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
    }
}
