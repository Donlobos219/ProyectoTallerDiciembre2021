using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FlickingLight : MonoBehaviour
{
    public Light mylight;
    

    public float minTime;
    public float maxTime;
    public float timer;
    public AudioSource lightSFX;

    public float bloom = 20f;


    // Start is called before the first frame update
    void Start()
    {
        
        timer = Random.Range(minTime, maxTime);
        
        lightSFX.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
        FlickerLight(); 
    }

    void FlickerLight()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
            //rend.sharedMaterial = material[1];

        if (timer<=0)
        {
            //rend.sharedMaterial = material[2];
            mylight.enabled = !mylight.enabled;
            timer = Random.Range(minTime, maxTime);
        }
    }
}
