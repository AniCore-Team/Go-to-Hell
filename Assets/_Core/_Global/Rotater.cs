using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private Vector3 direction;

    private void LateUpdate()
    {
        transform.Rotate(direction);
    }
}
