using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public Vector2 direction;
    public bool Emited;
    public float PrepareRotationSpeed;
    public float EmitRotationSpeed;

    private float RotationSpeed;
    private float RotaionAngle;
    private bool Frozen;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        Emited = false;
        EventManager.instance.AddHandler<FreezeGame>(OnFreezeGame);
        EventManager.instance.AddHandler<UnFreezeGame>(OnUnFreezeGame);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<FreezeGame>(OnFreezeGame);
        EventManager.instance.RemoveHandler<UnFreezeGame>(OnUnFreezeGame);
    }

    // Update is called once per frame
    void Update()
    {
        SetTransform();
    }

    private void SetTransform()
    {
        if (!Frozen)
        {
            transform.position += speed * (Vector3)direction * Time.deltaTime;
        }
        
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Emited)
        {
            GameObject ob = collision.GetComponent<Collider2D>().gameObject;

            if (!ob.CompareTag("Mirror"))
            {
                Instantiate(Resources.Load("Prefabs/VFX/StarHit"), transform.position, Quaternion.Euler(0, 0, 0));
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Emited)
        {
            GameObject ob = collision.GetComponent<Collider2D>().gameObject;

            if (ob.CompareTag("Mirror"))
            {
                direction.x = -direction.x;
            }
        }
    }

    private void OnFreezeGame(FreezeGame F)
    {
        Frozen = true;
        GetComponent<ParticleSystem>().Pause(true);
    }

    private void OnUnFreezeGame(UnFreezeGame F)
    {
        Frozen = false;
        GetComponent<ParticleSystem>().Play(true);
    }

}
