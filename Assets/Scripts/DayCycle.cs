using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public int dayDuration = 3000;
    public float speed = 1f;
    public float maxIntensity = 2.5f;

    private Light _light;
    private bool _sundown;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_light.intensity > 0 && _sundown)
        {
            float intensity = _light.intensity;
            intensity -= 0.001f*speed;
            _light.intensity = intensity;
        }
        else if (_light.intensity <= 2.5f && _sundown == false)
        {
            float intensity = _light.intensity;
            intensity += 0.001f*speed;
            _light.intensity = intensity;
        }
        else if (_light.intensity <= 0 || _light.intensity > maxIntensity)
        {
            _sundown = !_sundown;
        }
        
    }
}
