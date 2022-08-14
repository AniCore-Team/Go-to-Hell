using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
    public virtual void ActivateWindow()
    {
        gameObject.SetActive(true);
    }

    public virtual void DisactivateWindow()
    {
        gameObject.SetActive(false);
    }
}
