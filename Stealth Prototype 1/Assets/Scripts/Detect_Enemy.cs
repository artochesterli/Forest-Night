using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect_Enemy : MonoBehaviour
{


    private List<GameObject> detected_enemy_list;
    private const float detect_offset=0.5f;
    // Start is called before the first frame update
    void Start()
    {
        detected_enemy_list = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<CharacterMove>().OnGround)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        int layermask = 1 << LayerMask.NameToLayer("Default");
        float dis = GetComponent<CircleCollider2D>().radius;

        if (ob.CompareTag("Enemy")&&GetComponent<CharacterMove>().OnGround&&Vector2.Dot(ob.transform.right,transform.position-ob.transform.position)>0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position+detect_offset*Vector3.up, (ob.transform.position - transform.position ), dis, layermask);
            if (hit)
            {
                Debug.Log(hit.collider.gameObject);
            }
            if (!hit)
            {
                detected_enemy_list.Add(ob);
                var status = ob.GetComponent<Enemy_Status_Manager>();
                if (status.status ==EnemyStatus.Patrol)
                {
                    status.status = EnemyStatus.DrawnByGem;
                    ob.GetComponent<Chase_Gem>().connected_gem = gameObject;
                    ob.GetComponent<Chase_Gem>().StartCoroutine(ob.GetComponent<Chase_Gem>().Chase());
                }
            }
        }
    }



}
