using System.Collections.Generic;
using UnityEngine;

public class MaterialAtLevel : MonoBehaviour
{
    public List<Material> materials = new List<Material>();

    void Start()
    {
        if(LevelSelectWindow.CurrentLvl < materials.Count)
            GetComponent<Renderer>().material = materials[LevelSelectWindow.CurrentLvl];
    }
}
