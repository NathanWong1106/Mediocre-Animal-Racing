using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody= GetComponent<Rigidbody>();
    }
}
