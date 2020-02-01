using Assets.Scripts.Game.Components.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Game.Components.Bots
{
    public class C_Bot : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _body;

        [Header("Positions")]
        [SerializeField] private Transform _headPosition;
        [SerializeField] private Transform _leftArmPosition;
        [SerializeField] private Transform _rightArmPosition;
        [SerializeField] private Transform _chestPosition;

        [Header("Parts")]
        [SerializeField] private bool _head;
        [SerializeField] private bool _leftArm;
        [SerializeField] private bool _rightArm;
        [SerializeField] private bool _chest;

        [Header("Properties")]
        [SerializeField] private BotType _type;

        public event EventHandler OnBotComplete;

        private void OnEnable()
        {
            _collider = GetComponent<Collider>();
            _body = GetComponent<Rigidbody>();

            Assert.IsNotNull(_collider);
            Assert.IsNotNull(_body);
        }

        public void AddPart(C_Object part, GameObject player)
        {
            if(part.ObjectType == ObjectType.WASTE
                || part.BotType != _type)
            {
                part.Release(player);
                return;
            }

            Transform partDestination = null;

            switch (part.ObjectType)
            {
                case ObjectType.CHEST:
                    if (_chest)
                    {
                        part.Release(player);
                        return;
                    }
                    _chest = part;
                    partDestination = _chestPosition;
                    break;
                case ObjectType.HEAD:
                    if (_head)
                    {
                        part.Release(player);
                        return;
                    }
                    _head = part;
                    partDestination = _headPosition;
                    break;
                case ObjectType.LEFT_ARM:
                    if (_leftArm)
                    {
                        part.Release(player);
                        return;
                    }
                    _leftArm = part;
                    partDestination = _leftArmPosition;
                    break;
                case ObjectType.RIGHT_ARM:
                    if (_rightArm)
                    {
                        part.Release(player);
                        return;
                    }
                    _rightArm = part;
                    partDestination = _rightArmPosition;
                    break;
            }

            var botPartAnim = part.gameObject.AddComponent<C_BotPartAnimation>();
            botPartAnim.Init(partDestination, () =>
            {
                partDestination.gameObject.SetActive(true);
                CheckBotCompleted();
            }); 
        }

        private void CheckBotCompleted()
        {
            if(!_head
                || !_chest
                || !_leftArm
                || !_rightArm)
            {
                return;
            }

            BotComplete();
        }

        private void BotComplete()
        {
            _collider.enabled = false;
            _body.isKinematic = false;
            _body.useGravity = true;

            StartCoroutine(DestroyAfterTime());
        }

        private IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(5);

            OnBotComplete?.Invoke(this, null);
            Destroy(gameObject);
        }
    }
}
