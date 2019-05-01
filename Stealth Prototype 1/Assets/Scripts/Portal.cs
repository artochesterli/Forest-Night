using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int ConnectedLevel;
    public float ActivateDis;
    public Color ActivateColor;

    private bool Activated;

    private const float LoadSceneTime = 0.5f;

    private const float RotationSpeed = 120;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Activated)
        {
            CheckCharacter();
        }
    }

    private void CheckCharacter()
    {
        if (Character_Manager.Main_Character != null && Character_Manager.Fairy != null)
        {
            if((transform.position-Character_Manager.Main_Character.transform.position).magnitude<ActivateDis && (transform.position - Character_Manager.Fairy.transform.position).magnitude < ActivateDis)
            {
                Activated = true;
                StartCoroutine(Activate());
            }
        }

    }

    private IEnumerator Activate()
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().color = ActivateColor;
        }
        

        yield return new WaitForSeconds(LoadSceneTime);
        SceneManager.LoadSceneAsync("Level "+ConnectedLevel.ToString());
    }

}
