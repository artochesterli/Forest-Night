using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public float DisMax;
    public float DisMin;
    public Color SavedColor;

    private bool activated;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckStatus();
    }

    private void CheckStatus()
    {
        if (!activated)
        {
            float MainCharacterDis = DisMin;
            if ((transform.position - Character_Manager.Main_Character.transform.position).magnitude > MainCharacterDis)
            {
                MainCharacterDis = (transform.position - Character_Manager.Main_Character.transform.position).magnitude;
                if (MainCharacterDis > DisMax)
                {
                    MainCharacterDis = DisMax;
                }
            }
            float FairyDis = DisMin;
            if ((transform.position - Character_Manager.Fairy.transform.position).magnitude > FairyDis)
            {
                FairyDis = (transform.position - Character_Manager.Fairy.transform.position).magnitude;
                if (FairyDis > DisMax)
                {
                    FairyDis = DisMax;
                }
            }

            foreach (Transform child in transform)
            {
                child.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - (0.5f * (MainCharacterDis - DisMin) + 0.5f * (FairyDis - DisMin)) / (DisMax - DisMin));
            }

            if (MainCharacterDis <= DisMin && FairyDis <= DisMin)
            {
                activated = true;
                foreach (Transform child in transform)
                {
                    child.GetComponent<SpriteRenderer>().color = SavedColor;
                }
                EventManager.instance.Fire(new MemoryActivate());
            }
        }
    }
}
