using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeShowComponent : MonoBehaviour
{
    [SerializeField] Camera cakeCamera;
    [SerializeField] MeshFilter cakeMesh;
    [SerializeField] GameObject cakeSlides;
    [SerializeField] MeshFilter fullCakeMesh;
    [SerializeField] List<MeshFilter> cakeSlideMeshs;
    [SerializeField] DOTweenAnimation cakeAnim;
    [SerializeField] float normalCamZoom;
    [SerializeField] float unlockCamZoom;
    [SerializeField] Transform cakePlate;
    [SerializeField] Transform cakeShowPos;
    [SerializeField] Transform cakeStartPos;
    [SerializeField] ParticleSystem particle;
    List<Tween> cakeSlideTween = new List<Tween>();

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
        cakeSlides.SetActive(true);
        fullCakeMesh.gameObject.SetActive(false);
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
        cakeSlides.SetActive(true);
        fullCakeMesh.gameObject.SetActive(false);
        Mesh cakeSlideMesh = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakePieceMesh2(cakeId);
        cakeCamera.orthographicSize = unlockCamZoom;

        for (int i = 0; i < cakeSlideTween.Count; i++)
        {
            if (cakeSlideTween[i] != null)
                cakeSlideTween[i].Kill();
        }

        for (int i = 0; i < cakeSlideMeshs.Count; i++)
        {
            cakeSlideMeshs[i].mesh = cakeSlideMesh;
            if(cakeSlideMeshs.Count > cakeSlideTween.Count)
            {
                Tween tween = cakeSlideMeshs[i].transform.DOScale(3.5f, 0.25f).SetDelay((i + 1) * 0.15f + 0.35f).From(0);
                cakeSlideTween.Add(tween);
            }
            else
            {
                if (cakeSlideTween[i] != null)
                    cakeSlideTween[i].Kill();
                cakeSlideTween[i] = cakeSlideMeshs[i].transform.DOScale(3.5f, 0.25f).SetDelay((i + 1) * 0.15f + 0.35f).From(0);
            }
            
        }
    }

    public void ShowFullSelectetCake(int cakeId)
    {
        cakeSlides.SetActive(false);
        fullCakeMesh.gameObject.SetActive(true);
        Mesh cakeSlideMesh = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakeFullMesh2(cakeId);
        cakeCamera.orthographicSize = unlockCamZoom;
        
        fullCakeMesh.mesh = cakeSlideMesh;
        fullCakeMesh.transform.DOScale(1.75f, 0.25f).SetDelay(0.15f + 0.35f).From(0);
    }

    public int testCakeId;
    public int level;
    [Button]
    public void TestCake()
    {
        Mesh cakeSlideMesh = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakePieceMesh(testCakeId, level);
        for (int i = 0; i < cakeSlideMeshs.Count; i++)
        {
            cakeSlideMeshs[i].mesh = cakeSlideMesh;
        }
    }
}
