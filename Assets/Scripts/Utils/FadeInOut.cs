using System;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public bool fadein = false;
    public bool fadeout = false;
    public float timeToFade;

    private void Update()
    {
        if (fadein == true)
        {
            if (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / timeToFade;
                if(canvasGroup.alpha >= 1)
                {
                    fadein = false;
                }
            }
        }
        if (fadeout == true)
        {
            if (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / timeToFade;
                if (canvasGroup.alpha <= 0)
                {
                    fadeout = false;
                }
            }
        }
    }
    
    public void fadeIn()
    {
        fadeout = false;
        fadein = true;
    }
    
    public void fadeOut()
    {
        fadein = false;
        fadeout = true;
    }
}
