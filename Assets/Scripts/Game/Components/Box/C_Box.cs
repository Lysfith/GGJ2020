using Assets.Scripts.Game.Components.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Components.Box
{
    public class C_Box : MonoBehaviour
    {
        [SerializeField] private Rigidbody _body;

        private void OnEnable()
        {
            _body = GetComponent<Rigidbody>();
        }

        public void EnablePhysic()
        {
            _body.isKinematic = false;
            _body.useGravity = true;
        }

        public void ApplyForce(Vector3 force)
        {
            _body.AddForce(force);
        }

        public void Open(GameObject player)
        {
            SoundManager.PlaySound(SoundList.Sound.boxopen, player);
            var levelSystem = FindObjectOfType<C_LevelSystem>();
            levelSystem.SpawnBoxObject(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag.Equals("Floor"))
            {
                gameObject.layer = LayerMask.NameToLayer("Object");
            }
        }
    }
}
