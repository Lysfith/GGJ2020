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
        [SerializeField] private Transform _offsetHand;

        public ObjectType ObjectType => _type;
        public BotType BotType => _botType;
        public Transform OffsetHand => _offsetHand;

        private void OnEnable()
        {
            _body = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _offsetHand = GetComponent<Transform>();

            Assert.IsNotNull(_body);
            Assert.IsNotNull(_collider);
            Assert.IsNotNull(_offsetHand);
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

        public void EnterWorkbench()
        {
            _collider.enabled = false;
            _body.isKinematic = true;
        }
    }
}
