using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

using EvoVerve.Credits;

public class TimeManager : MonoBehaviour 
{
    public double logOffTime;
    public double logInTime;
    public static TimeManager instance = null;

    private float secondDifference;

    private void Awake()
    {
        if (!File.Exists(Application.persistentDataPath + "/PlayerData.evoverve"))
        {
            Init();
        }
    }

    void Init()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        logInTime = GetLocalTime();

        if (logOffTime > 1)
        {
            secondDifference = CalculateSecsBetweenSessions();
        }
    }


    private void OnEnable()
    {
        GameManager.Loaded += LoadTime;
    }

    private void OnDisable()
    {
        GameManager.Loaded -= LoadTime;
    }

    public void LoadTime(PlayerData data)
    {
        logOffTime = data.logOffTime;
        Init();
    }

    public float CalculateSecsBetweenSessions()
    {
        return (float)(logInTime - logOffTime);
    }

    public float GetSecondDifference()
    {
        return secondDifference;
    }

	public static double NtpTime ()
    {
        try
        {
            byte[] ntpData = new byte[48];
     
            ntpData[0] = 0x1B;
     
            IPAddress[] addresses = Dns.GetHostEntry ("time.google.com").AddressList;
            Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
     
            socket.Connect (new IPEndPoint (addresses[0], 123));
            socket.ReceiveTimeout = 1000;
     
            socket.Send (ntpData);
            socket.Receive (ntpData);
            socket.Close ();
     
            ulong intc = (ulong) ntpData[40] << 24 | (ulong) ntpData[41] << 16 | (ulong) ntpData[42] << 8 | (ulong) ntpData[43];
            ulong frac = (ulong) ntpData[44] << 24 | (ulong) ntpData[45] << 16 | (ulong) ntpData[46] << 8 | (ulong) ntpData[47];
     
            return (double) (intc + (frac / 0x100000000L));
        }
        catch (Exception exception)
        {
            Debug.Log ("Could not get NTP time");
            Debug.Log (exception);
            return GetLocalTime();
        }
    }
 
    public static double GetLocalTime ()
    {
        double localTime = Convert.ToInt64(DateTime.Now.Subtract(new DateTime (1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
        //return DateTime.Now.Subtract(new DateTime (1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        return localTime;
    }
}
