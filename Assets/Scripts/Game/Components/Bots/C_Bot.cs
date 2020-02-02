using Assets.Scripts.Game.Components.Objects;
using Assets.Scripts.Game.Components.Systems;
using DG.Tweening;
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
        [SerializeField] private Animator _animator;

        [Header("References")]
        [SerializeField] private List<GameObject> _heads;
        [SerializeField] private List<GameObject> _chests;
        [SerializeField] private List<GameObject> _leftArms;
        [SerializeField] private List<GameObject> _rightArms;
        [SerializeField] private List<GameObject> _legs;
        [SerializeField] private Material _materialV1;
        [SerializeField] private Material _materialV2;

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
        [SerializeField] private Dictionary<ObjectType, PartVersion> _parts;

        public Dictionary<ObjectType, PartVersion> Parts => _parts;

        public event EventHandler OnBotComplete;

        private void OnEnable()
        {
            _collider = GetComponent<Collider>();
            _body = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();

            Assert.IsNotNull(_collider);
            Assert.IsNotNull(_body);
            Assert.IsNotNull(_animator);

            _heads.ForEach((i) => { i.SetActive(false); });
            _chests.ForEach((i) => { i.SetActive(false); });
            _leftArms.ForEach((i) => { i.SetActive(false); });
            _rightArms.ForEach((i) => { i.SetActive(false); });
            _legs.ForEach((i) => { i.SetActive(false); });

            _parts = new Dictionary<ObjectType, PartVersion>();
        }

        public void Init(Dictionary<ObjectType, PartVersion> parts, Color color)
        {
            _parts = parts;

            var newMaterialV1 = _materialV1; // new Material(_materialV1);
            var newMaterialV2 = _materialV2; // new Material(_materialV2);

            newMaterialV1.SetColor("Color_F5B15491", color);
            newMaterialV1.SetFloat("FresnelOpacity_", 0);

            newMaterialV2.SetColor("Color_F5B15491", color);
            newMaterialV2.SetFloat("FresnelOpacity_", 0);

            var renderers = GetComponentsInChildren<Renderer>(true);
           
            foreach (var renderer in renderers)
            {
                var materialV1 = renderer.materials.Where(i => i.name == "Color2_V1").FirstOrDefault();
                var materialV2 = renderer.materials.Where(i => i.name == "Color2_V2").FirstOrDefault();

                if (materialV1 != null)
                {
                    var index = Array.IndexOf(renderer.materials, materialV1);
                    renderer.materials[index] = newMaterialV1;
                }
                else if (materialV2 != null)
                {
                    var index = Array.IndexOf(renderer.materials, materialV2);
                    renderer.materials[index] = newMaterialV2;
                }

            }

            var leg = GetGOFromTypeAndVersion(ObjectType.LEG, _parts[ObjectType.LEG]);
            leg.SetActive(true);
        }

        public GameObject GetGOFromTypeAndVersion(ObjectType type, PartVersion version)
        {
            var intVersion = (int)version;
            switch (type)
            {
                case ObjectType.HEAD: return _heads.ElementAt(intVersion);
                case ObjectType.CHEST: return _chests.ElementAt(intVersion);
                case ObjectType.LEFT_ARM: return _leftArms.ElementAt(intVersion);
                case ObjectType.RIGHT_ARM: return _rightArms.ElementAt(intVersion);
                case ObjectType.LEG: return _legs.ElementAt(intVersion);
            }

            return null;
        }

        public void AddPart(C_Object part, GameObject player)
        {
            if(part.ObjectType == ObjectType.WASTE
                || _parts[part.ObjectType] != part.Version)
            {
                part.Release(player);
                return;
            }

            var robotArm = part.GetComponent<C_RobotArm>();
            var robotPart = part.GetComponent<C_RobotPart>();

            if (robotArm == null && robotPart == null)
            {
                part.Release(player);
                return;
            }

            if((robotArm != null && robotArm.Progress < 1)
                || (robotPart != null && robotPart.Progress < robotPart.Hardness))
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
            SoundManager.PlaySound(SoundList.Sound.addpart);
            PopupManager.RemoveTipOnPlayer(this.gameObject);
            var botPartAnim = part.gameObject.AddComponent<C_BotPartAnimation>();
            botPartAnim.Init(partDestination, () =>
            {
                var go = GetGOFromTypeAndVersion(part.ObjectType, part.Version);
                go.SetActive(true);
                CheckBotCompleted();
            }); 
        }

        public void EnableAnimation()
        {
            //_animator.SetTrigger("Walking");

            var sequence = DOTween.Sequence();
            sequence.Insert(0, transform.DOMove(-transform.forward * 7, 1));
            sequence.InsertCallback(1, () =>
            {
                //_animator.SetTrigger("Jumping");
                _collider.enabled = false;
                _body.isKinematic = false;
                _body.useGravity = true;
                _body.AddForce(transform.forward * 20, ForceMode.Impulse);
            });
            sequence.OnComplete(() =>
            {
                StartCoroutine(DestroyAfterTime());
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
            C_CameraShakeSystem.Instance.Shake();
            EnableAnimation();

            SoundManager.PlaySound(SoundList.Sound.droprobot,priority:true);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManagement>().AddOneToCount();
        }

        private IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(5);

            OnBotComplete?.Invoke(this, null);
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag.Equals("Floor"))
            {
                SoundManager.PlaySound(SoundList.Sound.robotclonc);
                _body.isKinematic = true;
            }
        }
    }
}
