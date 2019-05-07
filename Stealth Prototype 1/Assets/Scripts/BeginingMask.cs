using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginingMask : MonoBehaviour
{
    private const float AppearTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Appear());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Appear()
    {
        GetComponent<Image>().color = Color.black;
        float timecount = 0;
        while (timecount < AppearTime)
        {
            GetComponent<Image>().color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), timecount / AppearTime);
            timecount += Time.deltaTime;
            yield return null;
        }
        GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    
}
