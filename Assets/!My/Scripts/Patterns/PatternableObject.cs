using DG.Tweening;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatternableObject : MonoBehaviour
{
    public int Weight;

    [Space]
    [SerializeField] private Transform target;

    [Space]
    [SerializeField] private Vector2 rngPosXOffset = Vector2.zero;
    [SerializeField] private Vector2 rngPosYOffset = Vector2.zero;

    [Space]
    [SerializeField] private Vector2 rngRotXOffset = Vector2.zero;
    [SerializeField] private Vector2 rngRotYOffset = Vector2.zero;
    [SerializeField] private Vector2 rngRotZOffset = Vector2.zero;

    [Space]
    [SerializeField] private Vector2 rngScaleXOffset = Vector2.zero;
    [SerializeField] private Vector2 rngScaleYOffset = Vector2.zero;
    [SerializeField] private Vector2 rngScaleZOffset = Vector2.zero;

    [Space]
    [SerializeField] private bool initToStart = false;

    private Transform targetZDes;

    private void Start()
    {
        if (initToStart)
            Init(null);
    }

    public void Init(Transform targetPlayer)
    {
        float xOffset = Random.Range(rngPosXOffset.x, rngPosXOffset.y);
        float yOffset = Random.Range(rngPosYOffset.x, rngPosYOffset.y);

        float xrOffset = Random.Range(rngRotXOffset.x, rngRotXOffset.y);
        float yrOffset = Random.Range(rngRotYOffset.x, rngRotYOffset.y);
        float zrOffset = Random.Range(rngRotZOffset.x, rngRotZOffset.y);

        float xsOffset = Random.Range(rngScaleXOffset.x, rngScaleXOffset.y);
        float ysOffset = Random.Range(rngScaleYOffset.x, rngScaleYOffset.y);
        float zsOffset = Random.Range(rngScaleZOffset.x, rngScaleZOffset.y);

        target.transform.position += new Vector3(xOffset, yOffset);
        target.transform.rotation = Quaternion.Euler(target.transform.rotation.eulerAngles +  new Vector3(xrOffset, yrOffset, zrOffset));
        target.transform.localScale += new Vector3(xsOffset, ysOffset, zsOffset);

        targetZDes = targetPlayer;
    }

    private void Update()
    {
        if (targetZDes != null && transform.position.z - targetZDes.position.z < -40f)
            Destroy(gameObject);
    }
}