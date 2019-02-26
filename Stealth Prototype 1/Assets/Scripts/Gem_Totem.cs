using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem_Totem : MonoBehaviour
{

    public float cooldown;
    public float emit_velocity;
    public float emit_angle;

    private float cooldown_count;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Check_Cooldown();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.name == "Weapon")
        {
            var self_status = GetComponent<Totem_Status_Manager>();
            self_status.Status = self_status.INGROUND;
            cooldown_count = 0;
            emit_gem();
        }
    }

    private void emit_gem()
    {
        GameObject gem = (GameObject)Instantiate(Resources.Load("Prefabs/Gem"), transform.position, new Quaternion(0, 0, 0, 0));
        Vector2 direction = Vector2.right;
        direction = Rotate(direction, emit_angle);
        gem.GetComponent<Rigidbody2D>().velocity = direction * emit_velocity;
    }

    private void Check_Cooldown()
    {
        var self_status = GetComponent<Totem_Status_Manager>();
        if (self_status.Status == self_status.INGROUND)
        {
            cooldown_count += Time.deltaTime;
            if (cooldown_count > cooldown)
            {
                self_status.Status = self_status.APPEAR;
            }
        }
    }

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}
