using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopJumpTutorialTrigger : MonoBehaviour
{
    private bool FairyIn;
    private bool MainCharacterIn;

    public GameObject LTIcon;
    public GameObject RTIcon;
    public Vector2 LTIconLocalPosition;
    public Vector2 RTIconLocalPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }

    private void CheckState()
    {
        if (FairyIn)
        {
            if (Character_Manager.Fairy.GetComponent<CharacterMove>().OnGround)
            {
                LTIcon.GetComponent<SpriteRenderer>().enabled = false;
                
            }
            else
            {
                LTIcon.GetComponent<SpriteRenderer>().enabled = true;
                LTIcon.transform.position = Character_Manager.Fairy.transform.position + (Vector3)LTIconLocalPosition;
            }
        }
        else
        {
            LTIcon.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (MainCharacterIn)
        {
            if (Character_Manager.Main_Character.GetComponent<Dash_To_Fairy>().detect_float_fairy)
            {
                RTIcon.GetComponent<SpriteRenderer>().enabled = true;
                RTIcon.transform.position = Character_Manager.Main_Character.transform.position + (Vector3)RTIconLocalPosition;
            }
            else
            {
                RTIcon.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else
        {
            RTIcon.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Fairy"))
        {
            FairyIn = true;
        }
        if (ob.CompareTag("Main_Character"))
        {
            MainCharacterIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Fairy"))
        {
            FairyIn = false;
        }
        if (ob.CompareTag("Main_Character"))
        {
            MainCharacterIn = false;
        }
    }
}
