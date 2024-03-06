using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectAdd : MonoBehaviour
{
    UnityAction actionCallback;
    public void SetActionCallBack(UnityAction actionCallback) { 
        this.actionCallback = actionCallback;
    }

    public void DoneEffect() { 
        if (actionCallback != null) { actionCallback(); }
        Destroy(gameObject);
    }
}
