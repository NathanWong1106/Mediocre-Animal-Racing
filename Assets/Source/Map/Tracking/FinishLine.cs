using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Racing.User;

public class FinishLine : MonoBehaviour
{
    public event Action<Player> onFinishCrossed;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player && onFinishCrossed != null)
            onFinishCrossed.Invoke(player);
    }
}
