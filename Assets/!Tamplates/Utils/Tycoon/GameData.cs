using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[Serializable]
public class GameData
{
     public IntContainer LifeContainer;

    [Space, Header("Effects")]
    public float HitStopPlayerHit = 0.5f;
    public Vector3 CameraShakePunch = Vector3.one;

    [Space, Header("Game Parametrs")]
    public float RiseWeight = 0.2f;
    public float RiseSpeedFwd = 0.2f;
    public float FinalDiatance = 2000;

    [Space]
    public ChoiseCharacterData[] CharacterDatas;

    /// <summary>
    /// Возвращает полностью защищённый фасад
    /// </summary>
    public ReadOnlyGameData GetReadOnly()
    {
        return ReadOnlyGameData.Create(this);
    }
}

public class ReadOnlyGameData
{
    public ReadOnlyIntContainer LifeContainer { get; private set; }

    public float HitStopPlayerHit { get; private set; }
    public float RiseWeight { get; private set; }
    public float RiseSpeedFwd { get; private set; }
    public float FinalDiatance { get; private set; }

    public Vector3 CameraShakePunch { get; private set; }

    public ChoiseCharacterData[] CharacterDatas { get; private set; }

    // Приватный конструктор, чтобы не создавать объект напрямую
    private ReadOnlyGameData() { }

    /// <summary>
    /// Фабричный метод для создания ReadOnlyGameData
    /// из GameData.
    /// </summary>
    public static ReadOnlyGameData Create(GameData data)
    {
        var readOnly = new ReadOnlyGameData
        {
            LifeContainer = new ReadOnlyIntContainer(data.LifeContainer),
            HitStopPlayerHit = data.HitStopPlayerHit,
            RiseWeight = data.RiseWeight,
            RiseSpeedFwd = data.RiseSpeedFwd,
            FinalDiatance = data.FinalDiatance,
            CameraShakePunch = data.CameraShakePunch,
            CharacterDatas = data.CharacterDatas
        };
        return readOnly;
    }
}