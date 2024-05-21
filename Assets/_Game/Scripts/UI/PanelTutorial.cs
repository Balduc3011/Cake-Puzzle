using AssetKits.ParticleImage.Enumerations;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTutorial : UIPanel
{
    [SerializeField] Transform handUI;
    [SerializeField] UIFloating targetUI;
    [SerializeField] UIFloating cakeUI;
    Camera cam;
    Sequence mainSequence;
    public override void Awake()
    {
        panelType = UIPanelType.PanelTutorial;
        base.Awake();
        mainSequence = DOTween.Sequence();
    }

    public void PlayTutorial(Transform plate, Transform cake)
    {
        targetUI.SetTarget(plate);
        cakeUI.SetTarget(cake);
        mainSequence.Kill();
        mainSequence = DOTween.Sequence();
        if (cam == null) cam = GameManager.Instance.cameraManager.GetMainCamera();
        Vector3 pos1 = cam.WorldToScreenPoint(cake.position);
        Vector3 pos2 = cam.WorldToScreenPoint(plate.position);

        mainSequence.Append(handUI.DOMove(pos2, 1f).From(pos1));
        mainSequence.SetLoops(-1);
        mainSequence.Play();
    }
}
