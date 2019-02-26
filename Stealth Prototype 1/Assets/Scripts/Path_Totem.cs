using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Totem : MonoBehaviour
{
    public GameObject connected_totem;
    public float Path_Length;

    private float Path_Collider_Width = 0.5f;
    private float Path_Vertical_Offset = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Totem_Status_Manager>().Status== GetComponent<Totem_Status_Manager>().INGROUND)
        {
            Create_Path();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;

        if (ob.name == "Weapon")
        {
            var self_status = GetComponent<Totem_Status_Manager>();
            self_status.Status = self_status.INGROUND;
            Create_Path();
            if (connected_totem != null)
            {
                var connected_status = connected_totem.GetComponent<Totem_Status_Manager>();
                connected_status.Status = connected_status.APPEAR;
                connected_totem.GetComponent<Path_Totem>().Clear_Path();
            }
        }
    }

    private void Create_Path()
    {
        GameObject Path = transform.Find("Path").gameObject;
        Path.GetComponent<BoxCollider2D>().enabled = true;
        Path.GetComponent<BoxCollider2D>().size = new Vector2(Path_Collider_Width, Path_Length+1);
        Path.GetComponent<BoxCollider2D>().offset = new Vector2(0, -Path_Length / 2-Path_Vertical_Offset);
        for(int i = 0; i < Path_Length; i++)
        {
            Instantiate(Resources.Load("Prefabs/Path_Unit"), Path.transform.position + Vector3.down * (i+1), new Quaternion(0, 0, 0, 0));
        }
    }

    private void Clear_Path()
    {
        GameObject Path = transform.Find("Path").gameObject;
        Path.GetComponent<BoxCollider2D>().enabled = false;
        foreach (Transform child in Path.transform)
        {
            if (child.name != "Path_End")
            {
                Destroy(child.gameObject);
            }
            
        }
    }
}
