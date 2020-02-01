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

        private bool soundtriggered = false;
        private void Update()
        {
            if(!soundtriggered && transform.position.y < _y/3)
            {
                soundtriggered = true;
                SoundManager.PlaySound(SoundList.Sound.dropsmall, priority: false, pitch:UnityEngine.Random.Range(0.7f,1.3f));
            }

            if(transform.position.y < _y)
            {
                Destroy(gameObject);
            }
        }
    }
}
