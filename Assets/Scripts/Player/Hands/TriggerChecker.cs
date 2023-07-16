using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    public static event System.Action TriggerHitsSmth;

    private void OnTriggerEnter(Collider other)
    {
        TriggerHitsSmth?.Invoke();
    }
}
