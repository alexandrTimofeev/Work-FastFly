using DG.Tweening;
using System;
using UnityEngine;

// EntryPoint сцены Game
public class GameEntryPoint : ISceneEntryPoint
{
    public string SceneName => GameSceneManager.GameSceneName;
    public void InitGSystems() => GameG.Init();
    public void OnSceneLoaded()
    {
        Debug.Log("Game scene loaded");

        GameG.SessionData.Init(G.GlobalData);
        PocketRandomazer.Clear();

        InitButtonMediator();
        InitInterface();
        InitInterfaceCommands();
        InitPlayer();
        InitFinal();
        InitKillable();
        InitBonusMediator();
        InitLevel();

        AudioManager.Init();

        GameG.PatternFFly.targetTest = GameG.Player.transform;

        GamePause.SetPause(false);
    }

    private static void InitInterface()
    {
        InterfaceManager.Init();
        //GameG.ScoreSys.OnAddScore += (score, point) => InterfaceManager.CreateScoreFlyingText(score, point);

        InterfaceManager.BarMediator.ShowForID("Score", 0);
        GameG.ScoreSys.OnScoreChange += (info) =>
        {
            InterfaceManager.BarMediator.ShowForID("Score", info.Value);
            if(info.Point.HasValue)
                InterfaceManager.CreateScoreFlyingText(info.Delta, info.Point.Value);
        };

        InitReloadingUI(); 

        InterfaceManager.BarMediator.ShowForID("BulletsCount", GameG.Player.Gun.CountBullets.Value);
        GameG.Player.OnCountBulletChange += (value) => InterfaceManager.BarMediator.ShowForID("BulletsCount", value);
        GameG.Player.OnReloadingTimeChange += (wait) => InterfaceManager.BarMediator.ShowForID("Reloading", 
            (GameG.Player.Gun.ReloadingTime - wait) / GameG.Player.Gun.ReloadingTime);

        InterfaceManager.BarMediator.ShowForID("Life", GameG.SessionData.LifeContainer.Value);
        GameG.SessionData.LifeContainer.OnChangeValue += (value) => InterfaceManager.BarMediator.ShowForID("Life", value);
    }

    private void InitButtonMediator()
    {
        GameG.ButtonGameMediator.OnClick += (actionInfo) =>
        {
            switch (actionInfo.ActionType)
            {
                case ButtonGameActionType.None:
                    break;
                case ButtonGameActionType.Delite:
                    break;
            }
        };
    }

    public void OnSceneUnloaded()
    {
    }

    private void InitPlayer()
    {
        GameG.Player.Init(G._Input);
        GameG.Player.OnGrap += (grapOb) => GameG.GrappableObjectMediator.Invoke(grapOb);

        InitPlayerDamage();
    }

    private static void InitReloadingUI()
    {
        GameG.Player.OnStartReload += OnStartReloadUIRection;
        GameG.Player.OnEndReload += OnEndReloadUIRection;
    }
    private static void OnStartReloadUIRection()
    {
        GameG.GameSessionManagerMono.ReloadState(true);
    }
    private static void OnEndReloadUIRection()
    {
        GameG.GameSessionManagerMono.ReloadState(false);
    }

    private void InitPlayerDamage()
    {
        GameG.Player.OnDamage += (d) =>
        {
            EffectsManagerMono.HitStop(G.GlobalData.HitStopPlayerHit, 0.18f);
            EffectsManagerMono.CameraShake(G.GlobalData.CameraShakePunch, isIndependentUpdate: true);
            GameG.SessionData.LifeContainer.RemoveValue(1);
        };

        GameG.SessionData.LifeContainer.OnDownfullValue += (d) => Lose();
    }

    private void InitFinal()
    {
        GameG.Player.OnFinal += () =>
        {
            InterfaceManager.ShowWinWindow(GameG.ScoreSys.Score, LeaderBoard.GetBestScore());
            LeaderBoard.SaveScore($"Level{LevelSelectWindow.CurrentLvl}", GameG.ScoreSys.Score);
            GameG.Player.gameObject.SetActive(false);
            LevelSelectWindow.CompliteLvl();
        };
    }

    private void Lose()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            InterfaceManager.ShowLoseWindow(GameG.ScoreSys.Score, LeaderBoard.GetBestScore());
            GameG.Player.gameObject.SetActive(false);
        });
    }

    private void InitKillable()
    {
        KillableMediator.Init();

        KillableMediator.OnDead += (ko) =>
        {
            KillableDeadData deadData = ko.DeadData;
            GameG.ScoreSys.AddScore(deadData.ScoreForKill, ko.transform.position);
        };
    }

    private void InitBonusMediator()
    {
        GameG.BonusExecuter.Init();
    }

    public void InitInterfaceCommands()
    {
        InterfaceManager.OnClickCommand += (command) =>
        {
            switch (command)
            {
                case InterfaceComand.OpenPause:
                    GameG.GameSessionManagerMono.Pause(true);
                    break;
                case InterfaceComand.ClosePause:
                    GameG.GameSessionManagerMono.Pause(false);
                    break;

                default:
                    break;
            }
        };
    }

    private void InitLevel()
    {
        GameG.GameSessionManagerMono.SetLevelData(GameG.CurrentLevelData);
    }
}