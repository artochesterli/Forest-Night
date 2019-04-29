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

    public float ImageMoveTime;
    public Vector2 ImagePos;

    private Dictionary<int, Sprite> IndexToSprite;
    private Dictionary<int, GameObject> IndexToButton;
    private int SelectedMenu;

    private bool Active;

    private float RotationAngle;

    private const float height = 1080;
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
                EventManager.instance.Fire(new ExitAbilitiesMenu());
                EventManager.instance.Fire(new EnterMainHelpMenu());
            }
        }
    }

    private void OnEnterAbilitiesMenu(EnterAbilitiesMenu E)
    {
        Active = true;
        AbilityImage.GetComponent<Image>().color = Color.white;
        BackAbilityImage.GetComponent<Image>().color = Color.white;
        BackAbilityImage.GetComponent<RectTransform>().anchoredPosition = AbilityImage.GetComponent<RectTransform>().anchoredPosition + Vector2.up * height;
        foreach (Transform child in transform)
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

    private void MoveImage(bool up)
    {

        AbilityImage.GetComponent<Image>().sprite = BackAbilityImage.GetComponent<Image>().sprite;
        AbilityImage.GetComponent<RectTransform>().anchoredPosition = ImagePos;
        if (up)
        {
            BackAbilityImage.GetComponent<RectTransform>().anchoredPosition = ImagePos + Vector2.down * height;
        }
        else
        {
            BackAbilityImage.GetComponent<RectTransform>().anchoredPosition = ImagePos + Vector2.up * height;
        }
        BackAbilityImage.GetComponent<Image>().sprite = IndexToSprite[SelectedMenu];

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
            AbilityImage.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(ImagePos, ImagePos + direction * height, timecount / ImageMoveTime);
            BackAbilityImage.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(ImagePos - direction * height, ImagePos, timecount / ImageMoveTime);
            timecount += Time.deltaTime;
            yield return null;
        }
        AbilityImage.GetComponent<Image>().sprite= BackAbilityImage.GetComponent<Image>().sprite;
        AbilityImage.GetComponent<RectTransform>().anchoredPosition = ImagePos;
        BackAbilityImage.GetComponent<RectTransform>().anchoredPosition = ImagePos + direction*height;
    }
}
