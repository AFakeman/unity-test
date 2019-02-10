using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public int dayDuration = 3000;
    public float speed = 1f;
    public float maxIntensity = 2.5f;

    private Light _light;
    private int _ticksPassed;
    private float _tickCoef, _intensityGain;
    private bool _sundown = true;
    private int _dayspassed;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
        _tickCoef = 720f / dayDuration; //there are 720 minutes in 12 hours, so half a full day

        _intensityGain = maxIntensity / (dayDuration);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_sundown  && _light.intensity >= 0)
        {
            float intensity = _light.intensity;
            intensity -= _intensityGain*speed;
            _light.intensity = intensity;
        }
        else if (!_sundown  && _light.intensity <= maxIntensity)
        {
            float intensity = _light.intensity;
            intensity += _intensityGain * speed;
            _light.intensity = intensity;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            int timeinMinutes = GetCurrentTime();
            int hours = timeinMinutes / 60;
            if (hours == 0)
            {
                hours = 12;
            }
            int minutes = timeinMinutes % 60;
            Debug.Log(hours + ":" + minutes);
            Debug.Log(_tickCoef + ":" + _ticksPassed + ":"+_dayspassed);
        }
        
        if (_ticksPassed % dayDuration == 0)
        {
            _sundown = !_sundown;
            if (_ticksPassed % (dayDuration*2) == 0)
            {
                _dayspassed++;
            }
        }

        _ticksPassed++;


    }
    /// <summary>
    /// Returns current time in minutes in 12 hour format
    /// </summary>
    /// <returns></returns>
    public int GetCurrentTime()
    {
        //return (int)(_light.intensity / (0.001 * speed));
        int time = _ticksPassed % dayDuration;
        int timeInMinutes = (int)((float)time * _tickCoef);
        return timeInMinutes;
    }
}
