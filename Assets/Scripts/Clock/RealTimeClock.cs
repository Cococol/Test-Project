using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RealTimeClock : MonoBehaviour
{
    //text is used to show the time, even though it's private it will show in the inspector
    //because it has the serializefield above it
    [SerializeField]
    private Text currentTime;

    //toggle to switch between local and utc time
    [SerializeField]
    private Toggle changeCurrentTime;

    private bool showDifferentTime = false;

    //the variables to get the time from the computer system
    //you can only use these when you have "Using System;" in your project
    private DateTime localTime;
    private DateTime utcTime;

    void Update()
    {
        //sets the time fom the computer to the variables localTime and utcTime
        localTime = DateTime.Now;
        utcTime = DateTime.UtcNow;

        //checks which time has to be shown.
        if (showDifferentTime)
        {
            //the "HH:mm:ss" makes sure it only shows the hours, minutes and seconds.
            //if you do not have that it will also show the date.
            currentTime.text = utcTime.ToString("HH:mm:ss");
        }
        else if (!showDifferentTime)
        {
            //the "HH:mm:ss" makes sure it only shows the hours, minutes and seconds.
            //if you do not have that it will also show the date.
            currentTime.text = localTime.ToString("HH:mm:ss");
        }
    }
    //is called as soon as the toggle is changed in the scene
    public void ChangeTimeZone()
    {
        //checks if the toggle is on or off
        if(changeCurrentTime.isOn == true)
        {
            showDifferentTime = true;
        }
        else if(changeCurrentTime.isOn == false)
        {
            showDifferentTime = false;
        }
    }

}
