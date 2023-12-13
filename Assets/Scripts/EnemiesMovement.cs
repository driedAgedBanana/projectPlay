using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemiesMovement : MonoBehaviour
{
    public GameObject player;
    public float speed;

    public float distanceBetween;

    private float distance;

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        // Limit movement to up, down, right, and left
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        int roundedAngle = Mathf.RoundToInt(angle / 90.0f) * 90; // Round to nearest 90 degrees
        Vector2 limitedDirection = Quaternion.Euler(0, 0, roundedAngle) * Vector2.right;

        if (distance < distanceBetween)
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + limitedDirection, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * roundedAngle);
        }
    }
}
