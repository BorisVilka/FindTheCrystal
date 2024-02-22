using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{

    [SerializeField] UniWebView webView;

    // Start is called before the first frame update
    void Start()
    {
        string url = PlayerPrefs.GetString("url");
        webView.Load(url);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
