using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;
        transform.rotation = Camera.main.transform.rotation;
    }
}
