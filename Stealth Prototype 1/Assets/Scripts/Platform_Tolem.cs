using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Tolem : MonoBehaviour
{
    public GameObject Connected_Moving_Platform;
    public GameObject Connected_Platform_Tolem;
    public Vector3 FirstPoint;
    public Vector3 SecondPoint;
    public float move_period;
    public bool moving;

    private bool At_First_Point;
    // Start is called before the first frame update
    void Start()
    {
        At_First_Point = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Move()
    {
        moving = true;
        Vector2 direction = Vector2.zero;
        Vector3 EndPoint = Vector3.zero;
        float speed = ((Vector2)(SecondPoint - FirstPoint)).magnitude/move_period;
        if (At_First_Point)
        {
            direction = SecondPoint - FirstPoint;
            EndPoint = SecondPoint;
        }
        else
        {
            direction = FirstPoint - SecondPoint;
            EndPoint = FirstPoint;
        }
        direction.Normalize();
        while (Vector2.Dot(direction, transform.position - EndPoint) < 0)
        {
            transform.position += (Vector3)(speed * direction * Time.deltaTime);
            yield return null;
        }
        transform.position = EndPoint;
        At_First_Point = !At_First_Point;
        moving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.name == "Weapon"&&!moving)
        {
            StartCoroutine(Move());
            if (Connected_Moving_Platform != null)
            {
                Connected_Moving_Platform.GetComponent<Platform_Tolem>().StartCoroutine(Connected_Moving_Platform.GetComponent<Platform_Tolem>().Move());
            }
        }
    }
}
