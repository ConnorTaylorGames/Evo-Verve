using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvoVerve.Clicker;

namespace EvoVerve.Credits
{
    public class CreditManager : MonoBehaviour
    {
        public static CreditManager instance = null;

        private int credits;
        public int Credits { get { return credits; } set { credits = value; } }

        public bool hasEnoughCredits;

        public delegate void UpdateCreditUI();
        public static event UpdateCreditUI UpdateUI;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            ClickerManager.Tapped += IncrementCredits;
            CreditOverTimeParent.IncrementCredits += IncrementCredits;
            GameManager.Loaded += LoadCredits;
        }

        private void OnDisable()
        {
            ClickerManager.Tapped -= IncrementCredits;
            CreditOverTimeParent.IncrementCredits -= IncrementCredits;
            GameManager.Loaded -= LoadCredits;
        }


        void IncrementCredits(int incremementAmount)
        {
            credits += incremementAmount;
            UpdateUI();
        }

        public void SpendCredits(int creditAmount)
        {
            credits -= creditAmount;
        }

        public bool HasEnoughCredits(int creditAmount)
        {
            if ((credits - creditAmount) > 0)
            {
                hasEnoughCredits = true;
                return true;
            }
            else
            {
                hasEnoughCredits = false;
                UpdateUI();
                return false;
            }
        }

        private void LoadCredits(PlayerData data)
        {
            Credits = data.credits;
        }
    }
}
