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
                if (difference.x > 0)
                {
                    anim.SetBool("isFlying", true);
                    anim.SetBool("FlyFront", false);
                    anim.SetBool("FlyBack", false);
                    anim.SetBool("flyAtk", false);
                    anim.SetBool("isCribbling", false);

                    sr.flipX = true;
                }
                else if (difference.x < 0)
                {
                    anim.SetBool("isFlying", true);
                    anim.SetBool("FlyFront", false);
                    anim.SetBool("FlyBack", false);
                    anim.SetBool("flyAtk", false);
                    anim.SetBool("isCribbling", false);

                    sr.flipX = false;
                }
            }

            if (Mathf.Abs(difference.y) > Mathf.Abs(difference.x))
            {
                if (difference.y > 0)
                {
                    anim.SetBool("FlyBack", true);
                    anim.SetBool("isFlying", false);
                    anim.SetBool("FlyFront", false);
                    anim.SetBool("flyAtk", false);
                    anim.SetBool("isCribbling", false);
                }
                else if (difference.y < 0)
                {
                    anim.SetBool("FlyFront", true);
                    anim.SetBool("flyAtk", false);
                    anim.SetBool("FlyBack", false);
                    anim.SetBool("isFlying", false);
                    anim.SetBool("isCribbling", false);
                }
            }


            // Check for transforming condition
            if (isStatue && Mathf.Abs(difference.x) > 0.01f)
            {
                // Trigger transforming animation only for horizontal movement
                StartCoroutine(notMoving());
            }
        }
        else
        {
            isStatue = true;

            anim.SetBool("FlyFront", false);
            anim.SetBool("flyAtk", false);
            anim.SetBool("FlyBack", false);
            anim.SetBool("isFlying", false);
            anim.SetBool("isCribbling", false);
        }
        

        IEnumerator notMoving()
        {
            anim.SetTrigger("isCribbling");
            speed = 0;
            isStatue = false;
            yield return new WaitForSeconds(2.5f);
            speed = 7;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player entered the trigger zone, trigger transforming animation
            anim.SetTrigger("isCribbling");
            isStatue = false;
        }
    }
}
