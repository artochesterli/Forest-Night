using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Transport : MonoBehaviour
{
    private bool NearPortal;
    private Player player;
    
    private const float stick_y_threshold = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;
    }

    // Update is called once per frame
    void Update()
    {
        if (NearPortal)
        {
            if (gameObject.CompareTag("Fairy"))
            {
                var Status = GetComponent<Fairy_Status_Manager>();
                if (Status.status == FairyStatus.Normal || Status.status==FairyStatus.Transporting)
                {
                    CheckInput();
                }
            }
            else if (gameObject.CompareTag("Main_Character"))
            {
                var Status = GetComponent<Main_Character_Status_Manager>();
                if (Status.status == MainCharacterStatus.Normal||Status.status==MainCharacterStatus.Transporting)
                {
                    CheckInput();
                }
            }
        }
    }

    private void CheckInput()
    {
        if(player.GetAxis("Left Stick Y") > stick_y_threshold)
        {
            if (gameObject.CompareTag("Fairy"))
            {
                var Status = GetComponent<Fairy_Status_Manager>();
                Status.status = FairyStatus.Transporting;
            }
            else if (gameObject.CompareTag("Main_Character"))
            {
                var Status = GetComponent<Main_Character_Status_Manager>();
                Status.status = MainCharacterStatus.Transporting;
            }
        }
        else
        {
            if (gameObject.CompareTag("Fairy"))
            {
                var Status = GetComponent<Fairy_Status_Manager>();
                Status.status = FairyStatus.Normal;
            }
            else if (gameObject.CompareTag("Main_Character"))
            {
                var Status = GetComponent<Main_Character_Status_Manager>();
                Status.status = MainCharacterStatus.Normal;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Portal"))
        {
            NearPortal=true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Portal"))
        {
            NearPortal = false;
        }
    }

}
