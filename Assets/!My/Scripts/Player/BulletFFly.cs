using System;
using System.Collections;
using UnityEngine;

public class BulletFFly : MonoBehaviour
{
    public float speed = 15f;

    public void Init(float additionSpeed)
    {
        speed += additionSpeed;
    }

    public void Update()
    {
        transform.position += speed * transform.forward * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}