using System.Collections;
using UnityEngine;

public class StepMainGameState : GameStepBase
{
    public override string Id => "GameLoop";

    protected override IEnumerator OnExecute(GameLoopContext context)
    {
        GameG.PatternFFly.CreateFinal(GetFinalDistance());

        while (true)
        {
            GameG.PatternFFly.WeightOnAllLines += GetRiseWeight() * Time.deltaTime;
            GameG.Player.RiseSpeed(GetSpeedWeight() * Time.deltaTime);
            yield return null;
        }
    }

    public float GetFinalDistance()
    {
        return G.GlobalData.FinalDiatance * (GameG.CurrentLevelData ? GameG.CurrentLevelData.DistFinalKof : 1f);
    }

    public float GetRiseWeight()
    {
        return G.GlobalData.RiseWeight;
    }

    public float GetSpeedWeight()
    {
        return G.GlobalData.RiseSpeedFwd;
    }
}
