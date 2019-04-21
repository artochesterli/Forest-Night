using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesMenuManager : MonoBehaviour
{
    public GameObject AbilityImage;
    public GameObject BackAbilityImage;

    public GameObject SlashButton;
    public GameObject GlideButton;
    public GameObject ShootButton;
    public GameObject SuperDashButton;
    public GameObject FadeButton;

    public Sprite SlashImage;
    public Sprite GlideImage;
    public Sprite ShootImage;
    public Sprite SuperDashImage;
    public Sprite FadeImage;

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
        IndexToButton.Add(0, SlashButton);
        IndexToButton.Add(1, GlideButton);
        IndexToButton.Add(2, ShootButton);
        IndexToButton.Add(3, SuperDashButton);
        IndexToButton.Add(4, FadeButton);

        IndexToSprite = new Dictionary<int, Sprite>();
        IndexToSprite.Add(0, SlashImage);
        IndexToSprite.Add(1, GlideImage);
        IndexToSprite.Add(2, ShootImage);
        IndexToSprite.Add(3, SuperDashImage);
        IndexToSprite.Add(4, FadeImage);

        SelectedMenu = 0;

        AtFront = true;
        RotationAngle = 0;
        BackAbilityImage.transform.rotation = Quaternion.Euler(0, RotationAngle + 180, 0);
        AbilityImage.GetComponent<Image>().sprite = IndexToSprite[SelectedMenu];

        EventManager.instance.AddHandler<EnterAbilitiesMenu>(OnEnterAbilitiesMenu);
        EventManager.instance.AddHandler<ExitAbilitiesMenu>(OnExitAbilitiesMenu);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterAbilitiesMenu>(OnEnterAbilitiesMenu);
        EventManager.instance.RemoveHandler<ExitAbilitiesMenu>(OnExitAbilitiesMenu);
    }

    // Update is called once per frame
    void Update()
    {
        SetMenuState();
        CheckInput();
        AbilityImage.transform.rotation = Quaternion.Euler(0, RotationAngle, 0);
        BackAbilityImage.transform.rotation = Quaternion.Euler(0, RotationAngle + 180, 0);
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
                EventManager.instance.Fire(new ExitAbilitiesMenu());
                EventManager.instance.Fire(new EnterMainHelpMenu());
            }
        }
    }

    private void OnEnterAbilitiesMenu(EnterAbilitiesMenu E)
    {
        Active = true;
        if (AtFront)
        {
            AbilityImage.GetComponent<Image>().color = Color.white;
        }
        else
        {
            BackAbilityImage.GetComponent<Image>().color = Color.white;
        }
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void OnExitAbilitiesMenu(ExitAbilitiesMenu E)
    {
        Active = false;
        AbilityImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        BackAbilityImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
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
            AbilityImage.GetComponent<Image>().sprite = IndexToSprite[SelectedMenu];
        }
        else
        {
            BackAbilityImage.GetComponent<Image>().sprite = IndexToSprite[SelectedMenu];
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
                        AbilityImage.GetComponent<Image>().color = Color.white;
                        BackAbilityImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    }
                }
                else
                {
                    if(RotationAngle < -90)
                    {
                        BackAbilityImage.GetComponent<Image>().color = Color.white;
                        AbilityImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
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
                        AbilityImage.GetComponent<Image>().color = Color.white;
                        BackAbilityImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    }
                }
                else
                {
                    if (RotationAngle > 90)
                    {
                        BackAbilityImage.GetComponent<Image>().color = Color.white;
                        AbilityImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
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
