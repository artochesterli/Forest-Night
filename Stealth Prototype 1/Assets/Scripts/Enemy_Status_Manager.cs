using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStatus
{
    Patrol,
    Alert,
    ShootCharacter,
    AlertRelease,
    DrawnByGem,
    Stunned
}

public class Enemy_Status_Manager : MonoBehaviour
{
    public EnemyStatus status;

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
        if (status == EnemyStatus.Patrol)
        {
            Indicator.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if(status==EnemyStatus.Alert || status == EnemyStatus.AlertRelease)
        {
            var EnemyCheck = GetComponent<Enemy_Check>();
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/exclamation_mark", typeof(Sprite)) as Sprite;
            Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 1 - EnemyCheck.alert_time_count / EnemyCheck.Alert_Time, 1 - EnemyCheck.alert_time_count / EnemyCheck.Alert_Time);
        }
        else if (status == EnemyStatus.DrawnByGem)
        {
            Indicator.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (status == EnemyStatus.ShootCharacter)
        {
            var EnemyCheck = GetComponent<Enemy_Check>();
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/exclamation_mark", typeof(Sprite)) as Sprite;
            Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 1 - EnemyCheck.alert_time_count / EnemyCheck.Alert_Time, 1 - EnemyCheck.alert_time_count / EnemyCheck.Alert_Time);
        }
        else if (status == EnemyStatus.Stunned)
        {
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/Stunned Mark", typeof(Sprite)) as Sprite;
            Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }


    private void set_status()
    {
        GameObject view = transform.Find("View").gameObject;
        if (status == EnemyStatus.ShootCharacter || status==EnemyStatus.Stunned)
        {
            view.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            view.GetComponent<SpriteRenderer>().enabled = true;
        }
    }


}
