using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemiesMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform player;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            transform.Translate(direction * speed * Time.deltaTime);

            transform.up = direction;
        }
    }
}
