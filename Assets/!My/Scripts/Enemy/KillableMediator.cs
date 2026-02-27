using System;
using UnityEngine;

public static class KillableMediator
{
    public static Action<KillableObjectFFly> OnDead;

    public static void Init()
    {
        OnDead = null;
    }

    public static void Dead(KillableObjectFFly obj)
    {
        OnDead?.Invoke(obj);
    }
}
