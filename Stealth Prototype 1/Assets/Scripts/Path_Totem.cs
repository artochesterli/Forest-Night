using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Totem : MonoBehaviour
{
    public float Path_Length;

    private float Path_Collider_Width = 0.3f;
    private float Path_Vertical_Offset = 0;

    private const int PathSpritePerMeter = 2;
    private const int PathSpriteNumber = 4;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Totem_Status_Manager>().Status== GetComponent<Totem_Status_Manager>().INGROUND)
        {
            Create_Path();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;

        if (ob.name == "Weapon")
        {
            var self_status = GetComponent<Totem_Status_Manager>();
            self_status.Status = self_status.INGROUND;
            Create_Path();
        }
    }

    private void Create_Path()
    {
        GameObject Path = transform.Find("Path").gameObject;
        Path.GetComponent<BoxCollider2D>().enabled = true;
        Path.GetComponent<BoxCollider2D>().size = new Vector2(Path_Collider_Width, Path_Length+1);
        Path.GetComponent<BoxCollider2D>().offset = new Vector2(0, -Path_Length / 2-Path_Vertical_Offset);

        int PathUnitNumber = Mathf.RoundToInt(Path_Length * PathSpritePerMeter);
        Sprite[] PathSprites = new Sprite[PathUnitNumber];
        for(int i = 0; i < PathUnitNumber; i++)
        {
            PathSprites[i] = Resources.Load("Sprite/GameElementSprite/Path" + (i % PathSpriteNumber+ 1).ToString(), typeof(Sprite)) as Sprite;
        }
        for(int i = 0; i < PathUnitNumber; i++)
        {
            GameObject Unit=(GameObject)Instantiate(Resources.Load("Prefabs/Path_Unit"), Path.transform.position + Vector3.down * (((float)i+ 1)/ PathSpritePerMeter + 0.5f/PathSpritePerMeter), new Quaternion(0, 0, 0, 0));
            Unit.GetComponent<SpriteRenderer>().sprite = PathSprites[i % PathUnitNumber];
            Unit.transform.parent = Path.transform;
        }
    }

}
