using System;
using System.Collections;
using System.Collections.Generic;
using Racing.Util;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    Vehicle vehicle;
    public bool grounded = true;

    private void Awake()
    {
        vehicle = GetComponent<Vehicle>();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        vehicle.rigidbody.AddTorque(Vector3.up * InputHelper.Horizontal * VehicleSettings.turnSpeed * Time.deltaTime);
        vehicle.rigidbody.AddForce(transform.forward * InputHelper.Vertical * VehicleSettings.speed * Time.deltaTime);
        vehicle.rigidbody.velocity = Vector3.ClampMagnitude(vehicle.rigidbody.velocity, 2.5f);

        if (InputHelper.Jump && grounded)
        {
            vehicle.rigidbody.AddForce(Vector3.up * VehicleSettings.jumpForce);
            grounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
        grounded = collision.gameObject.CompareTag("Ground") ? true : grounded;
    }
}
