using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

// Глобальные системы
public static class G
{
    public static IInput _Input;
    public static ReadOnlyGameData GlobalData;

    public static void Init()
    {
        Debug.Log("G initialized (global systems)");

        _Input = InputFabric.GetOrCreateInpit(true);
        GameGlobalData.ClearInstance();
        GlobalData = GameGlobalData.Instance.Data;
    }
}

// Локальные G-системы сцены Game
public static class GameG
{
    //Stundart

    public static GameSessionData SessionData;
    //public static GameMain Main;
    public static ScoreSystem ScoreSys;
    public static ResourceManager ResourceManager;
    public static ButtonGameActionMediator ButtonGameMediator;
    public static GrappableObjectMediator GrappableObjectMediator;

    public static CellPlacer CellPlacer;

    //ForThisGame
    public static PlayerPlane Player;
    public static PatternFFly PatternFFly;
    public static GameSessionManagerMono GameSessionManagerMono;
    public static BonusExecuter BonusExecuter;
    public static LevelData CurrentLevelData;

    public static void Init()
    {
        Debug.Log("GameG initialized (scene systems)");
        InitBase();

        //ForThisGame
        Player = Object.FindAnyObjectByType<PlayerPlane>(FindObjectsInactive.Include);
        PatternFFly = Object.FindAnyObjectByType<PatternFFly>(FindObjectsInactive.Include);
        GameSessionManagerMono = Object.FindAnyObjectByType<GameSessionManagerMono>(FindObjectsInactive.Include);

        BonusExecuter = new GameObject().AddComponent<BonusExecuter>();

        CurrentLevelData = LevelData.LoadFromResources(LevelSelectWindow.CurrentLvl);
    }

    private static void InitBase()
    {
        CellPlacer = Object.FindAnyObjectByType<CellPlacer>();

        ScoreSys = new ScoreSystem();
        SessionData = new GameSessionData();
        ButtonGameMediator = new ButtonGameActionMediator();
        GrappableObjectMediator = new GrappableObjectMediator();
    }
}

// Локальные G-системы сцены Menu
public static class MenuG
{
    public static void Init()
    {
        Debug.Log("MenuG initialized (scene systems)");
    }
}