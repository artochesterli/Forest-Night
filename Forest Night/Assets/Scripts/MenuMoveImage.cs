using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMoveImage : MonoBehaviour
{
    public List<Sprite> ConConSpriteList;
    public List<Sprite> ConKeySpriteList;
    public List<Sprite> KeyConSpriteList;
    public List<Sprite> KeyKeySpriteList;

    public GameObject Image;

    public int ImageIndex;

    private const float ImageShowTime = 0.2f;

    private void Update()
    {
        SetSprite();
    }


    public void SetSprite()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            if (ControllerManager.FairyJoystick != null)
            {
                Image.GetComponent<Image>().sprite = ConConSpriteList[ImageIndex];
            }
            else
            {
                Image.GetComponent<Image>().sprite = ConKeySpriteList[ImageIndex];
            }
        }
        else
        {
            if (ControllerManager.FairyJoystick != null)
            {
                Image.GetComponent<Image>().sprite = KeyConSpriteList[ImageIndex];
            }
            else
            {
                Image.GetComponent<Image>().sprite = KeyKeySpriteList[ImageIndex];
            }
        }
    }

    public void MoveImage(int index)
    {
        ImageIndex = index;

        Image.GetComponent<Image>().color = Color.white;

        SetSprite();

        StopAllCoroutines();
        StartCoroutine(ShowImage());
    }

    private IEnumerator ShowImage()
    {
        float timecount = 0;

        Image.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        while (timecount < ImageShowTime)
        {
            timecount += Time.deltaTime;
            Image.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, timecount / ImageShowTime);
            yield return null;
        }

    }
}
