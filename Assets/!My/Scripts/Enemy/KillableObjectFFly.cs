using System;
using UnityEngine;

public class KillableObjectFFly : MonoBehaviour
{
    public DamageHurtBox3D HurtBox;
    [SerializeField] private int life = 5;
    public KillableDeadData DeadData;

    [Space]
    [SerializeField] private Transform targetHitPunch;
    [SerializeField] private DOTweenPunchData hitDOData;

    [Space]
    [SerializeField] private GameObject deadVFX;

    [Space]
    public static Action<KillableObjectFFly> OnDead;

    private void Start()
    {
        if(HurtBox)
            HurtBox.OnDamage += DamageWork;
    }

    private void DamageWork(DamageHitBox3D d)
    {
        Hurt(1);
    }

    private void Hurt(int v)
    {
        life -= v;
        if (life <= 0)
            KillImmidiatly();
        else
            hitDOData.TransformPunchScale(targetHitPunch);
    }

    private void KillImmidiatly()
    {
        OnDead?.Invoke(this);
        KillableMediator.Dead(this);
        Destroy(gameObject);

        if(deadVFX)
            Destroy(Instantiate(deadVFX, targetHitPunch.position, targetHitPunch.rotation), 10f);
    }
}

[Serializable]
public class KillableDeadData
{
    public int ScoreForKill = 100;
} 
