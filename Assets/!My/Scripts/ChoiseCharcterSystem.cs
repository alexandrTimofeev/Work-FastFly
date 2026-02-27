using System;
using System.Collections.Generic;
using UnityEngine;

public static class ChoiseCharcterSystem
{
    public static string CurrentID { get; private set; }
    private static List<ChoiseCharacterVisualGO> choiseCharacterVisualGOs = new List<ChoiseCharacterVisualGO>();

    public static event Action<string> OnSelectCharacter;

    public static void SelectCharacter(string ID)
    {
        CurrentID = ID;
        choiseCharacterVisualGOs.ForEach(go => go.UpdateChoice(ID));
        OnSelectCharacter?.Invoke(ID);

        Debug.Log($"CurrentID Character {CurrentID}");
    }

    public static void AddCharcterObject(ChoiseCharacterVisualGO choiseCharacterVisualGO)
    {
        choiseCharacterVisualGOs.Add(choiseCharacterVisualGO);
    }

    public static void RemoveCharcterObject(ChoiseCharacterVisualGO choiseCharacterVisualGO)
    {
        choiseCharacterVisualGOs.Remove(choiseCharacterVisualGO);
    }
}

[Serializable]
public class ChoiseCharacterData
{
    public string ID;
    public string Name;
}

public class ChoiseCharacterVisualGO : MonoBehaviour
{
    public string ID;

    public void Start()
    {
        ChoiseCharcterSystem.AddCharcterObject(this);
        UpdateChoice(ChoiseCharcterSystem.CurrentID);
    }

    private void OnDestroy()
    {
        ChoiseCharcterSystem.RemoveCharcterObject(this);
    }

    public void UpdateChoice (string id)
    {
        gameObject.SetActive(ID == id);
    }
}