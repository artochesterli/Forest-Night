﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoshH.UI;

public class ButtonAppearance : MonoBehaviour
{
    public ButtonType type;
    public ButtonStatus state;
    public bool Clickable;

    private bool Fading;

    private const float SelectedScale=1.1f;

    private const float MinimalAlpha = 0.3f;
    private const float MaximalAlpha = 1f;
    private const float FadingTime = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAppearance();
        ImageChange();
    }

    private void Init()
    {
        transform.Find("TextSelected").localScale = Vector3.one * SelectedScale;
        ChangeAppearance();
    }

    private void ChangeAppearance()
    {

        if (state == ButtonStatus.NotSelected)
        {
            GetComponent<Image>().enabled = false;
            transform.Find("TextNotSelected").GetComponent<Text>().enabled = true;
            transform.Find("TextSelected").GetComponent<Text>().enabled = false;

        }
        else if(state == ButtonStatus.Selected)
        {
            GetComponent<Image>().enabled = true;
            transform.Find("TextNotSelected").GetComponent<Text>().enabled = false;
            transform.Find("TextSelected").GetComponent<Text>().enabled = true;
            
        }
    }

    private void ImageChange()
    {
        if (Clickable)
        {
            if (Fading)
            {
                var Image = GetComponent<Image>();
                float CurrentAlpha = Image.color.a;
                Image.color = new Color(1, 1, 1, CurrentAlpha - (MaximalAlpha - MinimalAlpha) / FadingTime * Time.deltaTime);
                if (Image.color.a <= MinimalAlpha)
                {
                    Image.color = new Color(1, 1, 1, MinimalAlpha);
                    Fading = false;
                }
            }
            else
            {
                var Image = GetComponent<Image>();
                float CurrentAlpha = Image.color.a;
                Image.color = new Color(1, 1, 1, CurrentAlpha + (MaximalAlpha - MinimalAlpha) / FadingTime * Time.deltaTime);
                if (Image.color.a >= 1)
                {
                    Image.color = new Color(1, 1, 1, 1);
                    Fading = true;
                }
            }
        }
    }

}
