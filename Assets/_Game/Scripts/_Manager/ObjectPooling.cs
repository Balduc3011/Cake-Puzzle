using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public Transform groupCakeParents;
    public List<GroupCake> groupCakes = new List<GroupCake>();
    public GroupCake groupCakePref;

    public GroupCake GetGroupCake() {
        for (int i = 0; i < groupCakes.Count; i++) {
            if (!groupCakes[i].gameObject.activeSelf) return groupCakes[i];
        }
        GroupCake newGroupCake = Instantiate(groupCakePref, groupCakeParents);
        groupCakes.Add(newGroupCake);
        return newGroupCake;
    }
}
