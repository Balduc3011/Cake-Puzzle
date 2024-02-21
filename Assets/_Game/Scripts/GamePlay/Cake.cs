using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cake : MonoBehaviour
{
    public List<Piece> pieces = new List<Piece>();
    public List<int> rotates = new List<int>();
    public int totalPieces;
    public int totalCakeID;
    public float radiusCheck;
    public Plate currentPlate;
    GroupCake myGroupCake;
    public List<int> pieceCakeIDCount = new List<int>();
    public List<int> pieceCakeID = new List<int>();
    public Piece piecePref;
    public bool cakeDone;

    [SerializeField] AnimationCurve curveRotate;
    [SerializeField] Vector3 vectorOffsetEffect;
    [SerializeField] Vector3 vectorOffsetExp;
    [SerializeField] float scaleDefault;

    [SerializeField] Transform spawnContainer;
    public Dictionary<int, GameObject> objectDecoration = new Dictionary<int, GameObject>();

    private void Start()
    {
        EventManager.AddListener(EventName.ChangePlateDecor.ToString(), UpdatePlateDecor);
    }

    public void InitData() {
        transform.DOScale(scaleDefault, .5f).From(1.2f).SetEase(Ease.InOutBack);
        totalPieces = GameManager.Instance.cakeManager.GetPiecesTotal() + 1;
        SetupPiecesCakeID();
        pieceIndex = 0;
        SetUpCakeID();
        for (int i = 0; i < pieceCakeIDCount.Count; i++)
        {
            InitPiecesSame(pieceCakeIDCount[i], pieceCakeID[i]);
        }
        UpdatePlateDecor();
    }

    public void InitData(CakeSave cakeSaveData) {
        transform.DOScale(scaleDefault, .5f).From(1.2f).SetEase(Ease.InOutBack);
        pieceCakeIDCount = cakeSaveData.pieceCakeIDCount;
        pieceCakeID = cakeSaveData.pieceCakeID;
        pieceIndex = 0;
        for (int i = 0; i < pieceCakeIDCount.Count; i++)
        {
            InitPiecesSame(pieceCakeIDCount[i], pieceCakeID[i]);
        }
        UpdatePlateDecor();
    }

    public void InitData(List<int> cakeIDs) {
        transform.DOScale(scaleDefault, .5f).From(1.2f).SetEase(Ease.InOutBack);
        
        pieceIndex = 0;
        for (int i = 0; i < cakeIDs.Count; i++)
        {
            if (!pieceCakeID.Contains(cakeIDs[i])) pieceCakeID.Add(cakeIDs[i]);
            Piece newPiece = Instantiate(piecePref, transform);
            pieces.Add(newPiece);
            InitPiece(i, cakeIDs[i]);
        }
        UpdatePlateDecor();
    }


    int indexRandom;
    void SetupPiecesCakeID() {
        pieceCakeIDCount.Clear();
        totalCakeID = GameManager.Instance.cakeManager.GetTotalCakeID() + 1;
        if (totalCakeID > totalPieces)
            totalCakeID = totalPieces;

        for (int i = 0; i < totalCakeID; i++)
        {
            pieceCakeIDCount.Add(1);
            pieceCakeID.Add(-1);
        }

        for (int i = 0; i < totalPieces - totalCakeID; i++)
        {
            indexRandom = Random.Range(0, pieceCakeIDCount.Count);
            pieceCakeIDCount[indexRandom]++;
        }
        pieceCakeIDCount.Sort((a, b) => Compare(a, b));
    }

    int Compare(int a, int b) {
        if (a < b) return 1;
        if (a > b) return -1;
        return 0;
    }

    List<int> cakeID = new List<int>();
    void SetUpCakeID() {
        cakeID = ProfileManager.Instance.playerData.cakeSaveData.cakeIDUsing;
        for (int i = 0; i < pieceCakeID.Count; i++)
        {
            int randomID = GetRandomCakeID();
            pieceCakeID[i] = randomID;
        }
    }

    int GetRandomCakeID() {
        int randomIndexX = Random.Range(0, cakeID.Count);
        while (pieceCakeID.Contains(cakeID[randomIndexX]))
        {
            randomIndexX = Random.Range(0, cakeID.Count);
        }
        return cakeID[randomIndexX];
    }

    int pieceIndex;
    void InitPiecesSame(int totalPiecesSame, int pieceCakeID) {
        int pieceCountSame = 0;
        while (pieceCountSame < totalPiecesSame) {
            Piece newPiece = Instantiate(piecePref, transform);
            pieces.Add(newPiece);
            InitPiece(pieceIndex, pieceCakeID);
            pieceIndex++;
            pieceCountSame++;
        }
    }

    void InitPiece(int pieceInidex, int pieceCakeID) {
        pieces[pieceInidex].transform.eulerAngles = Vector3.zero;
        GameObject objecPref = Resources.Load("Pieces/Piece_" + pieceCakeID) as GameObject;
        pieces[pieceInidex].InitData(objecPref, pieceCakeID);

        pieces[pieceInidex].transform.eulerAngles = new Vector3(0, rotates[pieceInidex], 0);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.IsPointerOverGameObject(0))
            return;
        if (myGroupCake != null)
        {
            myGroupCake.OnFollowMouse();
        }
        else Debug.Log("Group cake null!");
    }

    public bool CheckDrop()
    {
        if (currentPlate != null && currentPlate.currentCake == null)
        {
            currentPlate.SetCurrentCake(this);
            currentPlate.Deactive();
            return true;
        }
        return false;
    }

    bool onDrop;
    public void DropDone() {
        onDrop = true;
        transform.parent = currentPlate.pointStay;
        transform.DOLocalMove(Vector3.zero, .1f);
        //ProfileManager.Instance.playerData.cakeSaveData.SaveCake(currentPlate.GetPlateIndex(), this);
    }

    public void GroupDropFail() {
        if (currentPlate != null)
        {
            currentPlate.currentCake = null;
            currentPlate.Deactive();
            currentPlate = null;
        }
    }

    RaycastHit hitInfor;
    [SerializeField] LayerMask mask;
    [SerializeField] Vector3 vectorCheckOffset;
    public void CheckOnMouse() {
        if (onDrop) return;
        if (Physics.SphereCast(transform.position, radiusCheck, -transform.up * .1f+ vectorCheckOffset, out hitInfor))
        {
            if (hitInfor.collider.gameObject.layer == 6)
            {
                Plate plate = hitInfor.collider.gameObject.GetComponent<Plate>();
                if (currentPlate != null)
                    currentPlate.Deactive();
                if (plate.currentCake != null)
                {
                    currentPlate = null;
                    DeActiveCurrentPlate();
                    return;
                }
                currentPlate = plate;
                plate.Active();
            }
            else DeActiveCurrentPlate();
        }
        else DeActiveCurrentPlate();
    }

    void DeActiveCurrentPlate() {
        if (currentPlate != null)
        {
            currentPlate.Deactive();
            currentPlate = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusCheck);
    }

    public void SetGroupCake(GroupCake groupCake)
    {
        myGroupCake = groupCake;
    }
    Piece piece;
    public bool GetCakePieceSame(int cakeID) {
        piece = pieces.Find(e => (e.cakeID == cakeID && e.gameObject.activeSelf));
        return piece != null;
    }
    bool otherCake = false;
    bool sameCake = false;
    public int GetRotateIndex(int cakeID) {
        otherCake = false;
        sameCake = false;
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID != cakeID)
            {
                otherCake = true;
                if (otherCake && sameCake)
                {
                    indexRotate = 0;
                    if (!cakeDone)
                        StartCoroutine(RotateOtherPiece(i));
                    return i;
                }
            }

            if (pieces[i].cakeID == cakeID)
            {
                sameCake = true;
                if (otherCake && sameCake)
                {
                    if (!cakeDone)
                        StartCoroutine(RotateOtherPiece(i+1));
                    return i+1;
                }
            }

           
        }
        return pieces.Count;
    }
    Vector3 vectorRotateTo;
    int indexRotate = 0;
    IEnumerator RotateOtherPiece(int pieceIndex) {
        indexRotate = pieceIndex;
        while (indexRotate < pieces.Count)
        {
            vectorRotateTo = new Vector3(0, rotates[indexRotate], 0);
            pieces[indexRotate].transform.DORotate(vectorRotateTo, .15f).SetEase(Ease.InOutSine);
            indexRotate++;
            yield return new WaitForSeconds(.1f);
        }
    }

    public void RotateOtherPieceRight(int pieceIndex) {
        indexRotate = pieceIndex;
        if (cakeDone) return;
        StartCoroutine(RotateOtherPieceRightWay());
       
    }

    IEnumerator RotateOtherPieceRightWay() {
       while (indexRotate < pieces.Count) { 
            vectorRotateTo = new Vector3(0, rotates[indexRotate], 0);
            pieces[indexRotate].transform.DORotate(vectorRotateTo, .15f).SetEase(Ease.InOutSine);
            indexRotate++;
            yield return new WaitForSeconds(.1f);
        }
    }

    public Piece GetPieceMove(int cakeID)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID == cakeID)
            {
                piece = pieces[i];
                pieces.Remove(pieces[i]);
                return piece;
            }
        }
        return null;
    }

    public bool CheckMoveDone(int cakeID)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID == cakeID)
                return false;
        }

        return true;
    }

    public bool CheckBestCakeDone(int cakeID, int totalPieces)
    {
        int pieceCount = 0;
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID == cakeID)
                pieceCount++;
        }
        return pieceCount >= totalPieces;
    }

    public int GetCurrentPiecesSame(int cakeID)
    {
        int pieceCount = 0;
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID == cakeID)
                pieceCount++;
        }
        return pieceCount;
    }

    Piece pieceTemp;
    public void AddPieces(Piece piece, int indexChange)
    {
        pieces.Add(piece);
        pieceTemp = pieces[indexChange];
        pieces[indexChange] = piece;
        pieces[pieces.Count - 1] = pieceTemp;
    }

    public bool CheckCakeIsDone(int cakeID) {
        if (pieces.Count < 6) return false;
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID != cakeID)
               return false;
        }
        return true;
    }

    public bool CheckHaveCakeID(int cakeID)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID == cakeID)
                return true;
        }
        return false;
    }

    public void DoneCakeMode()
    {
        GameObject objecPref = Resources.Load("Pieces/Cake_" + pieces[0].cakeID) as GameObject;
        CakeFullAnimation trs = Instantiate(objecPref).GetComponent<CakeFullAnimation>();
        trs.transform.position = transform.position;
        trs.AnimDoneCake();
        Transform trsEffect = GameManager.Instance.objectPooling.GetCakeDoneEffect();
        trsEffect.transform.position = transform.position + vectorOffsetEffect;
        trsEffect.gameObject.SetActive(true);

        ExpEffect expEffect = GameManager.Instance.objectPooling.GetExpEffect();
        expEffect.transform.position = Camera.main.WorldToScreenPoint(transform.position) + vectorOffsetExp;
        expEffect.ChangeTextExp("10");
        expEffect.gameObject.SetActive(true);

        ProfileManager.Instance.playerData.playerResourseSave.AddExp(10);
        ProfileManager.Instance.playerData.playerResourseSave.AddMoney(10);
        transform.localScale = Vector3.zero;
        //Destroy(gameObject);
        DOVirtual.DelayedCall(.5f, () => { Destroy(gameObject); });
    }

    public void UpdatePlateDecor()
    {
        int allPlateCount = ProfileManager.Instance.dataConfig.decorationDataConfig.GetDecorationDataList(DecorationType.Plate).decorationDatas.Count;
        int currentId = ProfileManager.Instance.playerData.decorationSave.GetUsingDecor(DecorationType.Plate);
        for (int i = 0; i < allPlateCount; i++)
        {
            if (objectDecoration.ContainsKey(i))
            {
                if(i != currentId)
                {
                    objectDecoration[i].SetActive(false);
                }
                else
                {
                    objectDecoration[i].SetActive(true);
                }
            }
        }
        if (!objectDecoration.ContainsKey(currentId))
        {
            GameObject newDecor = Instantiate(Resources.Load("Decoration/Plate/" + currentId.ToString()) as GameObject, spawnContainer);
            objectDecoration.Add(currentId, newDecor);
        }   
    }

    public int GetPieceFree()
    {
        return 6 - pieces.Count;
    }
}
