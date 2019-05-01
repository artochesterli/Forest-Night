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

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        Emited = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetTransform();
    }

    private void SetTransform()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount];
        GetComponent<ParticleSystem>().GetParticles(particles);
        if (Emited)
        {
            if (OutBound())
            {
                Destroy(gameObject);
            }
            RotationSpeed = EmitRotationSpeed;
        }
        else
        {
            RotationSpeed = PrepareRotationSpeed;
        }
        transform.position += speed * (Vector3)direction * Time.deltaTime;
        
        
    }

    private bool OutBound()
    {
        float bound_y = Camera.main.orthographicSize;
        float bound_x = Camera.main.orthographicSize * Camera.main.pixelWidth / Camera.main.pixelHeight;
        if (transform.position.x > bound_x || transform.position.x < -bound_x || transform.position.y > bound_y || transform.position.y < -bound_y)
        {
            return true;
        }
        else
        {
            return false;
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

}
