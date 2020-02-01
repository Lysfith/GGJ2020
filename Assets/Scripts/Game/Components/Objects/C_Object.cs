using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using Assets.Scripts.Game.Components.Repairing;
using Assets.Scripts.Game.Components.Bots;

namespace Assets.Scripts.Game.Components.Objects
{
    public class C_Object : MonoBehaviour
    {
        [SerializeField] private Rigidbody _body;
        [SerializeField] private ObjectType type;
        [SerializeField] private C_Workbench _workbench;
        [SerializeField] private Collider _collider;
        [SerializeField] private BotType _botType;

        public ObjectType ObjectType => _type;
        public BotType BotType => _botType;

        private void OnEnable()
        {
            _body = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();

            Assert.IsNotNull(_body);
            Assert.IsNotNull(_collider);
        }

        public void Take()
        {
            if(_workbench)
            {
                _workbench.CurrentObject = null;
            }
            _collider.enabled = false;
            _body.isKinematic = true;
        }

        public void Release()
        {
            _collider.enabled = true;
            _body.isKinematic = false;
        }

        public void Throw(Vector3 direction)
        {
            _collider.enabled = true;
            _body.isKinematic = false;
            _body.AddForce(direction.normalized * 10, ForceMode.Impulse);
        }

        public void OnCollisionEnter(Collision collision)
        {
            var workbench = collision.collider.GetComponent<C_Workbench>();
            if (workbench && !workbench.CurrentObject && workbench.ObjectType == type)
            {
                transform.rotation = workbench.transform.rotation;
                // TODO : Change to workbench offset transform
                transform.position = workbench.transform.position;
                workbench.CurrentObject = this;
                _workbench = workbench;
                _body.isKinematic = true;
            }
        }
    }
}
