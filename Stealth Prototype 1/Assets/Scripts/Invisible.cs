using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour
{
    public bool AbleToInvisible;
    public bool invisible;
    public float invisible_alpha;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckCapability();
        CheckStatus();
    }

    private void CheckCapability()
    {
        if (CompareTag("Main_Character"))
        {
            if (GetComponent<Main_Character_Status_Manager>().status == MainCharacterStatus.Normal&&GetComponent<CharacterMove>().OnGround)
            {
                AbleToInvisible = true;
            }
            else
            {
                AbleToInvisible = false;
            }
        }
        else if(CompareTag("Fairy"))
        {
            if (GetComponent<Fairy_Status_Manager>().status == FairyStatus.Normal && GetComponent<CharacterMove>().OnGround)
            {
                AbleToInvisible = true;
            }
            else
            {
                AbleToInvisible = false;
            }
        }
    }
 
    private void CheckStatus()
    {
        if (AbleToInvisible)
        {
            transform.Find("AbleToInvisibleIndicator").GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            transform.Find("AbleToInvisibleIndicator").GetComponent<SpriteRenderer>().enabled = false;
        }
        if (invisible)
        {
            Color current_color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(current_color.r, current_color.g, current_color.b, invisible_alpha);
            gameObject.layer = LayerMask.NameToLayer("Invisible_Object");
            transform.Find("2D Light").GetComponent<MeshRenderer>().enabled = false;
            foreach(Transform child in transform.Find("LightToEnvironment"))
            {
                child.GetComponent<Light>().enabled = false;
            }
        }
        else
        {
            Color current_color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(current_color.r, current_color.g, current_color.b, 1);
            if (CompareTag("Main_Character"))
            {
                gameObject.layer = LayerMask.NameToLayer("Main_Character");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Fairy");
            }
            transform.Find("2D Light").GetComponent<MeshRenderer>().enabled = true;
            foreach (Transform child in transform.Find("LightToEnvironment"))
            {
                child.GetComponent<Light>().enabled = true;
            }
        }
    }
    
}
