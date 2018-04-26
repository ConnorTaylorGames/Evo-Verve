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
        }

        private void OnEnable()
        {
            ClickerManager.Tapped += IncrementCredits;
            ObjectManager.IncrementCredits += IncrementCredits;
            GameManager.Loaded += LoadCredits;
            CPSManager.cpsPopulated += AddOfflineCredits;

        }

        private void OnDisable()
        {
            ClickerManager.Tapped -= IncrementCredits;
            ObjectManager.IncrementCredits -= IncrementCredits;
            GameManager.Loaded -= LoadCredits;
            CPSManager.cpsPopulated -= AddOfflineCredits;

        }


        void IncrementCredits(int incremementAmount)
        {
            credits += incremementAmount;
            UpdateUI();
        }

        public void SpendCredits(int creditAmount)
        {
            credits -= creditAmount;
            UpdateUI();
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


        public void AddOfflineCredits()
        {
            
            if (TimeManager.instance != null)
            {

                if (TimeManager.instance.GetSecondDifference() > 2629746)
                {
                    //More than a month
                    double secondMultiplier = 15770000 * 0.5;
                    int tempCredits = CPSManager.GetCPS() * (int)secondMultiplier;

                }
                else if(TimeManager.instance.GetSecondDifference() > 5259492)
                {
                    //More than 2 months
                    double secondMultiplier = 15770000 * 0.4;
                    int tempCredits = CPSManager.GetCPS() * (int)secondMultiplier;
                    
                }
                else if (TimeManager.instance.GetSecondDifference() > 788923)
                {
                    //More than 3 months
                    double secondMultiplier = 15770000 * 0.3;
                    int tempCredits = CPSManager.GetCPS() * (int)secondMultiplier;

                }
                else if (TimeManager.instance.GetSecondDifference() > 10518984)
                {
                    //More than 4 months
                    double secondMultiplier = 15770000 * 0.2;
                    int tempCredits = CPSManager.GetCPS() * (int)secondMultiplier;

                }
                else if (TimeManager.instance.GetSecondDifference() > 13148730)
                {
                    //More than 5 months
                    double secondMultiplier = 15770000 * 0.1;
                    int tempCredits = CPSManager.GetCPS() * (int)secondMultiplier;

                }
                else
                {
                    float secondMultiplier = TimeManager.instance.GetSecondDifference() * 0.6f;
                    int tempCredits = CPSManager.GetCPS() * (int)secondMultiplier;
                    //Debug.Log(tempCredits);
                    Credits += tempCredits;

                }
            }
        }
    }
}
