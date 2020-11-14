using System.Collections;
using System.Collections.Generic;
using Racing.Vehicle.Common;
using UnityEngine;

namespace Racing.Vehicle.Common
{
    public class Vehicle : MonoBehaviour
    {
        public new Rigidbody rigidbody;
        public List<Axle> axles;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }
    }

}