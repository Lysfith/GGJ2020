using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using Assets.Scripts.Game.Components.Repairing;

namespace Assets.Scripts.Game.Components.Objects
{

    public enum ObjectType
    {
        HEAD,
        CHEST,
        LEFT_ARM,
        RIGHT_ARM,
        WASTE
    }

    public class C_Object : MonoBehaviour
    {
        [SerializeField] private Rigidbody _body;
        [SerializeField] private ObjectType type;

        public ObjectType ObjectType {
            get {
                return type;
            }
        }

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

        public void OnCollisionEnter(Collision collision)
        {
            var workbench = collision.collider.GetComponent<C_Workbench>();
            if (workbench && !workbench.CurrentObject)
            {
                transform.parent = workbench.transform;
                transform.rotation = workbench.transform.rotation;
                transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
                workbench.CurrentObject = this;
                _body.isKinematic = true;
            }
        }
    }
}
