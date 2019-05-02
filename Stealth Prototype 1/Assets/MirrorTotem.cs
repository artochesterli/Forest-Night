using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTotem : MonoBehaviour
{
    public List<GameObject> ConnectedMirrors;
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
            for(int i = 0; i < ConnectedMirrors.Count; i++)
            {
                ConnectedMirrors[i].GetComponent<Totem_Status_Manager>().Activated = !ConnectedMirrors[i].GetComponent<Totem_Status_Manager>().Activated;
            }
        }
    }
}
