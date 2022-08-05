using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationCameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform target;

    public void Init(Transform target)
    {
        this.target = target;
        transform.position = target.position;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
