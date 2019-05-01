using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMoveImage : MonoBehaviour
{
    public List<Sprite> SpriteList;

    public GameObject Image;
    public GameObject BackImage;
    public Vector2 ImagePos;

    private const float ImageMoveTime = 0.2f;
    private const float height = 1080;

    private void Update()
    {
        
    }
    public void MoveImage(bool up, int index)
    {
        Image.GetComponent<Image>().sprite = BackImage.GetComponent<Image>().sprite;
        Image.GetComponent<RectTransform>().anchoredPosition = ImagePos;
        if (up)
        {
            BackImage.GetComponent<RectTransform>().anchoredPosition = ImagePos + Vector2.down * height;
        }
        else
        {
            BackImage.GetComponent<RectTransform>().anchoredPosition = ImagePos + Vector2.up * height;
        }
        BackImage.GetComponent<Image>().sprite = SpriteList[index];

        StopAllCoroutines();
        StartCoroutine(Move(up));
    }

    private IEnumerator Move(bool up)
    {
        float timecount = 0;
        Vector2 direction;
        if (up)
        {
            direction = Vector2.up;
        }
        else
        {
            direction = Vector2.down;
        }
        while (timecount < ImageMoveTime)
        {
            Image.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(ImagePos, ImagePos + direction * height, timecount / ImageMoveTime);
            BackImage.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(ImagePos - direction * height, ImagePos, timecount / ImageMoveTime);
            timecount += Time.deltaTime;
            yield return null;
        }
        Image.GetComponent<Image>().sprite = BackImage.GetComponent<Image>().sprite;
        Image.GetComponent<RectTransform>().anchoredPosition = ImagePos;
        BackImage.GetComponent<RectTransform>().anchoredPosition = ImagePos + direction * height;
    }
}
