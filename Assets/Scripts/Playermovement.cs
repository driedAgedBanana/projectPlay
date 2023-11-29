using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        transform.Translate(Vector2.right * xInput * speed * Time.deltaTime);
        transform.Translate(Vector2.up * yInput * speed * Time.deltaTime);
    }
}
