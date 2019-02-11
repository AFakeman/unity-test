using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public int dayDuration = 3000;
    public bool dayCycleEnabled, timeTrackingEnabled = true;

    private Light _light;
    private Gradient _gradient, _gradientDay;
    private GradientAlphaKey[] _gradientAlphaKeys;
    private GradientColorKey[] _gradientColorKeys;
    private int _ticksPassed;
    private bool _night;
    private int _dayspassed;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
        _gradient = new Gradient();

        _gradientAlphaKeys = new GradientAlphaKey[2];
        _gradientAlphaKeys[0].alpha = 1.0f;
        _gradientAlphaKeys[0].time = 0.0f;
        _gradientAlphaKeys[1].alpha = 0.0f;
        _gradientAlphaKeys[1].time = 1.0f;

        _gradientColorKeys = new GradientColorKey[3];
        _gradientColorKeys[0].color = Color.black;
        _gradientColorKeys[0].time = 0.0f;
        _gradientColorKeys[1].color = Color.white;
        _gradientColorKeys[1].time = 0.5f;
        _gradientColorKeys[2].color = Color.black;
        _gradientColorKeys[2].time = 1.0f;

        _gradient.SetKeys(_gradientColorKeys, _gradientAlphaKeys);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dayCycleEnabled)
        {
            _light.color = _gradient.Evaluate((float)((GetCurrentTime() / dayDuration))); //Set light leven on a gradient
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (Night() == true)
            {
                Debug.Log("Night");
            }
            else
            {
                Debug.Log("Day");
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            
        }
        if (timeTrackingEnabled)
        {
            _ticksPassed++; //Track amounts of ticks passed
        }
    }
    /// <summary>
    /// Returns current time in ticks
    /// </summary>
    /// <returns></returns>
    public float GetCurrentTime()
    {
        int time = _ticksPassed % dayDuration;
        return time;
    }
    public bool Night()
    {
        if ((GetCurrentTime() <= (dayDuration / 4)) || (GetCurrentTime() >= (dayDuration * 0.75)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
