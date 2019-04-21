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

    public float ImageFlipTime;

    private Dictionary<int, Sprite> IndexToSprite;
    private Dictionary<int, GameObject> IndexToButton;
    private int SelectedMenu;

    private bool Active;

    private bool AtFront;
    private bool ToNext;
    private float RotationAngle;
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

        AtFront = true;
        RotationAngle = 0;
        BackObjectsImage.transform.rotation = Quaternion.Euler(0, RotationAngle + 180, 0);
        ObjectsImage.GetComponent<Image>().sprite = IndexToSprite[SelectedMenu];

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
        ObjectsImage.transform.rotation = Quaternion.Euler(0, RotationAngle, 0);
        BackObjectsImage.transform.rotation = Quaternion.Euler(0, RotationAngle + 180, 0);
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

                SetFlip(true);
                
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SelectedMenu = (SelectedMenu + 1) % IndexToButton.Count;

                SetFlip(false);

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
        if (AtFront)
        {
            ObjectsImage.GetComponent<Image>().color = Color.white;
        }
        else
        {
            BackObjectsImage.GetComponent<Image>().color = Color.white;
        }
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

    private void SetFlip(bool up)
    {
        AtFront = !AtFront;
        if (!AtFront)
        {
            RotationAngle = 0;
        }
        else
        {
            if (up)
            {
                RotationAngle = -180;
            }
            else
            {
                RotationAngle = 180;
            }
        }
        StopAllCoroutines();
        StartCoroutine(Flip(false));
        if (AtFront)
        {
            ObjectsImage.GetComponent<Image>().sprite = IndexToSprite[SelectedMenu];
        }
        else
        {
            BackObjectsImage.GetComponent<Image>().sprite = IndexToSprite[SelectedMenu];
        }
    }

    private IEnumerator Flip(bool AngleMinus)
    {
        float timecount = 0;
        while (timecount < ImageFlipTime)
        {
            if (AngleMinus)
            {
                RotationAngle -= 180 / ImageFlipTime * Time.deltaTime;
                if (AtFront)
                {
                    if (RotationAngle < 90)
                    {
                        ObjectsImage.GetComponent<Image>().color = Color.white;
                        BackObjectsImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    }
                }
                else
                {
                    if (RotationAngle < -90)
                    {
                        BackObjectsImage.GetComponent<Image>().color = Color.white;
                        ObjectsImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    }
                }
            }
            else
            {
                RotationAngle += 180 / ImageFlipTime * Time.deltaTime;
                if (AtFront)
                {
                    if (RotationAngle > -90)
                    {
                        ObjectsImage.GetComponent<Image>().color = Color.white;
                        BackObjectsImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    }
                }
                else
                {
                    if (RotationAngle > 90)
                    {
                        BackObjectsImage.GetComponent<Image>().color = Color.white;
                        ObjectsImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    }
                }
            }
            timecount += Time.deltaTime;
            yield return null;
        }
        if (AtFront)
        {
            RotationAngle = 0;
        }
        else
        {
            if (AngleMinus)
            {
                RotationAngle = -180;
            }
            else
            {
                RotationAngle = 180;
            }
        }
    }
}
