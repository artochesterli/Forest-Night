using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIcon : MonoBehaviour
{
    public Sprite ControllerSprite;
    public Sprite KeyboardSprite;
    public Vector2 ControllerSize;
    public Vector2 KeyboardSize;

    // Start is called before the first frame update
    void Start()
    {
        SetAppearance();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        SetAppearance();
    }
    
    private void SetAppearance()
    {
        RectTransform rt = GetComponent<RectTransform>();
        if (ControllerManager.MainCharacterJoystick != null)
        {
            GetComponent<Image>().sprite = ControllerSprite;
            rt.sizeDelta = ControllerSize;
        }
        else
        {
            GetComponent<Image>().sprite = KeyboardSprite;
            rt.sizeDelta = KeyboardSize;
        }
    }
}
