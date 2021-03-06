﻿using Assets.Scripts.Game.Components.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Components.Characters.Triggers
{
    public class C_TriggerHand : MonoBehaviour
    {
        //public C_Object CurrentObject;

        //private void OnTriggerEnter(Collider other)
        //{
        //    CurrentObject = other.GetComponent<C_Object>();
        //}

        //private void OnTriggerExit(Collider other)
        //{
        //    CurrentObject = null;
        //}

        public IEnumerable<C_Object> GetObjects()
        {
            var colliders = Physics.OverlapSphere(transform.position, 1, LayerMask.GetMask("Object", "Waste"));
            return colliders.Select(c => c.GetComponent<C_Object>()).ToList();
        }
    }
}
