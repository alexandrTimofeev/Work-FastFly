using System;
using UnityEngine;

public class FinalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DamageHurtBox3D hurtBox3D) &&
            hurtBox3D.ColliderTags.Contains("Player"))
            GameG.Player.Final();
    }
}
