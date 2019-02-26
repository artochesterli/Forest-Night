using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_Gem : MonoBehaviour
{

    public GameObject connected_gem;
    public float chase_speed;
    public float chase_preparation_time;

    private const float eat_dis = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Chase()
    {
        yield return new WaitForSeconds(chase_preparation_time);
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        while (true)
        {
            Vector2 direction = connected_gem.transform.position - transform.position;
            direction.Normalize();
            transform.position += (Vector3)direction * chase_speed * Time.deltaTime;
            yield return null;

            if ((connected_gem.transform.position - transform.position).magnitude < eat_dis)
            {
                Destroy(connected_gem);
                GetComponent<Rigidbody2D>().gravityScale = 1;
                GetComponent<BoxCollider2D>().isTrigger = false;
                yield break;
            }
        }
        
    }

}
