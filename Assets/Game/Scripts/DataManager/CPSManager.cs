using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvoVerve.Credits;

public class CPSManager
{
    private static int cps;
    public static int CPS { get { return cps; } set { cps = value; } }

    public delegate void CPSAction();
    public static event CPSAction cpsAction;

    public delegate void CPSPopulated();
    public static event CPSPopulated cpsPopulated;

    public static  int GetCPS()
    {
        return CPS;
    }

    public static void SetCPS(int newCPS)
    {
        CPS = newCPS;

        cpsAction();
        cpsPopulated();
        
    }

    public static void ResetCPS()
    {
        CPS = 0;
    }

    public static void AddToCPS(int newAmount)
    {
        CPS += newAmount;

        if (cpsAction != null)
        {
            cpsAction();
        }
    }

    public static void RemoveFromCPS(int amount)
    {
        CPS -= amount;

        if (cpsAction != null)
        {
            cpsAction();
        }
    }
}
