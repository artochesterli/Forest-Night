using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TutorialFrame : MonoBehaviour
{
    public bool Open;

    private Player MainCharacterPlayer;
    private Player FairyPlayer;

    // Start is called before the first frame update
    void Start()
    {
        MainCharacterPlayer = ReInput.players.GetPlayer(0);
        FairyPlayer = ReInput.players.GetPlayer(1);
    }

    // Update is called once per frame
    void Update()
    {
        CheckStatus();
        CheckInput();
    }

    private void CheckStatus()
    {
        if (Open)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            transform.Find("Canvas").GetComponent<Canvas>().enabled = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            transform.Find("Canvas").GetComponent<Canvas>().enabled = false;
        }
    }

    private void CheckInput()
    {
        if (Open)
        {
            if (MainCharacterPlayer.GetButtonDown("A") || FairyPlayer.GetButtonDown("A"))
            {
                EventManager.instance.Fire(new TutorialClose());
                Open = false;
            }
        }
    }
}
