using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButterflyScript : EnemiesMovement
{
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < distanceBetween)
        {
            // Move the enemy towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            //Vector2 direction = (player.transform.position - transform.position).normalized;
            Vector2 difference = player.transform.position - transform.position;

            if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
            {

            }
        }

    }
}
