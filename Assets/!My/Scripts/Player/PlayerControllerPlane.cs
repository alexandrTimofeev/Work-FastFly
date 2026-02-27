
using System;
using System.Collections;
using UnityEngine;

public class PlayerControllerPlane : MonoBehaviour
{
    public float SpeedFwd = 1f;
    [SerializeField] private float speedMove = 1f;
    [SerializeField] private Vector2 zoneMove = new Vector2(10, 10);

    private IInput input;

    public void Init(IInput input)
    {
        this.input = input;

        input.OnMoved += MovedWork;
    }

    private void FixedUpdate()
    {
    }

    private void Update()
    {
        if (GamePause.IsPause)
            return;

        transform.position += Vector3.forward * SpeedFwd * Time.deltaTime;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -zoneMove.x / 2f, zoneMove.x / 2f),
            Mathf.Clamp(transform.position.y, -zoneMove.y / 2f, zoneMove.y / 2f),
            transform.position.z);
    }

    private void MovedWork(IInput.InputMoveScreenInfo info)
    {
        if (GamePause.IsPause)
            return;

        if (info.PositionOnScreen.x < Screen.width / 2f)
            return;

        transform.position += (Vector3)info.Delta * speedMove * Time.deltaTime;
    }

    private void OnDestroy()
    {
        if (input != null)
            input.OnMoved -= MovedWork;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(0, 0, transform.position.z), zoneMove);
    }
}