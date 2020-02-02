using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucaHorn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SoundManager.PlaySound(SoundList.Sound.cucaracha);
    }

}
