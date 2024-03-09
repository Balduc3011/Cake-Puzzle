using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CacheEngine;

public class Cake : MonoBehaviour
{
    [Header("LIST")]
    public List<Piece> pieces = new List<Piece>();
    public List<int> rotates = new List<int>();
    public List<int> pieceCakeIDCount = new List<int>();
    public List<int> pieceCakeID = new List<int>();
    List<int> cakeID = new List<int>();
    List<int> listCakeIDFillUp = new List<int>();
    List<Tween> tweens = new();
    List<IDInfor> currentIDInfor = new();

    [Header("INT")]
    public int totalPieces;
    public int totalCakeID;
    public int indexOfNewPiece;

    int pieceIndex;
    int indexFirstSpawn;
    int indexRandom;
    int currentRotateIndex = 0;
    int pieceCakeIDFill = 0;
    int indexRotate = 0;
    int indexReturn;
    int indexRotateRW = 0;

    [Header("FLOAT")]
    public float radiusCheck;
    [SerializeField] float scaleDefault;
    float timeRotate;

    [Header("BOOL")]
    public bool cakeDone;
    public bool needRotateRightWay = false;

    bool onDrop;
    bool callBackRotateDone = false;
    bool otherCake = false;
    bool sameCake = false;
    bool flagDoActionCallBack = false;

    [Header("OTHER COMPONENT")]
    public Plate currentPlate;
    public Piece piecePref;

    [SerializeField] Transform spawnContainer;
    [SerializeField] LayerMask mask;
    RaycastHit hitInfor;

    GroupCake myGroupCake;
    Piece piece;
    Piece pieceTemp;
    PanelTotal panelTotal;
    IDInfor idInfor;

    AnimationCurve curveRotate;

    public Dictionary<int, GameObject> objectDecoration = new Dictionary<int, GameObject>();
    UnityAction actionCallBackRotateDone;
    UnityAction rotateRWDone;

    [Header("VECTOR")]
    [SerializeField] Vector3 vectorOffsetEffect;
    [SerializeField] Vector3 vectorOffsetExp;
    [SerializeField] Vector3 vectorCheckOffset;

    Vector3 vectorRotateTo;

    private void Start()
    {
        EventManager.AddListener(EventName.ChangePlateDecor.ToString(), UpdatePlateDecor);
        EventManager.AddListener(EventName.UsingFillUp.ToString(), UsingFillUpMode);
        EventManager.AddListener(EventName.UsingFillUpDone.ToString(), UsingFillUpDone);
        curveRotate = ProfileManager.Instance.dataConfig.cakeAnimationSetting.GetCurveRightWay();
        timeRotate = ProfileManager.Instance.dataConfig.cakeAnimationSetting.GetTimeRightWay();
    }

    bool onUsingFillUp;
    void UsingFillUpMode()
    {
        onUsingFillUp = true;
    }

    void UsingFillUpDone() {
        onUsingFillUp = false;
    }

