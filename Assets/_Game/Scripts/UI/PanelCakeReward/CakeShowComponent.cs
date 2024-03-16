using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeShowComponent : MonoBehaviour
{
    [SerializeField] Camera cakeCamera;
    [SerializeField] MeshFilter cakeMesh;
    [SerializeField] List<MeshFilter> cakeSlideMeshs;
    [SerializeField] DOTweenAnimation cakeAnim;
    [SerializeField] float normalCamZoom;
    [SerializeField] float unlockCamZoom;
    [SerializeField] Transform cakePlate;
    [SerializeField] Transform cakeShowPos;
    [SerializeField] Transform cakeStartPos;

    private void Start()
    {
        ShowNormalCake();
        ShowNextToUnlockCake();
    }

    public void ShowNormalCake()
    {
        cakeCamera.orthographicSize = normalCamZoom;
    }

    public void ShowNewUnlockCake()
    {
        Mesh cakeSlideMesh = GameManager.Instance.cakeManager.GetNewUnlockedCakePieceMesh();
        cakeCamera.orthographicSize = unlockCamZoom;

        cakePlate.DOScale(1, 0.35f).From(0).SetEase(Ease.OutBack);
        cakePlate.DOMove(cakeShowPos.position, 0.35f).From(cakeStartPos.position).SetEase(Ease.OutBack);

        for (int i = 0; i < cakeSlideMeshs.Count; i++)
        {
            cakeSlideMeshs[i].mesh = cakeSlideMesh;
            cakeSlideMeshs[i].transform.DOScale(3.5f, 0.25f).SetDelay((i + 1)*0.15f + 0.35f).From(0);
        }
    }

    public void ShowNextToUnlockCake()
    {
        Mesh cakeSlideMesh = GameManager.Instance.cakeManager.GetNextUnlockedCakeMesh();
        cakeMesh.mesh = cakeSlideMesh;
    }

    public void ShowSelectetCake(int cakeId)
    {
        Mesh cakeSlideMesh = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeMesh(cakeId);
        cakeMesh.mesh = cakeSlideMesh;
    }
}
