using System;
using TMPro;
using UnityEngine;

public class GameSessionManagerMono : MonoBehaviour
{
    [SerializeField] private GameObject reloadBack;
    [SerializeField] private TextMeshProUGUI countBulletsTmp;

    public void ReloadState(bool show)
    {
        reloadBack.SetActive(show);
        countBulletsTmp.color = show ? (Color.gray - new Color(0, 0, 0, 0.5f)) : Color.white;
    }

    public void Pause(bool isPause)
    {
        GamePause.SetPause(isPause);
    }

    public void SetLevelData(LevelData levelData)
    {
        if(levelData == null)
            return;

        GameG.PatternFFly.WeightOnAllLines *= levelData.StartWeghitAllLineKof;
        GameG.PatternFFly.WeightCap *= levelData.StartWeghitCap;
    }
}
