using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public Transform groupCakeParents;
    public List<GroupCake> groupCakes = new List<GroupCake>();
    public GroupCake groupCakePref;

    public GroupCake GetGroupCake() {
        for (int i = groupCakes.Count - 1; i >= 0; i--)
        {
            if (groupCakes[i] == null)
            {
                groupCakes.RemoveAt(i);
                continue;
            }
            if (!groupCakes[i].gameObject.activeSelf) return groupCakes[i];
        }
        GroupCake newGroupCake = Instantiate(groupCakePref, groupCakeParents);
        groupCakes.Add(newGroupCake);
        return newGroupCake;
    }

    public void CheckGroupCake() {
        for (int i = groupCakes.Count - 1; i >= 0; i--) {
            if (groupCakes[i] == null)
            {
                groupCakes.RemoveAt(i);
                continue;
            }
            groupCakes[i].CheckGroupDone();
        }
    }

    public Transform trsEffect;
    public List<Transform> cakeDoneEffects = new List<Transform>();
    public Transform cakeDoneEffect;

    public Transform GetCakeDoneEffect()
    {
        for (int i = cakeDoneEffects.Count - 1; i >= 0; i--)
        {
            if (cakeDoneEffects[i] == null)
            {
                cakeDoneEffects.RemoveAt(i);
                continue;
            }
            if (!cakeDoneEffects[i].gameObject.activeSelf) return cakeDoneEffects[i];
        }
        Transform newEffect = Instantiate(cakeDoneEffect, trsEffect);
        cakeDoneEffects.Add(newEffect);
        return newEffect;
    }

    public List<Transform> pieceDoneEffects = new List<Transform>();
    public Transform piecesEffect;

    public Transform GetPieceDoneEffect()
    {
        for (int i = pieceDoneEffects.Count - 1; i >= 0; i--)
        {
            if (pieceDoneEffects[i] == null)
            {
                pieceDoneEffects.RemoveAt(i);
                continue;
            }
            if (!pieceDoneEffects[i].gameObject.activeSelf) return pieceDoneEffects[i];
        }
        Transform newEffect = Instantiate(piecesEffect, trsEffect);
        pieceDoneEffects.Add(newEffect);
        return newEffect;
    }

    public Transform trsPanel;
    public List<ExpEffect> expEffects = new List<ExpEffect>();
    public ExpEffect expEffect;
    

    public ExpEffect GetExpEffect()
    {
        for (int i = expEffects.Count - 1; i >= 0; i--)
        {
            if (expEffects[i] == null)
            {
                expEffects.RemoveAt(i);
                continue;
            }
            if (!expEffects[i].gameObject.activeSelf) return expEffects[i];
        }
        ExpEffect newEffect = Instantiate(expEffect, trsPanel);
        expEffects.Add(newEffect);
        return newEffect;
    }

    public List<EffectMove> expEffectMoves = new List<EffectMove>();
    public EffectMove expEffectMove;


    public EffectMove GetEffectMove()
    {
        for (int i = expEffectMoves.Count - 1; i >= 0; i--)
        {
            if (expEffectMoves[i] == null)
            {
                expEffectMoves.RemoveAt(i);
                continue;
            }
            if (!expEffectMoves[i].gameObject.activeSelf) return expEffectMoves[i];
        }
        EffectMove newEffect = Instantiate(expEffectMove, trsPanel);
        expEffectMoves.Add(newEffect);
        return newEffect;
    }

    public List<EffectAdd> effectAddExps = new List<EffectAdd>();
    public EffectAdd effectAddExp;


    public EffectAdd GetEffectExp()
    {
        for (int i = effectAddExps.Count - 1; i >= 0; i--)
        {
            if (effectAddExps[i] == null)
            {
                effectAddExps.RemoveAt(i);
                continue;
            }
            if (!effectAddExps[i].gameObject.activeSelf) return effectAddExps[i];
        }
        EffectAdd newEffect = Instantiate(effectAddExp, trsPanel);
        effectAddExps.Add(newEffect);
        return newEffect;
    }

    public List<CoinEffect> effectCoins = new List<CoinEffect>();
    public CoinEffect effectCoin;


    public CoinEffect GetCoinEffect()
    {
        for (int i = effectCoins.Count - 1; i >= 0; i--)
        {
            if (effectCoins[i] == null)
            {
                effectCoins.RemoveAt(i);
                continue;
            }
            if (!effectCoins[i].gameObject.activeSelf) return effectCoins[i];
        }
        CoinEffect newEffect = Instantiate(effectCoin, trsPanel);
        effectCoins.Add(newEffect);
        return newEffect;
    }

    public List<StreakEffect> streakEffects = new List<StreakEffect>();
    public StreakEffect streakEffect;
    public StreakEffect GetStreakEffect()
    {
        for (int i = streakEffects.Count - 1; i >= 0; i--)
        {
            if (streakEffects[i] == null)
            {
                streakEffects.RemoveAt(i);
                continue;
            }
            if (!streakEffects[i].gameObject.activeSelf) return streakEffects[i];
        }
        StreakEffect newEffect = Instantiate(streakEffect, trsPanel);
        streakEffects.Add(newEffect);
        return newEffect;
    }

    public List<Transform> smokeEffects = new List<Transform>();
    public Transform smokeEffect;
    public Transform GetSmokeEffect()
    {
        for (int i = smokeEffects.Count - 1; i >= 0; i--)
        {
            if (smokeEffects[i] == null)
            {
                smokeEffects.RemoveAt(i);
                continue;
            }
            if (!smokeEffects[i].gameObject.activeSelf) return smokeEffects[i];
        }
        Transform newSmokeEffect = Instantiate(smokeEffect, transform);
        smokeEffects.Add(newSmokeEffect);
        return newSmokeEffect;
    }

}
