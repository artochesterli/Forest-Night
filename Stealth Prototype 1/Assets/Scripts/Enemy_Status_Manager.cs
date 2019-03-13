using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Status_Manager : MonoBehaviour
{
    public int status;

    public int PATROL = 0;
    public int ALERT = 1;
    public int SHOOT_CHARACTER = 2;
    public int ALERT_RELEASE = 3;
    public int DRAWN_BY_GEM = 4;
    public int STUNNED = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetIndicatorIcon();
        set_status();
    }

    private void SetIndicatorIcon()
    {
        GameObject Indicator = transform.Find("Indicator").gameObject;
        if (status == PATROL)
        {
            Indicator.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if(status==ALERT || status == ALERT_RELEASE)
        {
            var EnemyCheck = GetComponent<Enemy_Check>();
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/exclamation_mark", typeof(Sprite)) as Sprite;
            Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 1 - EnemyCheck.alert_time_count / EnemyCheck.Alert_Time, 1 - EnemyCheck.alert_time_count / EnemyCheck.Alert_Time);
        }
        else if (status == DRAWN_BY_GEM)
        {
            Indicator.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (status == SHOOT_CHARACTER)
        {
            var EnemyCheck = GetComponent<Enemy_Check>();
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/exclamation_mark", typeof(Sprite)) as Sprite;
            Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 1 - EnemyCheck.alert_time_count / EnemyCheck.Alert_Time, 1 - EnemyCheck.alert_time_count / EnemyCheck.Alert_Time);
        }
        else if (status == STUNNED)
        {
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/Stunned Mark", typeof(Sprite)) as Sprite;
            Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }


    private void set_status()
    {
        GameObject view = transform.Find("View").gameObject;
        if (status == SHOOT_CHARACTER || status==STUNNED)
        {
            view.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            view.GetComponent<SpriteRenderer>().enabled = true;
        }
    }


}
