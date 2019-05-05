using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Totem : MonoBehaviour
{
    public float Path_Length;

    private float Path_Collider_Width = 0.3f;
    private float Path_Vertical_Offset = 0;
    private float PathOpenTime = 0.25f;

    private const float LightAppearTime = 0.5f;
    private const int PathSpritePerMeter = 2;
    private const int PathSpriteNumber = 4;

    private bool Activated;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Slash")&&!Activated)
        {
            StartCoroutine(CreatePath());
        }
    }

    private IEnumerator CreatePath()
    {
        Activated = true;
        yield return StartCoroutine(LightAppear());
        GameObject Path = transform.Find("Path").gameObject;
        Path.transform.Find("Path_End").GetComponent<BoxCollider2D>().enabled = true;
        Path.GetComponent<BoxCollider2D>().enabled = true;
        Path.GetComponent<BoxCollider2D>().size = new Vector2(Path_Collider_Width, Path_Length + 1);
        Path.GetComponent<BoxCollider2D>().offset = new Vector2(0, -Path_Length / 2 - Path_Vertical_Offset);

        int PathUnitNumber = Mathf.RoundToInt(Path_Length * PathSpritePerMeter);
        Sprite[] PathSprites = new Sprite[PathUnitNumber];
        for (int i = 0; i < PathUnitNumber; i++)
        {
            PathSprites[i] = Resources.Load("Sprite/GameElementSprite/Path" + (i % PathSpriteNumber + 1).ToString(), typeof(Sprite)) as Sprite;
        }
        for (int i = 0; i < PathUnitNumber; i++)
        {
            GameObject Unit = (GameObject)Instantiate(Resources.Load("Prefabs/GameObject/PathUnit"), Path.transform.position + Vector3.down * (((float)i + 1) / PathSpritePerMeter + 0.5f / PathSpritePerMeter), new Quaternion(0, 0, 0, 0));
            Unit.GetComponent<SpriteRenderer>().sprite = PathSprites[i % PathUnitNumber];
            Unit.transform.parent = Path.transform;
            Path.GetComponent<BoxCollider2D>().size = new Vector2(Path_Collider_Width, (i+1)*(1.0f/PathSpritePerMeter) + 1);
            Path.GetComponent<BoxCollider2D>().offset = new Vector2(0, -(i+1) * (1.0f / PathSpritePerMeter) / 2 - Path_Vertical_Offset);
            yield return new WaitForSeconds(PathOpenTime/PathUnitNumber);
        }
    }

    private IEnumerator LightAppear()
    {
        GameObject Light = transform.Find("ActivatedLight").gameObject;
        float timecount = 0;
        while (timecount < LightAppearTime)
        {
            Light.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, timecount / LightAppearTime);
            timecount += Time.deltaTime;
            yield return null;
        }

        Light.GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        Instantiate(Resources.Load("Prefabs/VFX/PathTotemDisappear"), transform.position, Quaternion.Euler(0, 0, 0));
    }

}
