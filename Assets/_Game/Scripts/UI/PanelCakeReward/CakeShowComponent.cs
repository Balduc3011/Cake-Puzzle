using DG.Tweening;
using Sirenix.OdinInspector;
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
    [SerializeField] ParticleSystem particle;

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
        particle.gameObject.SetActive(true);
        particle.Play();
        cakePlate.DOScale(1, 0.35f).From(0).SetEase(Ease.OutBack);
        cakePlate.DOMove(cakeShowPos.position, 0.35f).From(cakeStartPos.position).SetEase(Ease.OutBack);

        for (int i = 0; i < cakeSlideMeshs.Count; i++)
        {
            cakeSlideMeshs[i].mesh = cakeSlideMesh;
            cakeSlideMeshs[i].transform.DOScale(3.5f, 0.25f).SetDelay((i)*0.1f + 0.2f).From(0).SetEase(Ease.OutBack);
        }
    }

    public void ShowNextToUnlockCake()
    {
        
    }

    public void ShowSelectetCake(int cakeId)
    {
        Mesh cakeSlideMesh = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakePieceMesh2(cakeId);
        cakeCamera.orthographicSize = unlockCamZoom;

        //cakePlate.DOScale(1, 0.35f).From(0).SetEase(Ease.OutBack);
        //cakePlate.DOMove(cakeShowPos.position, 0.35f).From(cakeStartPos.position).SetEase(Ease.OutBack);

        for (int i = 0; i < cakeSlideMeshs.Count; i++)
        {
            cakeSlideMeshs[i].mesh = cakeSlideMesh;
            cakeSlideMeshs[i].transform.DOScale(3.5f, 0.25f).SetDelay((i + 1) * 0.15f + 0.35f).From(0);
        }
    }

    public int testCakeId;
    [Button]
    public void TestCake()
    {
        Mesh cakeSlideMesh = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakePieceMesh(testCakeId);
        for (int i = 0; i < cakeSlideMeshs.Count; i++)
        {
            cakeSlideMeshs[i].mesh = cakeSlideMesh;
        }
    }
}
