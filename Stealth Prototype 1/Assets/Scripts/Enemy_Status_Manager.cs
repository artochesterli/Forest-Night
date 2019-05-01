using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStatus
{
    Patrol,
    Alert,
    ShootCharacter,
    ShootStar,
    AlertRelease,
    Stunned
}

public class Enemy_Status_Manager : MonoBehaviour
{
    public EnemyStatus status;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<CharacterDied>(OnCharacterDied);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<CharacterDied>(OnCharacterDied);
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
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/StunIcon", typeof(Sprite)) as Sprite;
            Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }


    private void set_status()
    {
        GameObject view = transform.Find("View").gameObject;
        if (status == EnemyStatus.ShootCharacter || status==EnemyStatus.Stunned)
        {
            view.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            view.GetComponent<MeshRenderer>().enabled = true;
            if (transform.right.x > 0)
            {
                view.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                view.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }

    private void OnCharacterDied(CharacterDied C)
    {
        status = EnemyStatus.Patrol;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;

        if (ob.CompareTag("Slash"))
        {
            Instantiate(Resources.Load("Prefabs/VFX/EnemyDeath"), transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }

        if (ob.CompareTag("Arrow"))
        {
            GetComponent<Enemy_Check>().stun_time_count = 0;
            status = EnemyStatus.Stunned;
        }
    }
}
