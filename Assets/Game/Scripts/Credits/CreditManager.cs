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


        private void OnEnable()
        {
            ClickerManager.Tapped += IncrementCredits;
        }

        private void OnDisable()
        {
            ClickerManager.Tapped -= IncrementCredits;
        }


        void IncrementCredits()
        {
            credits++;
        }
    }
}
