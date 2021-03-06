﻿using Assets.Scripts.Game.Components.Objects;
using Assets.Scripts.Game.Components.Systems;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Components.Bots
{
    public class C_BotPartAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _destination;
        [SerializeField] private Collider _collider;
        [SerializeField] private Action _callbackAtEnd;
        [SerializeField] private float _duration = 2;

        private void OnEnable()
        {
            _collider = GetComponent<Collider>();
        }

        public void Init(Transform destination, Action callbackAtEnd)
        {
            _destination = destination;
            _callbackAtEnd = callbackAtEnd;

            StartAnimation();
        }
        
        private void StartAnimation()
        {
            _collider.enabled = false;

            var sequence = DOTween.Sequence();
            sequence.Insert(0, transform.DOMove(_destination.position, _duration));
            sequence.Insert(0, transform.DORotate(_destination.rotation.eulerAngles + new Vector3(0, 90, 0), _duration));
            sequence.Insert(0, transform.DOScale(_destination.localScale, _duration));
            sequence.OnComplete(EndAnimation);
        }

        private void EndAnimation()
        {
            _callbackAtEnd?.Invoke();

            C_LevelSystem.Instance.RemoveObjectFromList(gameObject.GetComponent<C_Object>());

            Destroy(gameObject);
        }
    }
}
