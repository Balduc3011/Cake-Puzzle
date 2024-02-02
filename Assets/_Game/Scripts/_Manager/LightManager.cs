using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] Light lightDecore;
    public void UsingItemMode() {
        DOVirtual.Float(lightDecore.intensity, 0, .5f, (value) => {
            lightDecore.intensity = value;
        }).SetEase(Ease.InOutQuad);
    }

    public void OutItemMode()
    {
        DOVirtual.Float(lightDecore.intensity, 1.2f, .5f, (value) => {
            lightDecore.intensity = value;
        }).SetEase(Ease.InOutQuad);
    }
}
