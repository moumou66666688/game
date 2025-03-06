using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorController : MonoBehaviour
{
    // Start is called before the first frame update
    public string nextSceneName = "test";

    void Start()
    {
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
