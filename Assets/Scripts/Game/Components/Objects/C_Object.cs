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
    [Serializable]
    public class C_Object : MonoBehaviour
    {
        [SerializeField] private Rigidbody _body;
        [SerializeField] private ObjectType _type;
        [SerializeField] private C_Workbench _workbench;
        [SerializeField] private Collider _collider;
        [SerializeField] private PartVersion _partVersion;
        [SerializeField] private Transform _offsetHand;

        public ObjectType ObjectType => _type;
        public PartVersion Version => _partVersion;
        public Transform OffsetHand => _offsetHand;

        private void OnEnable()
        {
            _body = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();

            Assert.IsNotNull(_body);
            Assert.IsNotNull(_collider);
            Assert.IsNotNull(_offsetHand);
        }

        public void Take(GameObject player)
        {
            if(_workbench)
            {
                _workbench.CurrentObject = null;
                PopupManager.ShowTipOnPlayer(player, ObjectType.COMPLETED);
            }
            else
            {
                PopupManager.ShowTipOnPlayer(player, this._type);
            }
            _collider.enabled = false;
            _body.isKinematic = true;
        }

        public void Release(GameObject player)
        {
            _collider.enabled = true;
            _body.isKinematic = false;
            transform.SetParent(null);
            PopupManager.RemoveTipOnPlayer(player);
        }

        public void Throw(Vector3 direction, GameObject player)
        {
            _collider.enabled = true;
            _body.isKinematic = false;
            transform.SetParent(null);
            _body.AddForce(direction.normalized * 10, ForceMode.Impulse);
            PopupManager.RemoveTipOnPlayer(player);
        }

        public void EnterWorkbench()
        {
            _collider.enabled = false;
            _body.isKinematic = true;
        }

        public Vector3 GetHandPosition()
        {
            return -new Vector3(
                OffsetHand.localPosition.x * transform.localScale.x,
                OffsetHand.localPosition.y * transform.localScale.y,
                OffsetHand.localPosition.z * transform.localScale.z
           );
        }
    }
}
