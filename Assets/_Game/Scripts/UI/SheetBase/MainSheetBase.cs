using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainSheetBase<Data> : MonoBehaviour
{
    public List<SlotBase<Data>> listSlots;
    public SlotBase<Data> slotPref;
    public Transform trsParents;
    SlotBase<Data> slotTemp;
    UnityAction<SlotBase<Data>> actionCallback;
    public virtual void LoadData(List<Data> datas)
    {
        for (int i = 0; i < listSlots.Count; i++)
            listSlots[i].gameObject.SetActive(false);
        for (int i = 0; i < datas.Count; i++)
        {
            slotTemp = GetSlotFree();
            slotTemp.gameObject.SetActive(true);
            slotTemp.InitData(datas[i]);
            slotTemp.SetActionCallback(ActioncallBackOnSlot);
        }
    }
    public void SetActionCallBack(UnityAction<SlotBase<Data>> actionCallback) { this.actionCallback = actionCallback; }
    void ActioncallBackOnSlot(SlotBase<Data> slot)
    {
        if (actionCallback != null)
            actionCallback(slot);
    }

    public SlotBase<Data> GetSlotFree()
    {
        for (int i = 0; i < listSlots.Count; i++)
        {
            if (!listSlots[i].gameObject.activeSelf)
                return listSlots[i];
        }
        SlotBase<Data> newSlot = Instantiate(slotPref, trsParents);
        listSlots.Add(newSlot);
        return newSlot;
    }
}
