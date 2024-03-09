using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class SheetAnimation : MonoBehaviour
{
    [SerializeField] bool overflow;
    public ItemAnimType itemAnimType;
    public List<Transform> itemTrs;
    public List<CanvasGroup> itemCGs;
    public void PlayAnim()
    {
        switch (itemAnimType)
        {
            case ItemAnimType.PopOut:
                for (int i = 0; i < itemTrs.Count; i++)
                {
                    PlayPopOutAnim(itemTrs[i], (i + 1) * 0.1f);
                }
                break;
            case ItemAnimType.Fade:
                for (int i = 0; i < itemTrs.Count; i++)
                {
                    PlayFadeAnim(itemCGs[i], (i + 1) * 0.1f);
                }
                break;
            default:
                break;
        }
    }

    void PlayPopOutAnim(Transform item, float delay)
    {
        item.DOScale(1f, 0.25f).From(0f).SetEase(overflow? Ease.OutBack : Ease.InOutQuad).SetDelay(delay);
        //item.DOScale(1f, 0.05f).SetDelay(delay + 0.25f);
    }

    void PlayFadeAnim(CanvasGroup item, float delay)
    {
        item.DOFade(1f, 0.25f).From(0f).SetDelay(delay);
        //item.DOScale(1f, 0.05f).SetDelay(delay + 0.25f);
    }
}

public enum ItemAnimType
{
    PopOut,
    Fade
}
