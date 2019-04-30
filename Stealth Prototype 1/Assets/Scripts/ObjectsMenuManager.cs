using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsMenuManager : MonoBehaviour
{
    public GameObject ObjectsImage;
    public GameObject BackObjectsImage;

    public GameObject VinesButton;
    public GameObject PlatformButton;
    public GameObject MirrorButton;

    public Sprite VinesImage;
    public Sprite PlatformImage;
    public Sprite MirrorImage;

    public float ImageMoveTime;
    public Vector2 ImagePos;

    private Dictionary<int, Sprite> IndexToSprite;
    private Dictionary<int, GameObject> IndexToButton;
    private int SelectedMenu;

    private bool Active;

    private const float height = 1080;
    // Start is called before the first frame update
    void Start()
    {
        IndexToButton = new Dictionary<int, GameObject>();
        IndexToButton.Add(0, VinesButton);
        IndexToButton.Add(1, PlatformButton);
        IndexToButton.Add(2, MirrorButton);

        IndexToSprite = new Dictionary<int, Sprite>();
        IndexToSprite.Add(0, VinesImage);
        IndexToSprite.Add(1, PlatformImage);
        IndexToSprite.Add(2, MirrorImage);
        SelectedMenu = 0;

        ObjectsImage.GetComponent<Image>().sprite = IndexToSprite[SelectedMenu];
        BackObjectsImage.GetComponent<Image>().sprite = IndexToSprite[SelectedMenu];

        EventManager.instance.AddHandler<EnterObjectsMenu>(OnEnterObjectsMenu);
        EventManager.instance.AddHandler<ExitObjectsMenu>(OnExitObjectsMenu);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterObjectsMenu>(OnEnterObjectsMenu);
        EventManager.instance.RemoveHandler<ExitObjectsMenu>(OnExitObjectsMenu);
    }
    // Update is called once per frame
    void Update()
    {
        SetMenuState();
        CheckInput();
    }

    private void SetMenuState()
    {
        if (Active)
        {
            for (int i = 0; i < IndexToButton.Count; i++)
            {
                if (SelectedMenu == i)
                {
                    IndexToButton[i].GetComponent<ButtonAppearance>().state = ButtonStatus.Selected;
                }
                else
                {
                    IndexToButton[i].GetComponent<ButtonAppearance>().state = ButtonStatus.NotSelected;
                }
            }
        }
    }

    private void CheckInput()
    {
        if (Active)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (SelectedMenu - 1 < 0)
                {
                    SelectedMenu += IndexToButton.Count;
                }
                SelectedMenu = (SelectedMenu - 1) % IndexToButton.Count;

                MoveImage(true);
                
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SelectedMenu = (SelectedMenu + 1) % IndexToButton.Count;

                MoveImage(false);

                return;
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                EventManager.instance.Fire(new EnterMainHelpMenu());
                EventManager.instance.Fire(new ExitObjectsMenu());
            }
        }
    }

    private void OnEnterObjectsMenu(EnterObjectsMenu E)
    {
        Active = true;
        ObjectsImage.GetComponent<Image>().color = Color.white;
        BackObjectsImage.GetComponent<Image>().color = Color.white;
        BackObjectsImage.GetComponent<RectTransform>().anchoredPosition = ObjectsImage.GetComponent<RectTransform>().anchoredPosition + Vector2.up * height;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void OnExitObjectsMenu(ExitObjectsMenu E)
    {
        Active = false;
        ObjectsImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        BackObjectsImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void MoveImage(bool up)
    {

        ObjectsImage.GetComponent<Image>().sprite = ObjectsImage.GetComponent<Image>().sprite;
        ObjectsImage.GetComponent<RectTransform>().anchoredPosition = ImagePos;
        if (up)
        {
            BackObjectsImage.GetComponent<RectTransform>().anchoredPosition = ImagePos + Vector2.down * height;
        }
        else
        {
            BackObjectsImage.GetComponent<RectTransform>().anchoredPosition = ImagePos + Vector2.up * height;
        }
        BackObjectsImage.GetComponent<Image>().sprite = IndexToSprite[SelectedMenu];

        StopAllCoroutines();
        StartCoroutine(Move(up));
    }

    private IEnumerator Move(bool up)
    {
        float timecount = 0;
        float Speed = height / ImageMoveTime;
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
            ObjectsImage.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(ImagePos, ImagePos + direction * height, timecount / ImageMoveTime);
            BackObjectsImage.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(ImagePos - direction * height, ImagePos, timecount / ImageMoveTime);
            timecount += Time.deltaTime;
            yield return null;
        }
        ObjectsImage.GetComponent<Image>().sprite = BackObjectsImage.GetComponent<Image>().sprite;
        ObjectsImage.GetComponent<RectTransform>().anchoredPosition = ImagePos;
        BackObjectsImage.GetComponent<RectTransform>().anchoredPosition = ImagePos + direction * height;
    }
}
