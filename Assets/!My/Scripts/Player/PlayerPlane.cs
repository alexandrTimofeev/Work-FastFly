using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPlane : MonoBehaviour
{
    public PlayerControllerPlane ControllerPlane;
    private IInput input;

    [Space]
    [SerializeField] private GunPlayerFFly gun;
    private GunPlayerFFly gunSimple;
    [SerializeField] private GunPlayerFFly gunSuperHot;
    private bool isHoldShoot;

    [Space]
    [SerializeField] private List<GameObject> meshesForCharacters = new List<GameObject>();
    [SerializeField] private List<DamageHurtBox3D> damageHurtBoxes;
    [SerializeField] private GrapCollider grapCollider;
    [SerializeField] private List<StateToGO> stateToGOs = new List<StateToGO>();

    private StateTimeExecuter stateTimeExecuter = new StateTimeExecuter();

    public GunPlayerFFly Gun => gun;

    public event Action<DamageHitBox3D> OnDamage;
    public event Action OnFinal;

    public event Action<int> OnCountBulletChange;
    public event Action OnStartReload;
    public event Action OnEndReload;
    public event Action<float> OnReloadingTimeChange;

    public event Action<GrapObject> OnGrap;

    public void Init (IInput input)
    {
        this.input = input;

        ControllerPlane.Init(input);
        input.OnBegan += BeganWork;
        input.OnEnded += EndedWork;

        gunSimple = gun;
        SetGun(gun);

        foreach (var b in damageHurtBoxes)
        {
            if (b.gameObject.activeSelf)
            {
                b.OnDamage += DamageWork;
                grapCollider = b.GetComponent<GrapCollider>();
                break;
            }
        }

        grapCollider.OnGrap += OnGrap;

        InitBonusStates();

        //Debug.Log($"Character {ChoiseCharcterSystem.CurrentID}");
        for (int i = 0; i < G.GlobalData.CharacterDatas.Length; i++)
        {
            ChoiseCharacterData chData = G.GlobalData.CharacterDatas[i];
            if (chData.ID == ChoiseCharcterSystem.CurrentID)
            {
                InitCharacterMehes(i);
                break;
            }
        }        
    }

    private void InitCharacterMehes(int num)
    {
        Debug.Log($"InitCharacterMehes {num}");
        for (int i = 0; i < meshesForCharacters.Count; i++)
        {
            GameObject go = meshesForCharacters[i];
            go.SetActive(num == i);
        }
    }

    private void Update()
    {
        if (GamePause.IsPause)
            return;

        if (isHoldShoot)
            Shoot(true);

        stateTimeExecuter.Update();
    }

    private void BeganWork(Vector2 vector)
    {
        if (GamePause.IsPause)
            return;
        if (vector.x > Screen.width / 2f)
            return;

        isHoldShoot = true;

        Shoot(false);
    }

    private void EndedWork(Vector2 vector)
    {
        if (GamePause.IsPause)
            return;
        if (vector.x > Screen.width / 2f)
            return;

        isHoldShoot = false;
    }

    private void Shoot(bool isHold)
    {
        if(gun == null)
            Debug.LogError("Gun is null");

        gun.TryShoot(isHoldShoot);
    }

    private void DamageWork(DamageHitBox3D d)
    {
        if (stateTimeExecuter.IsStateWork("Invictible"))
            return;

        OnDamage?.Invoke(d);
        Invictible(1f);
    }

    public void RiseSpeed(float v)
    {
        ControllerPlane.SpeedFwd += v;
    }

    public void Final()
    {
        OnFinal?.Invoke();  
    }

    public void SetGun (GunPlayerFFly selectGun)
    {
        if(gun != null)
        {
            gun.OnCountBulletChange -= (v) => OnCountBulletChange.Invoke(v);
            gun.OnStartReload -= OnStartReload;
            gun.OnEndReload -= OnEndReload;
            gun.OnReloadingTimeChage -= (v) => OnReloadingTimeChange.Invoke(v);
        }

        gun = selectGun;

        if (selectGun == null)
            return;

        selectGun.OnCountBulletChange += (v) => OnCountBulletChange.Invoke(v);
        selectGun.OnStartReload += OnStartReload;
        selectGun.OnEndReload += OnEndReload;
        selectGun.OnReloadingTimeChage += (v) => OnReloadingTimeChange.Invoke(v);

        gun.AdditionSpeed = ControllerPlane.SpeedFwd;
    }

    public void Invictible(float duration)
    {
        stateTimeExecuter.StartState("Invictible", duration, StateTimeOverradeType.OverradeTimeIfGrater);
    }
    public void SlowMo(float duration)
    {
        stateTimeExecuter.StartState("SlowMo", duration, StateTimeOverradeType.OverradeTimeIfGrater);
    }
    public void SuperHot(float duration)
    {
        stateTimeExecuter.StartState("SuperHot", duration, StateTimeOverradeType.OverradeTimeIfGrater);
    }

    private void InitBonusStates()
    {
        stateTimeExecuter.OnStartState += (state) =>
        {
            switch (state.ID)
            {
                case "Invictible":
                    damageHurtBoxes.ForEach(b => b.enabled = false);
                    break;
                case "SlowMo":
                    Time.timeScale = 0.5f;
                    break;
                case "SuperHot":
                    SetGun(gunSuperHot);
                    break;
                default:
                    break;
            }

            stateToGOs.ForEach(s => s.StartState(state.ID));
        };

        stateTimeExecuter.OnDeltaTimeState += (state) =>
        {
            switch (state.ID)
            {
                case "Invictible":
                    break;
                case "SlowMo":
                    break;
                case "SuperHot":
                    break;
                default:
                    break;
            }
        };

        stateTimeExecuter.OnEndState += (state) =>
        {
            switch (state.ID)
            {
                case "Invictible":
                    damageHurtBoxes.ForEach(b => b.enabled = true);
                    break;
                case "SlowMo":
                    Time.timeScale = 1f;
                    break;
                case "SuperHot":
                    SetGun(gunSimple);
                    break;
                default:
                    break;
            }

            stateToGOs.ForEach(s => s.EndState(state.ID));
        };
    }

    private void OnDestroy()
    {
        input.OnBegan -= BeganWork;
        input.OnEnded -= EndedWork;
        SetGun(null);
    }

    [Serializable]
    public class StateToGO
    {
        public string StateID;
        public GameObject GO;

        public void StartState(string stateID)
        {
            if (StateID == stateID)
                GO.SetActive(true);
        }   

        public void EndState(string stateID)
        {
            if (StateID == stateID)
                GO.SetActive(false);
        }
    }
}

