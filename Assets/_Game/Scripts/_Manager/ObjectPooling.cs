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
}
