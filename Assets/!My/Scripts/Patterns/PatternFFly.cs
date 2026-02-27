using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class PatternFFly : MonoBehaviour
{
    [SerializeField] private PatternableObject[] patternableObjectsPref;
    [SerializeField] private int countLines = 3;
    [SerializeField] private float distBetweenLines = 5f;
    [SerializeField] private float distAtCenter = 3.5f;

    [Space]
    [SerializeField] private BonusObject bonusObjectPref;
    [SerializeField] private GrappbleObjectWithData[] simpleBonusPref;
    [SerializeField] private GrappbleObjectWithData[] superBonusPref;
    [SerializeField] private int bonusForLine = 2;
    [SerializeField] private int superBonusBetweenCount = 15;
    private int superBonusStep;

    [Space]
    [SerializeField] private Vector3 lowOffset;
    [SerializeField] private GameObject lowPattern;

    [Space]
    public float WeightOnAllLines = 5f;
    public float WeightCap = 1f;
    private float currentWeightOnAllLines;

    [Space]
    public GameObject finalGO;

    Vector3 lastDistance;
    [HideInInspector] public Transform targetTest;

    private void Start()
    {
        lastDistance = transform.position;
        superBonusStep = superBonusBetweenCount;

        PocketRandomazer.CreatePocket("SuperBonus", superBonusPref);

        GenerateAllLines();
        Generate();
    }

    private void Update()
    {
        if (lastDistance.z - targetTest.position.z < 5f)
            Generate();
    }

    public void Generate()
    {
        lastDistance += transform.forward * countLines * distBetweenLines;
        currentWeightOnAllLines = WeightOnAllLines;
        GenerateAllLines();

        WeightCap += 1.5f;
    }

    private void GenerateAllLines()
    {
        for (int i = 0; i < countLines; i++)
        {
            Vector3 posLine = lastDistance + (transform.forward * i * distBetweenLines);
            GenerateLineInPos(posLine);
        }
        Instantiate(lowPattern, lastDistance + lowOffset - (transform.forward * 2 * distBetweenLines), transform.rotation);
    }

    private void GenerateLineInPos(Vector3 posLine)
    {
        GeneratePattern(posLine + (transform.right * distAtCenter));
        GeneratePattern(posLine);
        GeneratePattern(posLine - (transform.right * distAtCenter));

        Vector3[] points = GetPointsForBonus(posLine, bonusForLine);
        for (int i = 0; i < points.Length; i++)
            BonusGenerate(points[i]);
    }

    private void GeneratePattern(Vector3 pos)
    {
        PatternableObject patternable = GetRandomPatternObject();
        PatternableObject pattOb = Instantiate(patternable, pos, transform.rotation);
        pattOb.Init(targetTest);

        currentWeightOnAllLines -= patternable.Weight;
    }

    private PatternableObject GetRandomPatternObject(bool onlyInCurrentWeight = true)
    {
        PatternableObject patternableObject = patternableObjectsPref[Random.Range(0, patternableObjectsPref.Length)];
        if(onlyInCurrentWeight)
            while (patternableObject.Weight > currentWeightOnAllLines || patternableObject.Weight > WeightCap)            
                patternableObject = patternableObjectsPref[Random.Range(0, patternableObjectsPref.Length)];            
        return patternableObject;
    }

    public void CreateFinal(float v)
    {
        Instantiate(finalGO, transform.position + (transform.forward * v), transform.rotation);
    }

    private void BonusGenerate(Vector3 pos)
    {
        superBonusStep--;

        if (superBonusStep <= 0)
        {
            superBonusStep = superBonusBetweenCount;
            CreateBonus(pos, PocketRandomazer.GetRandomElement<GrappbleObjectWithData>("SuperBonus"));
        }
        else
            CreateBonus(pos, simpleBonusPref);        
    }

    private void CreateBonus(Vector3 pos, params GrappbleObjectWithData[] datas)
    {
        BonusObject bonus = Instantiate(bonusObjectPref, pos, transform.rotation);
        bonus.Init(datas[Random.Range(0, datas.Length)]);
    }

    private Vector3[] GetPointsForBonus(Vector3 pointLine, int count)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = GetOffset(pointLine);
            while (points.Any((p3) => Vector3.Distance(p3, pos) < 15f))
                pos = GetOffset(pointLine);
            points.Add(pos);
        }

        return points.ToArray();

        Vector3 GetOffset(Vector3 pointLine)
        {
             return pointLine + (transform.right * Random.Range(-distAtCenter, distAtCenter)) +
                (transform.up * Random.Range(-15f, 10f) + (transform.forward * distBetweenLines / 2f));
        }
    }
}