    #region INIT DATA
    public void InitData() {
        SetFirstIndexOfPiece();
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
        SetFirstIndexOfPiece();
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

    public void InitData(List<int> cakeIDs, Plate plate) {
        SetFirstIndexOfPiece();
        currentPlate = plate;
        InitData(cakeIDs);
        GameManager.Instance.cakeManager.AddCakeNeedCheck(this);
    }

    public void InitData(List<int> cakeIDs) {
        SetFirstIndexOfPiece();
        transform.DOScale(scaleDefault, .5f).From(1.2f).SetEase(Ease.InOutBack);

        for (int i = 0; i < cakeIDs.Count; i++)
        {
            if (!pieceCakeID.Contains(cakeIDs[i])) pieceCakeID.Add(cakeIDs[i]);
            Piece newPiece = Instantiate(piecePref, transform);
            pieces.Add(newPiece);
            InitPiece(i, cakeIDs[i]);
        }
        UpdatePlateDecor();
    }

    public void InitData(List<IDInfor> idInfors)
    {
        SetFirstIndexOfPiece();
        transform.DOScale(scaleDefault, .5f).From(1.2f).SetEase(Ease.InOutBack);

        for (int i = 0; i < idInfors.Count; i++)
        {
            pieceCakeID.Add(idInfors[i].ID);
            pieceCakeIDCount.Add(idInfors[i].count);
            for (int j = 0; j < idInfors[i].count; j++)
            {
                Piece newPiece = Instantiate(piecePref, transform);
                pieces.Add(newPiece);
                InitPiece(pieces.Count - 1, idInfors[i].ID);
            }
        }

        UpdatePlateDecor();
    }
    #endregion

    void SetFirstIndexOfPiece() {
        indexFirstSpawn = Random.Range(0, 6);
        currentRotateIndex = indexFirstSpawn - 1;
    }

    
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
        currentRotateIndex++;
        if (currentRotateIndex >= rotates.Count)
            currentRotateIndex = 0;
        pieces[pieceInidex].InitData(objecPref, pieceCakeID, currentRotateIndex);

        pieces[pieceInidex].transform.eulerAngles = new Vector3(0, GetRotate(), 0);
    }

    
    float GetRotate() {
      
        return rotates[currentRotateIndex];
      
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.IsPointerOverGameObject(0))
            return;

        if (onUsingFillUp)
        {
            Debug.Log("Choose on Fill up");
            FillUp();
            return;
        }

