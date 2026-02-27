using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateTimeExecuter
{
    public List<StateTime> StateTimes = new List<StateTime>();

    public event Action<StateTime> OnStartState;
    public event Action<StateTime> OnDeltaTimeState;
    public event Action<StateTime> OnEndState;

    #region START

    public void StartState(string id, float wait,
        StateTimeOverradeType overradeType = StateTimeOverradeType.AddTime,
        int priority = 100)
    {
        StartState(new StateTime
        {
            ID = id,
            Wait = wait,
            OverradeType = overradeType,
            priotity = priority
        });
    }

    public void StartState(StateTime newState)
    {
        var existing = StateTimes.FirstOrDefault(s => s.ID == newState.ID);

        if (existing == null)
        {
            StateTimes.Add(newState);
            Sort();
            OnStartState?.Invoke(newState);
            return;
        }

        switch (newState.OverradeType)
        {
            case StateTimeOverradeType.NotOverrade:
                return;

            case StateTimeOverradeType.OverradeTime:
                existing.Wait = newState.Wait;
                break;

            case StateTimeOverradeType.AddTime:
                existing.Wait += newState.Wait;
                break;

            case StateTimeOverradeType.OverradeTimeIfGrater:
                if (newState.Wait > existing.Wait)
                    existing.Wait = newState.Wait;
                break;

            case StateTimeOverradeType.OverradeTimeIfLess:
                if (newState.Wait < existing.Wait)
                    existing.Wait = newState.Wait;
                break;

            case StateTimeOverradeType.EndAndReload:
                EndState(existing.ID);
                StateTimes.Add(newState);
                Sort();
                OnStartState?.Invoke(newState);
                break;
        }
    }

    #endregion

    #region END

    public void EndState(string id)
    {
        var state = StateTimes.FirstOrDefault(s => s.ID == id);
        if (state == null)
            return;

        StateTimes.Remove(state);
        OnEndState?.Invoke(state);
    }

    #endregion

    #region UPDATE

    public void Update()
    {
        if (StateTimes.Count == 0)
            return;

        float dt = Time.deltaTime;

        // уменьшаем таймер
        foreach (var state in StateTimes)
        {
            state.Wait -= dt;
            OnDeltaTimeState?.Invoke(state);
        }

        // удаляем завершённые
        for (int i = StateTimes.Count - 1; i >= 0; i--)
        {
            if (StateTimes[i].Wait <= 0f)
                EndState(StateTimes[i].ID);
        }
    }

    #endregion

    private void Sort()
    {
        StateTimes = StateTimes
            .OrderBy(s => s.priotity)
            .ToList();
    }

    public bool IsStateWork(string id)
    {
        return StateTimes.Exists(s => s.ID == id);
    }
}

public enum StateTimeOverradeType
{
    NotOverrade,
    OverradeTime,
    AddTime,
    OverradeTimeIfGrater,
    OverradeTimeIfLess,
    EndAndReload
}

[Serializable]
public class StateTime
{
    public string ID;
    public float Wait;
    public StateTimeOverradeType OverradeType;
    public int priotity = 100;
}