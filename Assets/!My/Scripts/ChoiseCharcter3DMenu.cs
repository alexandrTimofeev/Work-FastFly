using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChoiseCharcter3DMenu : MonoBehaviour
{
    [SerializeField] private ChoiseCharacter3DMenuData[] characters;
    [SerializeField] private Color colorNo;
    [SerializeField] private Material materialNo;
    [SerializeField] private TextMeshProUGUI infoTmp;
    private int selectIndex;

    private void Start()
    {
        string startID = !string.IsNullOrEmpty(ChoiseCharcterSystem.CurrentID) ? ChoiseCharcterSystem.CurrentID : characters[0].ID;
        Select(startID);
        selectIndex = characters.ToList().FindIndex(x => x.ID == startID);
    }

    private void Select(string ID)
    {
        TrySelectCharacter(ID);
    }

    private void Select(int v)
    {
        Select(characters[v].ID);
    }

    public void ClickNext (int offset)
    {
        selectIndex += offset;

        if (selectIndex < 0)        
            selectIndex = characters.Length - 1;        
        else if (selectIndex >= characters.Length)
            selectIndex = 0;

        Select(selectIndex);
    }

    public void TrySelectCharacter(string ID)
    {
        foreach (var choiseCharacter in characters)
        {
            choiseCharacter.CharacterPrefab.SetActive(choiseCharacter.ID == ID);
            if (choiseCharacter.ID == ID)
            {

                if (string.IsNullOrEmpty(choiseCharacter.AchivementID) ||
                    AchieviementSystem.IsUnlockAchiv(choiseCharacter.AchivementID, true))
                {
                    SelectVisual(choiseCharacter, true);
                    ChoiseCharcterSystem.SelectCharacter(ID);
                }
                else
                {
                    SelectVisual(choiseCharacter, false);
                }
            }
        }
    }

    private void SelectVisual(ChoiseCharacter3DMenuData choiseCharacter, bool isSelect)
    {
        Color color = isSelect ? Color.white : colorNo;
        foreach (var renderer in choiseCharacter.Renderers)
        {
            if(!isSelect)
                renderer.material = materialNo;
            renderer.material.color =  color;
        }

        infoTmp.text = string.IsNullOrEmpty(choiseCharacter.AchivementID) ? "" : $"Unlock by achivement: " +
            $"\n<size=150%><color=blue>{AchieviementSystem.GetAchivInfo(choiseCharacter.AchivementID).Title}";
    }
}

[Serializable]
public class ChoiseCharacter3DMenuData
{
    public string ID;
    public GameObject CharacterPrefab;
    public MeshRenderer[] Renderers;

    [Space]
    public string AchivementID;
}