        if (myGroupCake != null)
        {
            myGroupCake.OnFollowMouse();
        }
        else Debug.Log("Group cake null!");
    }
    
    void FillUp() {
        Debug.Log("Fill up");
        listCakeIDFillUp.Clear();
        pieceCakeIDFill = pieces[0].cakeID;
        for (int i = pieces.Count - 1; i >= 0; i--)
        {
            pieces[i].RemoveByFillUp();
            pieces.Remove(pieces[i]);
        }
        for (int i = 0; i < 6; i++)
        {
            listCakeIDFillUp.Add(pieceCakeIDFill);
        }
        InitData(listCakeIDFillUp, currentPlate);
        GameManager.Instance.itemManager.UsingItemDone();
        ProfileManager.Instance.playerData.playerResourseSave.UsingItem(ItemType.FillUp);
        DoneCakeMode();
        ProfileManager.Instance.playerData.cakeSaveData.RemoveCake(currentPlate.plateIndex);
        EventManager.TriggerEvent(EventName.UsingFillUpDone.ToString());
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

    
    public void DropDone(bool lastDrop, UnityAction actionCallback) {
        onDrop = true;
        transform.parent = currentPlate.pointStay;
        transform.DOLocalMove(Vector3.zero, .1f);
        transform.DOScale(Vector3.one * .9f, .25f).OnComplete(()=> {
            if (lastDrop)
                actionCallback();
            transform.DOScale(Vector3.one * 1.1f, .2f);
            transform.DOScale(Vector3.one, .2f).SetDelay(.2f);
        });
    }

    public void GroupDropFail() {
        if (currentPlate != null)
        {
            currentPlate.currentCake = null;
            currentPlate.Deactive();
            currentPlate = null;
        }
    }

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

    
    public bool GetCakePieceSame(int cakeID) {
        piece = pieces.Find(e => (e.cakeID == cakeID && e.gameObject.activeSelf));
        return piece != null;
    }
  
    public void MakeRotateIndexForNewPiece(int cakeID) {
        otherCake = false;
        sameCake = false;
        indexReturn = -1;
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID != cakeID)
                otherCake = true;

            if (pieces[i].cakeID == cakeID)
                sameCake = true;

            if (otherCake && sameCake)
            {
                
                indexOfNewPiece = i;
                indexReturn = pieces[i].currentRotateIndex;
                break;
            }

        }
        if (indexReturn == -1)
        {
            indexOfNewPiece = pieces.Count;
            indexReturn = pieces[pieces.Count - 1].currentRotateIndex + 1;
        }
    }

    public void StartRotateOtherPieceForNewPiece(UnityAction actionCallBack) {
        actionCallBackRotateDone = actionCallBack;
        if (!cakeDone)
        {
            flagDoActionCallBack = false;
            StartCoroutine(RotateOtherPiece(indexOfNewPiece + 1));
        }
    }

    public int GetRotateIndex() { return indexReturn; }
 
    IEnumerator RotateOtherPiece(int pieceIndex) {
        indexRotate = pieceIndex;
        while (indexRotate < pieces.Count)
        {
            pieces[indexRotate].currentRotateIndex++;
            if (pieces[indexRotate].currentRotateIndex >= rotates.Count) pieces[indexRotate].currentRotateIndex = 0;
            vectorRotateTo = new Vector3(0, rotates[pieces[indexRotate].currentRotateIndex], 0);
            Debug.Log(pieces[indexRotate]);
            pieces[indexRotate].transform.DORotate(vectorRotateTo, timeRotate).SetEase(curveRotate).OnComplete(() => {
                if (indexRotate == pieces.Count - 1)
                {
                    flagDoActionCallBack = true;
                    actionCallBackRotateDone();
                }
            });
            yield return new WaitForSeconds(timeRotate - 0.15f);
            indexRotate++;
        }
        if (!flagDoActionCallBack) actionCallBackRotateDone();

    }
  
    public void RotateOtherPieceRight(UnityAction actionCallRotateDone) {
        rotateRWDone = actionCallRotateDone;
        indexRotateRW = 0;
        callBackRotateDone = false;
        if (cakeDone)
        {
            rotateRWDone();
            return;
        }
        currentRotateIndex = indexFirstSpawn;
        needRotateRightWay = false;
        StartCoroutine(RotateOtherPieceRightWay());
       
    }

    IEnumerator RotateOtherPieceRightWay() {
        while (indexRotateRW < pieces.Count) {
            if (currentRotateIndex >= rotates.Count)
                currentRotateIndex = 0;
            if (pieces[indexRotateRW].currentRotateIndex != currentRotateIndex)
                pieces[indexRotateRW].currentRotateIndex = currentRotateIndex;
            vectorRotateTo = new Vector3(0, rotates[pieces[indexRotateRW].currentRotateIndex], 0);
            pieces[indexRotateRW].transform.DORotate(vectorRotateTo, timeRotate).SetEase(curveRotate);
            DOVirtual.DelayedCall(timeRotate - .15f, () =>
            {
                if (indexRotateRW == pieces.Count)
                {
                    callBackRotateDone = true;
                    rotateRWDone();
                }
            });
            indexRotateRW++;
            currentRotateIndex++;
            yield return new WaitForSeconds(timeRotate-.15f);
        }

        if (!callBackRotateDone)
            rotateRWDone();
    }

    public Piece GetPieceMove(int cakeID)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID == cakeID)
            {
                piece = pieces[i];
                pieces.Remove(pieces[i]);
                needRotateRightWay = true;
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
   
    public void AddPieces(Piece piece)
    {
        if (pieces.Contains(piece)) return;
        pieces.Add(piece);
        pieceTemp = pieces[indexOfNewPiece];
        pieces[indexOfNewPiece] = piece;
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
        GameManager.Instance.questManager.AddProgress(QuestType.CompleteCake, 1);
        if (panelTotal == null)
            panelTotal = UIManager.instance.panelTotal;

        GameManager.Instance.cakeManager.AddStreak(this);
        ProfileManager.Instance.playerData.playerResourseSave.AddExp((pieces[0].cakeID + 1) * ConstantValue.VAL_DEFAULT_EXP);
        ProfileManager.Instance.playerData.playerResourseSave.AddMoney((pieces[0].cakeID + 1) * GameManager.Instance.GetDefaultCakeProfit());
        ProfileManager.Instance.playerData.playerResourseSave.AddTrophy((pieces[0].cakeID + 1) * ConstantValue.VAL_DEFAULT_TROPHY);
        DOVirtual.DelayedCall(CacheSourse.float035, () => {
            transform.DOScale(CacheSourse.vector09, CacheSourse.float03);
            transform.DOScale(CacheSourse.vector12, CacheSourse.float03).SetDelay(CacheSourse.float04);
            transform.DOScale(CacheSourse.vector1, CacheSourse.float03).SetDelay(CacheSourse.float06);
            transform.DORotate(CacheSourse.rotateY360, 1f, RotateMode.WorldAxisAdd).SetDelay(CacheSourse.float04).OnComplete(() => {
                EffectDoneCake();
            });
        });
       
    }

    void EffectDoneCake() {
        CoinEffect coinEffect = GameManager.Instance.objectPooling.GetCoinEffect();
        coinEffect.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        coinEffect.Move(panelTotal.GetCoinTrs());

        EffectMove effectMove = GameManager.Instance.objectPooling.GetEffectMove();
        effectMove.gameObject.SetActive(true);
        effectMove.PrepareToMove(Camera.main.WorldToScreenPoint(transform.position), panelTotal.GetPointSlider(), () => {
            EffectAdd trsExpEffect = GameManager.Instance.objectPooling.GetEffectExp();
            trsExpEffect.SetActionCallBack(() => {
                EventManager.TriggerEvent(EventName.ChangeExp.ToString());
            });
            trsExpEffect.transform.position = panelTotal.GetPointSlider().position;
        });


        Transform trsEffect = GameManager.Instance.objectPooling.GetCakeDoneEffect();
        trsEffect.transform.position = transform.position + vectorOffsetEffect;
        trsEffect.gameObject.SetActive(true);

        ExpEffect expEffect = GameManager.Instance.objectPooling.GetExpEffect();
        expEffect.transform.position = Camera.main.WorldToScreenPoint(transform.position) + vectorOffsetExp;
        expEffect.ChangeText(((pieces[0].cakeID + 1) * ConstantValue.VAL_DEFAULT_EXP).ToString());
        expEffect.gameObject.SetActive(true);

       
        transform.localScale = CacheSourse.vector0;

        DOVirtual.DelayedCall(CacheSourse.float05, () => {
            Debug.Log("Destroy now");
            Destroy(gameObject);
        });
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
  
    public void DoAnimImpact()
    {
        if (cakeDone)
            return;
        tweens.ForEach(t => t?.Kill());
        tweens.Clear();
        tweens.Add(transform.DOScale(CacheSourse.vector09, CacheSourse.float013).SetEase(Ease.InSine));
        tweens.Add(transform.DOScale(CacheSourse.vector12, CacheSourse.float013).SetEase(Ease.InOutSine).SetDelay(CacheSourse.float013));
        tweens.Add(transform.DOScale(CacheSourse.vector1, CacheSourse.float013).SetEase(Ease.OutSine).SetDelay(CacheSourse.float026));
    }

    public int GetPieceFree()
    {
        return 6 - pieces.Count;
    }
   
    public List<IDInfor> GetIDInfor()
    {
        currentIDInfor.Clear();
        for (int i = 0; i < pieces.Count; i++)
        {
            idInfor = currentIDInfor.Find(e => e.ID == pieces[i].cakeID);
            if (idInfor != null)
            {
                idInfor.count++;
            }
            else {
                IDInfor newIDInfor = new();
                newIDInfor.ID = pieces[i].cakeID;
                newIDInfor.count = 1;
                currentIDInfor.Add(newIDInfor);
            }
        }

        return currentIDInfor;
    }

    public bool CakeIsNull() { return pieces.Count == 0; }
}
