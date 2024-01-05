using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainSheetBase<Data> : MonoBehaviour
{
    public List<Data> listData = new List<Data>();
    public List<SlotBase<Data>> slotBases = new List<SlotBase<Data>>();
    public SlotBase<Data> slotPref;
    public Transform slotParents;

    UnityAction<SlotBase<Data>> actionCallBackSlot;
    SlotBase<Data> slotBaseTemp;

    public virtual void SetActionCallBackOnSlot(UnityAction<SlotBase<Data>> actionCallBackSlot) { this.actionCallBackSlot = actionCallBackSlot; }

    public virtual void SetData(List<Data> data) {
        listData = data;
    }

    public virtual void LoadData() {

        for (int i = 0; i < listData.Count; i++)
        {
            slotBaseTemp = GetSlotBase();
            slotBaseTemp.InitData(listData[i]);
            slotBaseTemp.SetActionCallback(actionCallBackSlot);
        }

        for (int i = listData.Count; i < slotBases.Count; i++)
        {
            slotBases[i].gameObject.SetActive(false);
        }
        slotIndex = 0;
        StartCoroutine(IEAnimation());
    }
    int slotIndex = 0;
    IEnumerator IEAnimation() {
        while (slotIndex<slotBases.Count)
        {
            slotBases[slotIndex].PlaySequence();
            slotIndex++;
            yield return new WaitForSeconds(.1f);
        }
    }

    SlotBase<Data> GetSlotBase() {

        for (int i = 0; i < slotBases.Count; i++)
        {
            if (!slotBases[i].gameObject.activeSelf)
                return slotBases[i];
        }
        SlotBase<Data> newSlot = Instantiate(slotPref, slotParents);
        slotBases.Add(newSlot);
        return newSlot;

    }

    public virtual void ReloadDataAll() { }
    public virtual void ReloadDataOnSlot(Data slotData) { }
    public virtual void KillSequence() {
        StopCoroutine(IEAnimation());
        for (int i = 0; i < slotBases.Count; i++)
        {
            if (slotBases[i].gameObject.activeSelf)
            {
                slotBases[i].StopSequence();
            }
        }
    }
}
