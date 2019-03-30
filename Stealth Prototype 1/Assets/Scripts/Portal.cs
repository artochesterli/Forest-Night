using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int connected_scene;
    public float transport_time;


    private float time_count;

    private const float RotationSpeed = 120;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckCharacter();
    }

    private void CheckCharacter()
    {
        if (Character_Manager.Main_Character != null && Character_Manager.Fairy != null)
        {
            var MainCharacter = Character_Manager.Main_Character.GetComponent<Main_Character_Status_Manager>();
            var Fairy = Character_Manager.Fairy.GetComponent<Fairy_Status_Manager>();
            if (MainCharacter.status == MainCharacterStatus.Transporting && Fairy.status == FairyStatus.Transporting)
            {
                transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);
                time_count += Time.deltaTime;
                if (time_count > transport_time)
                {
                    SceneManager.LoadScene(connected_scene);
                }
            }
            else
            {
                time_count = 0;
            }
        }
        
        

    }


}
