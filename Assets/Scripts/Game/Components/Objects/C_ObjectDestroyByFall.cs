using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Components.Objects
{
    public class C_ObjectDestroyByFall : MonoBehaviour
    {
        [SerializeField] private float _y;

        private void Update()
        {
            if(transform.position.y < _y)
            {
                Destroy(gameObject);
            }
        }
    }
}
