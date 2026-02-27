using System;
using System.Collections;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [SerializeField] private Transform pointShoot;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject shootVFX;

    [Space]
    [SerializeField] private float speedRotate = Mathf.Infinity;
    [SerializeField] private int countToReload = 1000;
    private int countShoot;
    [SerializeField] private float reloadTime = 5f;

    [Space]
    [SerializeField] private float distToAttack = 40f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private bool onlyOneBullet = true;

    private Transform player;
    private GameObject lastBullet;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(EnemyShootRoutine());
    }

    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(player.position - transform.position), speedRotate * Time.deltaTime);
    }

    private IEnumerator EnemyShootRoutine()
    {
        while (true)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= distToAttack)
            {
                Shoot();
            }
            if(onlyOneBullet)
                yield return new WaitWhile(() => lastBullet != null);
            if(countShoot >= countToReload)
            {
                countShoot = 0;
                yield return new WaitForSeconds(reloadTime);
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null)
            return;

        lastBullet = Instantiate(bulletPrefab, pointShoot.position, pointShoot.rotation);

        if (shootVFX != null)
        {
            GameObject vfx = Instantiate(shootVFX, pointShoot.position, pointShoot.rotation);
            Destroy(vfx, 2f);
        }

        countShoot++;
    }
}
