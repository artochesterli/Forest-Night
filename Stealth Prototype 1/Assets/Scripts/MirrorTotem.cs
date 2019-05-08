using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTotem : MonoBehaviour
{
    public List<GameObject> ConnectedMirrors;

    private const float LightingTime = 0.2f;
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
        if (collision.GetComponent<Collider2D>().gameObject.CompareTag("Slash"))
        {
            StopAllCoroutines();
            StartCoroutine(Blink());
            for(int i = 0; i < ConnectedMirrors.Count; i++)
            {
                var MirrorStateManager = ConnectedMirrors[i].GetComponent<MirrorStateManager>();
                MirrorStateManager.Activated = !MirrorStateManager.Activated;
            }
        }
    }

    private IEnumerator Blink()
    {
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
    }
}
