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
        [SerializeField] private Collider _collider;
        [SerializeField] private ObjectType _type;
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

        public void Take(GameObject player)
        {
            _collider.enabled = false;
            _body.isKinematic = true;
            PopupManager.ShowTipOnPlayer(player, this._type);
        }

        public void Release(GameObject player)
        {
            _collider.enabled = true;
            _body.isKinematic = false;
            PopupManager.RemoveTipOnPlayer(player);
        }

        public void Throw(Vector3 direction, GameObject player)
        {
            _collider.enabled = true;
            _body.isKinematic = false;
            _body.AddForce(direction.normalized * 10, ForceMode.Impulse);
            PopupManager.RemoveTipOnPlayer(player);
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
