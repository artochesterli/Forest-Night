using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTotem : MonoBehaviour
{
    public List<GameObject> ConnectedMirrors;

    private const float LightingTime = 0.2f;
    private bool Blinking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public void Go()
    {
        if (!Blinking)
        {
            StopAllCoroutines();
            StartCoroutine(Blink());
            for (int i = 0; i < ConnectedMirrors.Count; i++)
            {
                var MirrorStateManager = ConnectedMirrors[i].GetComponent<MirrorStateManager>();
                MirrorStateManager.Activated = !MirrorStateManager.Activated;
            }
        }
    }

    private IEnumerator Blink()
    {
        Blinking = true;
        GameObject MirrorLight = transform.Find("ActivatedLight").gameObject;

        float timecount = 0;
        while (timecount < LightingTime)
        {
            MirrorLight.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, timecount / LightingTime);
            if (!Freeze_Manager.Frozen)
            {
                timecount += Time.deltaTime;
            }
            yield return null;
        }

        timecount = 0;
        while (timecount < LightingTime)
        {
            MirrorLight.GetComponent<SpriteRenderer>().color = Color.Lerp( Color.white, new Color(1, 1, 1, 0), timecount / LightingTime);
            if (!Freeze_Manager.Frozen)
            {
                timecount += Time.deltaTime;
            }
            yield return null;
        }
        Blinking = false;
    }

    public void DisableSelf()
    {
        transform.Find("ActivatedLight").GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        this.enabled = false;
    }

    public void EnableSelf()
    {
        transform.Find("ActivatedLight").GetComponent<SpriteRenderer>().enabled = true;
        transform.Find("ActivatedLight").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<PolygonCollider2D>().enabled = true;
        this.enabled = true;
    }
}
