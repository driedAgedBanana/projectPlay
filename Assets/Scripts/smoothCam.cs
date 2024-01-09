using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoothCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float damping;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 movePos = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePos, ref velocity, damping);

        if (target == null)
        {
            return;
        }
    }
}
