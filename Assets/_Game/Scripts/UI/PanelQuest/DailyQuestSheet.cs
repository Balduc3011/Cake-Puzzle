using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyQuestSheet : SheetBase<QuestData>
{
    List<QuestSlot> questSlots = new List<QuestSlot>();
    public override void LoadData(List<QuestData> datas)
    {
        base.LoadData(datas);
        for (int i = 0; i < listSlots.Count; i++)
        {
            AddToList(listSlots[i].GetComponent<QuestSlot>());
        }
        SortSlot();
    }

    public void AddToList(QuestSlot slot)
    {
        questSlots.Add(slot);
    }

    private void OnEnable()
    {
        ReOrderSlots();
        SortSlot();
    }

    void ReOrderSlots()
    {
        for (int i = 0; i < questSlots.Count; i++)
        {
            questSlots[i].transform.SetAsLastSibling();
        }
    }

    void SortSlot()
    {
        for (int i = 0; i < questSlots.Count; i++)
        {
            questSlots[i].CheckCollect();
        }
    }
}
