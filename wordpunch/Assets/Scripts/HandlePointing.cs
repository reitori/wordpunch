using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePointing : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameObject").GetComponent<GameManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
