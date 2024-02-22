using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    // Start is called before the first frame update
       
     void Start()
    {   
        Invoke("nextScreen", 2);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void nextScreen()
    {
        SceneManager.LoadScene(1);
    }

   

}
