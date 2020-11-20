using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing.Vehicles.Common
{
    public class Vehicle : MonoBehaviour
    {
        [HideInInspector] public Rigidbody Rigidbody;
        public List<Axle> Axles;

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
    }

}