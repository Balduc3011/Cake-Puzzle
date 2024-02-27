using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.XR;

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
    int indexFirstSpawn;

    AnimationCurve curveRotate;
    float timeRotate;
    [SerializeField] Vector3 vectorOffsetEffect;
    [SerializeField] Vector3 vectorOffsetExp;
    [SerializeField] float scaleDefault;

    [SerializeField] Transform spawnContainer;
    public Dictionary<int, GameObject> objectDecoration = new Dictionary<int, GameObject>();

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

    void SetFirstIndexOfPiece() {
        indexFirstSpawn = Random.Range(0, 6);
        currentRotateIndex = indexFirstSpawn - 1;
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
        currentRotateIndex++;
        if (currentRotateIndex >= rotates.Count)
            currentRotateIndex = 0;
        pieces[pieceInidex].InitData(objecPref, pieceCakeID, currentRotateIndex);

        pieces[pieceInidex].transform.eulerAngles = new Vector3(0, GetRotate(), 0);
    }

    int currentRotateIndex = 0;
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
    List<int> listCakeIDFillUp = new List<int>();
    int pieceCakeIDFill = 0;
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

    bool onDrop;
    public void DropDone(bool lastDrop, UnityAction actionCallback) {
        onDrop = true;
        transform.parent = currentPlate.pointStay;
        transform.DOLocalMove(Vector3.zero, .1f);
        transform.DOScale(Vector3.one * .95f, .25f).OnComplete(()=> {
            if (lastDrop)
                actionCallback();
            transform.DOScale(Vector3.one * .75f, .2f);
            transform.DOScale(Vector3.one * .8f, .2f).SetDelay(.2f);
        });
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

    public GroupCake GetGroupCake() { return myGroupCake; }
    Piece piece;
    public bool GetCakePieceSame(int cakeID) {
        piece = pieces.Find(e => (e.cakeID == cakeID && e.gameObject.activeSelf));
        return piece != null;
    }
    bool otherCake = false;
    bool sameCake = false;
    public int indexOfNewPiece;
    public int GetRotateIndex(int cakeID) {
        otherCake = false;
        sameCake = false;
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID != cakeID)
                otherCake = true;

            if (pieces[i].cakeID == cakeID)
                sameCake = true;

            if (otherCake && sameCake)
            {
                if (!cakeDone)
                    StartCoroutine(RotateOtherPiece(i));
                Debug.Log("return rotate index: " + pieces[i].currentRotateIndex);
                indexOfNewPiece = i;
                return pieces[i].currentRotateIndex - 1;
            }

        }
        //StartCoroutine(RotateOtherPiece(pieces.Count - 1));
        indexOfNewPiece = pieces.Count;
        Debug.Log("return rotate index: " + (pieces[pieces.Count - 1].currentRotateIndex+1));
        return pieces[pieces.Count - 1].currentRotateIndex+1;
    }
    Vector3 vectorRotateTo;
    int indexRotate = 0;
    IEnumerator RotateOtherPiece(int pieceIndex) {
        Debug.Log("Call rotae other pieces");
        indexRotate = pieceIndex;
        while (indexRotate < pieces.Count)
        {
            pieces[indexRotate].currentRotateIndex++;
            if (pieces[indexRotate].currentRotateIndex >= rotates.Count) pieces[indexRotate].currentRotateIndex = 0;
            vectorRotateTo = new Vector3(0, rotates[pieces[indexRotate].currentRotateIndex], 0);
            pieces[indexRotate].transform.DORotate(vectorRotateTo, timeRotate).SetEase(curveRotate);
            indexRotate++;
            yield return new WaitForSeconds(.25f);
        }
    }

    public void RotateOtherPieceRight(int pieceIndex) {
        indexRotate = pieceIndex;
        Debug.Log("Call rotae right way");
        if (cakeDone) return;
        currentRotateIndex = indexFirstSpawn - 1;
        StartCoroutine(RotateOtherPieceRightWay());
       
    }

    IEnumerator RotateOtherPieceRightWay() {
        
        while (indexRotate < pieces.Count) {
            currentRotateIndex++;
            if (currentRotateIndex >= rotates.Count)
                currentRotateIndex = 0;
            pieces[indexRotate].currentRotateIndex = currentRotateIndex;
            vectorRotateTo = new Vector3(0, rotates[currentRotateIndex], 0);
            pieces[indexRotate].transform.DORotate(vectorRotateTo, timeRotate).SetEase(curveRotate);
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
    public void AddPieces(Piece piece)
    {
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
    PanelTotal panelTotal;
    Vector3 vectorDefault = new Vector3(.8f, .8f, .8f);
    Vector3 vectorScaleUp = new Vector3(1f, 1f, 1f);
    Vector3 vectorScaleDown = new Vector3(.7f, .7f, .7f);
    Vector3 vectorRotate = new Vector3(0, 360, 0);
    public void DoneCakeMode()
    {
        //GameObject objecPref = Resources.Load("Pieces/Cake_" + pieces[0].cakeID) as GameObject;
        //CakeFullAnimation trs = Instantiate(objecPref).GetComponent<CakeFullAnimation>();
        //trs.transform.position = transform.position;
        //trs.AnimDoneCake();

        if (panelTotal == null)
            panelTotal = UIManager.instance.panelTotal;
        DOVirtual.DelayedCall(.35f, () => {
            transform.DOScale(vectorScaleDown, .3f);
            transform.DOScale(vectorScaleUp, .3f).SetDelay(0.3f);
            transform.DOScale(vectorDefault, .3f).SetDelay(.6f);
            transform.DORotate(vectorRotate, 1f, RotateMode.WorldAxisAdd).SetDelay(.4f).OnComplete(() => {
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
        //expEffect.ChangeTextExp("10");
        expEffect.ChangeTextExp(((pieces[0].cakeID + 1) * ConstantValue.VAL_DEFAULT_EXP).ToString());
        expEffect.gameObject.SetActive(true);

        ProfileManager.Instance.playerData.playerResourseSave.AddExp((pieces[0].cakeID + 1) * ConstantValue.VAL_DEFAULT_EXP);
        ProfileManager.Instance.playerData.playerResourseSave.AddMoney((pieces[0].cakeID + 1) * ConstantValue.VAL_DEFAULT_EXP);
        ProfileManager.Instance.playerData.playerResourseSave.AddTrophy((pieces[0].cakeID + 1) * ConstantValue.VAL_DEFAULT_TROPHY);
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

    private List<Tween> tweens = new();
    public void DoAnimImpact()
    {

        tweens.ForEach(t => t?.Kill());
        tweens.Clear();
        tweens.Add(transform.DOScale(Vector3.one * 0.67f, 0.13f).SetEase(Ease.InSine));
        tweens.Add(transform.DOScale(Vector3.one * 0.71f, 0.13f).SetEase(Ease.InOutSine).SetDelay(0.13f));
        tweens.Add(transform.DOScale(Vector3.one * 0.7f, 0.13f).SetEase(Ease.OutSine).SetDelay(0.26f));
    }

    public int GetPieceFree()
    {
        return 6 - pieces.Count;
    }

    List<int> listPieceIDReturn = new();
    public List<int> GetCurrenPieceIDs()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (!listPieceIDReturn.Contains(pieces[i].cakeID))
                listPieceIDReturn.Add(pieces[i].cakeID);
        }
        return listPieceIDReturn;
    }
    List<IDInfor> currentIDInfor = new();
    IDInfor idInfor;
    public List<IDInfor> GetIDInfor()
    {
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
}
