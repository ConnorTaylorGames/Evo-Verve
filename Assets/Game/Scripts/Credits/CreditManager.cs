using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvoVerve.Clicker;

namespace EvoVerve.Credits
{
    public class CreditManager : MonoBehaviour
    {
        private int credits;
        public int Credits { get { return credits; } set { credits = value; } }

        public delegate void UpdateCreditUI();
        public static event UpdateCreditUI UpdateUI;

        private void OnEnable()
        {
            ClickerManager.Tapped += IncrementCredits;
            CreditOverTimeParent.IncrementCredits += IncrementCredits;
        }

        private void OnDisable()
        {
            ClickerManager.Tapped -= IncrementCredits;
            CreditOverTimeParent.IncrementCredits -= IncrementCredits;

        }


        void IncrementCredits(int incremementAmount)
        {
            credits+= incremementAmount;
            UpdateUI();
        }
    }
}
