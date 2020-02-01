using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Game.Components.Objects
{
    public class C_Object : MonoBehaviour
    {
        [SerializeField] private Rigidbody _body;

        private void OnEnable()
        {
            _body = GetComponent<Rigidbody>();

            Assert.IsNotNull(_body);
        }

        public void Take()
        {
            _body.isKinematic = true;
        }

        public void Release()
        {
            _body.isKinematic = false;
        }

        public void Throw(Vector3 direction)
        {
            _body.isKinematic = false;
            _body.AddForce(direction.normalized * 10, ForceMode.Impulse);
        }
    }
}
