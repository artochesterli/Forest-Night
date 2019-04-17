using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoshH.UI;

public class ButtonAppearance : MonoBehaviour
{
    public ButtonType type;
    public ButtonStatus state;

    private bool Fading;

    private const float SelectedScale=1.1f;
    private const float ClickScale = 1.1f;
    private const float ClickTime = 0.05f;

    private const float MinimalAlpha = 0.3f;
    private const float FadingTime = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("TextSelected").localScale = Vector3.one * SelectedScale;
        transform.Find("TextClick").localScale = Vector3.one * ClickScale;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAppearance();
        ImageChange();
    }

    private void ChangeAppearance()
    {
        if (state == ButtonStatus.NotSelected)
        {
            GetComponent<Image>().enabled = false;
            transform.Find("TextNotSelected").GetComponent<Text>().enabled = true;
            transform.Find("TextSelected").GetComponent<Text>().enabled = false;
            transform.Find("TextClick").GetComponent<Text>().enabled = false;

        }
        else if(state == ButtonStatus.Selected)
        {
            GetComponent<Image>().enabled = true;
            transform.Find("TextNotSelected").GetComponent<Text>().enabled = false;
            transform.Find("TextSelected").GetComponent<Text>().enabled = true;
            transform.Find("TextClick").GetComponent<Text>().enabled = false;
            
        }
        else if(state == ButtonStatus.Click)
        {
            GetComponent<Image>().enabled = true;
            transform.Find("TextNotSelected").GetComponent<Text>().enabled = false;
            transform.Find("TextSelected").GetComponent<Text>().enabled = false;
            transform.Find("TextClick").GetComponent<Text>().enabled = true;
        }
    }

    private void ImageChange()
    {
        if (state != ButtonStatus.Click)
        {
            if (Fading)
            {
                var Image = GetComponent<Image>();
                float CurrentAlpha = Image.color.a;
                Image.color = new Color(1, 1, 1, CurrentAlpha - (1 - MinimalAlpha) / FadingTime * Time.deltaTime);
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
                Image.color = new Color(1, 1, 1, CurrentAlpha + (1 - MinimalAlpha) / FadingTime * Time.deltaTime);
                if (Image.color.a >= 1)
                {
                    Image.color = new Color(1, 1, 1, 1);
                    Fading = true;
                }
            }
        }
    }

    public IEnumerator Clicking()
    {
        state = ButtonStatus.Click;
        GetComponent<Image>().color = Color.white;
        //GetComponent<Image>().color=new Color(1,1,1,0);
        //GetComponent<Image>().sprite = Resources.Load("Sprite/UI/ClickEffect", typeof(Sprite)) as Sprite;
        yield return new WaitForSeconds(ClickTime);
        //GetComponent<Image>().sprite = Resources.Load("Sprite/UI/SelectionEffect", typeof(Sprite)) as Sprite;
        state = ButtonStatus.Selected;
        EventManager.instance.Fire(new FinishClick(type));
    }
}
