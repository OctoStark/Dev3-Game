using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyTrigger : MonoBehaviour
{
    public event System.Action<Collider> triggerEnter;
    public event System.Action<Collider> triggerExit;

    private void OnTriggerEnter(Collider other)
    {
        triggerEnter?.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        triggerExit?.Invoke(other);
    }
}
