using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pang : MonoBehaviour
{
    public float power = 1000f;
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
        if (collision.gameObject.tag == "Border")
        {

        }

    }
}
