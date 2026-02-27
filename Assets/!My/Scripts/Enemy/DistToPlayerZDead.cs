using System;
using UnityEngine;

public class DistToPlayerZDead : MonoBehaviour
{
    [SerializeField] private float distToPlayerZ = 1f;
    [SerializeField] private GameObject deadGO;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (transform.position.z - player.position.z < distToPlayerZ)
            Dead();
    }

    private void Dead()
    {
        Instantiate(deadGO, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
