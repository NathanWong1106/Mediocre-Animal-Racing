using System.Collections;
using System.Collections.Generic;
using Racing.Vehicle.Common;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public new Rigidbody rigidbody;
    public List<Axle> axles;

    private void Awake()
    {
        rigidbody= GetComponent<Rigidbody>();
    }
}
