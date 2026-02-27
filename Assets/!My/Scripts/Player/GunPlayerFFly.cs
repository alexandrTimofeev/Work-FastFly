using System;
using System.Collections;
using UnityEngine;

public class GunPlayerFFly : MonoBehaviour
{
    [SerializeField] private BulletFFly bulletGO;
    [SerializeField] private float pauseTime = 0.2f;
    [SerializeField] private bool shootIsHold;

    [Space]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject vfxShoot;

    [Space]
    [SerializeField] private bool isNeedReload = true;
    public IntContainer CountBullets = new IntContainer(50, new Vector2Int(0, 50));
    [SerializeField] private float reloadingTime = 2f;
    public float ReloadingTime => reloadingTime;

    [HideInInspector] public float AdditionSpeed;

    private bool isPause;
    private bool isReloading;
    public bool IsCanShoot => !isPause && !isReloading;

    public event Action<int> OnCountBulletChange;
    public event Action OnStartReload;
    public event Action OnEndReload;
    public event Action<float> OnReloadingTimeChage;

    private void Start()
    {
        CountBullets.OnChangeValue += OnCountBulletChange;

        StartCoroutine(ReloadAndPauseEn());
    }

    public BulletFFly TryShoot(bool isHold)
    {
        if (!IsCanShoot)
            return null;

        if (isHold && !shootIsHold)
            return null;

        BulletFFly bulletShoot = Shoot();
        ReloadStart();

        return bulletShoot;
    }

    private BulletFFly Shoot(bool isGetBullets = true)
    {
        BulletFFly bulletShoot = Instantiate(bulletGO, transform.position, transform.rotation);
        bulletShoot.Init(AdditionSpeed);

        if (isGetBullets)
            CountBullets.RemoveValue(1);

        Instantiate(vfxShoot, shootPoint.position, shootPoint.rotation);

        return bulletShoot;
    }

    public void ReloadStart()
    {
        isPause = true;
    }

    private IEnumerator ReloadAndPauseEn()
    {
        while (true)
        {
            yield return new WaitWhile(() => !isPause);

            if (isNeedReload)
            {
                if (CountBullets.Value <= 0)
                    yield return ReloadProcces();
            }

            yield return new WaitForSeconds(pauseTime);
            isPause = false;
        }
    }

    private IEnumerator ReloadProcces()
    {
        isReloading = true;

        OnStartReload?.Invoke();
        yield return new WaitForSecondsAndInvoke(reloadingTime, OnReloadingTimeChage);
        OnEndReload?.Invoke();

        CountBullets.SetValue(CountBullets.ClampRange.y);

        isReloading = false;
    }
}
