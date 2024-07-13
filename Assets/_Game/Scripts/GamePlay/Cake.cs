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
    List<Tween> tweenAnimations = new();
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
    bool onChooseRevive = false;
    bool centerRevive = false;
    bool doneDrop = false;

    [Header("OTHER COMPONENT")]
    public Plate currentPlate;
    public Piece piecePref;

    [SerializeField] Transform spawnContainer;
    [SerializeField] LayerMask mask;
    [SerializeField] Collider myCollider;
    RaycastHit hitInfor;

    GroupCake myGroupCake;
    Piece piece;
    PanelTotal panelTotal;
    IDInfor idInfor;

    AnimationCurve curveRotate;

    public Dictionary<int, GameObject> objectDecoration = new Dictionary<int, GameObject>();
    UnityAction actionCallBackRotateDone;
    UnityAction rotateRWDone;

    [Header("VECTOR")]
    [SerializeField] Vector3 vectorOffsetEffect;
    [SerializeField] Vector3 vectorOffsetEffectDrop;
    [SerializeField] Vector3 vectorOffsetExp;
    [SerializeField] Vector3 vectorCheckOffset;

    Vector3 vectorRotateTo;
    Transform fillUpEffect;

    private void Start()
    {
        EventManager.AddListener(EventName.ChangePlateDecor.ToString(), UpdatePlateDecor);
        EventManager.AddListener(EventName.UsingFillUp.ToString(), UsingFillUpMode);
        EventManager.AddListener(EventName.UsingFillUpDone.ToString(), UsingFillUpDone);
        EventManager.AddListener(EventName.UsingHammer.ToString(), UsingHammerMode);
        EventManager.AddListener(EventName.UsingHammerDone.ToString(), UsingHammerModeDone);
        EventManager.AddListener(EventName.OnUsingRevive.ToString(), OnUsingRevive);
        EventManager.AddListener(EventName.OnUsingReviveDone.ToString(), OnUsingReviveDone);
        curveRotate = ProfileManager.Instance.dataConfig.cakeAnimationSetting.GetCurveRightWay();
        timeRotate = ProfileManager.Instance.dataConfig.cakeAnimationSetting.GetTimeRightWay();
    }

    bool onUsingFillUp;
    void UsingFillUpMode()
    {
        if (doneDrop)
            SetActiveCollider(true);
        onUsingFillUp = true;
    }

    void UsingFillUpDone() {
        if (doneDrop)
            SetActiveCollider(false);
        onUsingFillUp = false;
    }

    public bool CheckPlateOtherCake(PlateIndex plateIndex, int cakePosition)
    {
        switch (cakePosition)
        {
            case -1:
                if (plateIndex.indexX + 1 != currentPlate.plateIndex.indexX)
                    return false;
                return true;
            case 1:
                if (plateIndex.indexY - 1 != currentPlate.plateIndex.indexY)
                    return false;
                return true;
            default:
                return true;
        }
    }

    bool onUsingHammger;
    void UsingHammerMode()
    {
        if (doneDrop)
            SetActiveCollider(true);
        onUsingHammger = true;
    }

    void UsingHammerModeDone() {
        if (doneDrop)
            SetActiveCollider(false);
        onUsingHammger = false;
    }

    void OnUsingRevive() {
        if (doneDrop)
            SetActiveCollider(true);
        onChooseRevive = true;
    }

    void OnUsingReviveDone() {
        if (doneDrop)
            SetActiveCollider(false);
        onChooseRevive = false;
    }

    #region INIT DATA
    public void InitData() {
        //Debug.Log("init by normal");
        SetFirstIndexOfPiece();
        tweenAnimations.Add(transform.DOScale(scaleDefault, .5f).From(1.2f).SetEase(Ease.InOutBack));
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

    public void InitData(Plate plate)
    {
        SetFirstIndexOfPiece();
        tweenAnimations.Add(transform.DOScale(scaleDefault, .5f).From(1.2f).SetEase(Ease.InOutBack));
        totalPieces = GameManager.Instance.cakeManager.GetPiecesTotal() + 1;
        SetupPiecesCakeID();
        pieceIndex = 0;
        SetUpCakeID();
        for (int i = 0; i < pieceCakeIDCount.Count; i++)
        {
            InitPiecesSame(pieceCakeIDCount[i], pieceCakeID[i]);
        }
        currentPlate = plate;
        UpdatePlateDecor();
    }
    public void InitData(CakeSave cakeSaveData) {
        //Debug.Log("init by data save");
        
        SetFirstIndexOfPiece();
        tweenAnimations.Add(transform.DOScale(scaleDefault, .5f).From(1.2f).SetEase(Ease.InOutBack));
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
        //Debug.Log("init by data save on plate");
        doneDrop = true;
        SetFirstIndexOfPiece();
        currentPlate = plate;
        InitData(cakeIDs);
        GameManager.Instance.cakeManager.AddCakeNeedCheck(this);
    }

    float scaleFillUp;

    public void InitPieces() {
        currentRotateIndex++;
        if (currentRotateIndex >= 6)
            currentRotateIndex = 0;
        Piece newPiece = Instantiate(piecePref, transform);
        pieces.Add(newPiece);
        //InitPiece(pieceIndex, pieceCakeIDFill);
        tweenAnimations.Add(newPiece.transform.DOScale(1, .25f).From(0).SetEase(Ease.OutBack));
        newPiece.transform.eulerAngles = Vector3.zero;
        GameObject objecPref = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakePref(pieceCakeIDFill);
        newPiece.InitData(objecPref, pieceCakeIDFill, currentRotateIndex);

        newPiece.transform.eulerAngles = new Vector3(0, rotates[currentRotateIndex], 0);

        tweenAnimations.Add(newPiece.transform.DOLocalMove(Vector3.zero, .3f).SetEase(Ease.InOutCirc).From(newPiece.transform.forward * 5f).OnComplete(() => {
            ImpactOnFillUP();
        }));
    }

    void ImpactOnFillUP() {
        tweens.ForEach(t => t?.Kill());
        tweens.Clear();
        tweens.Add(transform.DOScale(scaleFillUp - .1f, CacheSourse.float013).SetEase(Ease.InSine));
        tweens.Add(transform.DOScale(scaleFillUp + .1f, CacheSourse.float013).SetEase(Ease.InOutSine).SetDelay(CacheSourse.float013));
        tweens.Add(transform.DOScale(scaleFillUp, CacheSourse.float013).SetEase(Ease.OutSine).SetDelay(CacheSourse.float026));
    }

    public void InitData(List<int> cakeIDs) {
        SetFirstIndexOfPiece();
        tweenAnimations.Add(transform.DOScale(scaleDefault, .5f).From(1.2f).SetEase(Ease.InOutBack));

        for (int i = 0; i < cakeIDs.Count; i++)
        {
            if (!pieceCakeID.Contains(cakeIDs[i]))
            {
                pieceCakeID.Add(cakeIDs[i]);
                pieceCakeIDCount.Add(1);
            }
            else {
                pieceCakeIDCount[pieceCakeID.IndexOf(cakeIDs[i])]++;
            }
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
        //GameObject objecPref = Resources.Load("Pieces/Piece_" + pieceCakeID) as GameObject;
        GameObject objecPref = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakePref(pieceCakeID);
        currentRotateIndex++;
        if (currentRotateIndex >= rotates.Count)
            currentRotateIndex = 0;
        pieces[pieceInidex].InitData(objecPref, pieceCakeID, currentRotateIndex);

        pieces[pieceInidex].transform.eulerAngles = new Vector3(0, GetRotate(), 0);
    }

    public void ReInitData() {
        for (int i = 0; i < pieces.Count; i++)
        {
            GameObject objecPref = ProfileManager.Instance.dataConfig.cakeDataConfig.GetCakePref(pieces[i].cakeID);
            pieces[i].ReInitData(objecPref);
        }
    }

    
    float GetRotate() {
      
        return rotates[currentRotateIndex];
      
    }

    bool CheckPickable()
    {
        if(ProfileManager.Instance.playerData.playerResourseSave.currentLevel == 0)
        {
            return myGroupCake.groupCakeIndex ==
                GameManager.Instance.tutorialManager.tutCakesId[GameManager.Instance.tutorialManager.currentTutIndex - 1];
        }
        return true;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.IsPointerOverGameObject(0) || UIManager.instance.isHasPopupOnScene)
            return;
        if (Application.platform == RuntimePlatform.Android)
        {
            if (EventSystem.current.IsPointerOverGameObject(0))
                return;
        }
        if (!CheckPickable()) return;
        //Debug.Log("On click");
        if (onChooseRevive) {
            centerRevive = true;
            GameManager.Instance.itemManager.OnUsingItem();
            UsingRevive();
            EventManager.TriggerEvent(EventName.OnUsingReviveDone.ToString());
            return;
        }

        if (onUsingFillUp)
        {
            //Debug.Log("Choose on Fill up");
            FillUp();
            return;
        }

        if (onUsingHammger)
        {
            //Debug.Log("Choose on using hammer");
            UsingHammer();
            return;
        }

        if (myGroupCake != null)
        {
            myGroupCake.OnFollowMouse();
        }
        //else Debug.Log("Group cake null!");
    }
    int indexRemove;
    void FillUp() {
        if (cakeDone)
            return;
        if (myGroupCake != null)
        {
            if (GameManager.Instance.cakeManager.CakeOnWait(myGroupCake))
                return;
        }
        GameManager.Instance.itemManager.OnUsingItem();
        EventManager.TriggerEvent(EventName.UsingFillUpDone.ToString());
        tweenAnimations.Add(transform.DOMove(GameManager.Instance.itemManager.GetPointFillUp(), .25f).SetEase(Ease.OutBack));
        scaleFillUp = 1.9f;
        tweenAnimations.Add(transform.DOScale(1.9f, .25f).SetEase(Ease.OutBack).OnComplete(()=> {
            //transform.DORotate(new Vector3(0, 360f, 0), 1f, RotateMode.WorldAxisAdd);
            listCakeIDFillUp.Clear();
            indexRemove = 0;
            pieceCakeIDFill = pieces[0].cakeID;
            for (int i = pieces.Count - 1; i >= 0; i--)
            {
                Debug.Log("Remove");
                if (pieces[i].cakeID != pieceCakeIDFill)
                {
                    pieces[i].RemoveByFillUp(indexRemove);
                    pieces.Remove(pieces[i]);
                    indexRemove++;
                }
              
            }
            currentRotateIndex = pieces[pieces.Count - 1].currentRotateIndex;
            
            DOVirtual.DelayedCall((indexRemove + 1) * .25f, () => {
                fillUpEffect = GameManager.Instance.objectPooling.GetFillUpEffect();
                fillUpEffect.position = transform.position + vectorOffsetEffectDrop;
                fillUpEffect.gameObject.SetActive(true);
                indexRemove = 0;
                for (int i = pieces.Count; i < 6; i++)
                {
                    DOVirtual.DelayedCall(indexRemove * 0.25f, InitPieces);
                    indexRemove++;
                }
                DOVirtual.DelayedCall(indexRemove * 0.39f, () =>
                {
                    tweenAnimations.Add(transform.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.WorldAxisAdd).SetEase(Ease.InCirc).OnComplete(()=> {
                        transform.DOLocalMove(Vector3.zero, .3f).SetEase(Ease.OutBack);
                        fillUpEffect.gameObject.SetActive(false);
                        GameManager.Instance.itemManager.UsingItemDone();
                        ProfileManager.Instance.playerData.playerResourseSave.UsingItem(ItemType.FillUp);
                        DoneCakeMode();
                        ProfileManager.Instance.playerData.cakeSaveData.RemoveCake(currentPlate.plateIndex);
                        UsingFillUpDone();
                    }));

                    tweenAnimations.Add(transform.DOScale(scaleFillUp + .3f, .15f));
                    tweenAnimations.Add(transform.DOScale(scaleFillUp - .1f, .15f).SetDelay(.15f));
                    tweenAnimations.Add(transform.DOScale(scaleFillUp, .15f).SetDelay(.3f));
                   
                });
            });

          
        }));

    }

    void UsingHammer() {
          if (cakeDone)
            return;
        if (currentPlate != null)
            currentPlate.currentCake = null;
        if (myGroupCake != null)
        {
            if (GameManager.Instance.cakeManager.CakeOnWait(myGroupCake))
                return;
        }
        GameManager.Instance.itemManager.OnUsingItem();
        ProfileManager.Instance.playerData.playerResourseSave.UsingItem(ItemType.Hammer);
        ProfileManager.Instance.playerData.cakeSaveData.RemoveCake(currentPlate.plateIndex);
        GameManager.Instance.itemManager.CallUsingHammerOnCake(this, CallBackOnAnimHammerDone);
        EventManager.TriggerEvent(EventName.UsingHammerDone.ToString());
    }

    public void SetActiveCollider(bool active)
    {
        myCollider.enabled = active;
    }

    public void UsingRevive(bool lastCake = false) {
        if (cakeDone)
            return;
        if (currentPlate != null)
            currentPlate.currentCake = null;
        if (myGroupCake != null)
        {
            if (GameManager.Instance.cakeManager.CakeOnWait(myGroupCake))
                return;
        }

        this.lastCake = lastCake;
        if (!lastCake)
            GameManager.Instance.itemManager.Revie(this, CallBackOnReviveDone, lastCake);
        else
        {
            GameManager.Instance.itemManager.Revie(this, CallBackOnAnimHammerDone, lastCake);

        }

    }

    public void CallBackOnAnimHammerDone() {
        tweenAnimations.Add(transform.DOScale(0f, 0.25f).OnComplete(() => { gameObject.SetActive(false); }));
    }
    PlateIndex plateIndex;
    bool lastCake;
    void CallBackOnReviveDone() {
        CallBackOnAnimHammerDone();
        if (!centerRevive)
        {
            GameManager.Instance.itemManager.RemoveCake();
            return;
        }
       
        GameManager.Instance.itemManager.AssignCakeCallBack(this);
        plateIndex = new(currentPlate.plateIndex);
        ProfileManager.Instance.playerData.cakeSaveData.RemoveCake(plateIndex);
        GameManager.Instance.itemManager.RemoveCake();
    }

    

    public bool CheckDrop()
    {
        if (GameManager.Instance.cakeManager.onCheckLooseGame)
            return false;
        if (currentPlate != null && currentPlate.currentCake == null)
        {
            currentPlate.SetCurrentCake(this);
            currentPlate.Deactive();
            return true;
        }
        return false;
    }

    
    public void DropDone(bool lastDrop, UnityAction actionCallback) {
        doneDrop = true;
        myCollider.enabled = false;
        onDrop = true;
        transform.parent = currentPlate.pointStay;
        tweenAnimations.Add(transform.DOLocalMove(Vector3.zero, .1f).SetEase(Ease.InQuad).OnComplete(()=> {
            Transform effectDrop = GameManager.Instance.objectPooling.GetSmokeEffectDrop();
            effectDrop.transform.position = transform.position;
            effectDrop.gameObject.SetActive(true);
        }));
        //Debug.Log("last drop: "+ lastDrop);
        DOVirtual.DelayedCall(.1f, () =>
        {
            if (lastDrop)
                actionCallback();
        });

        //transform.DOScale(Vector3.one * .9f, .25f).OnComplete(() =>
        //{
        //    if (lastDrop)
        //        actionCallback();
        //    transform.DOScale(Vector3.one * 1.1f, .2f);
        //    transform.DOScale(Vector3.one, .2f).SetDelay(.2f);

        //});
        if (ProfileManager.Instance.playerData.playerResourseSave.currentLevel == 0)
        {
            GameManager.Instance.tutorialManager.PlayTutorial();
        }
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
        //Debug.DrawLine(transform.position, transform.position - vectorCheckOffset);
        if (Physics.Linecast(transform.position, transform.position - vectorCheckOffset, out hitInfor))
        {
            //Debug.Log(hitInfor.collider.gameObject.name);
            Debug.DrawLine(transform.position, hitInfor.point);
            if (hitInfor.collider.gameObject.layer == 6)
            {
                Plate plate = hitInfor.collider.gameObject.GetComponent<Plate>();
                if (currentPlate != null)
                    currentPlate.Deactive();
                if (!plate.actived) return;
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position - (transform.up * .1f + vectorCheckOffset), radiusCheck);
    }

    public void SetGroupCake(GroupCake groupCake)
    {
        myGroupCake = groupCake;
    }

    
    public bool GetCakePieceSame(int cakeID) {
        piece = pieces.Find(e => (e.cakeID == cakeID && e.gameObject.activeSelf));
        return piece != null;
    }
    public List<Piece> listPieceTemp = new();
    public int MakeRotateIndexForNewPiece(int cakeID) {
        otherCake = false;
        sameCake = false;
        indexReturn = -1;
        listPieceTemp.Clear();
        listPieceTemp = new(pieces);
        for (int i = 0; i < listPieceTemp.Count; i++)
        {
            if (listPieceTemp[i].cakeID != cakeID)
                otherCake = true;

            if (listPieceTemp[i].cakeID == cakeID)
                sameCake = true;

            if (otherCake && sameCake)
            {
                    indexOfNewPiece = i;
                    indexReturn = listPieceTemp[i].currentRotateIndex;
                    //Debug.Log(pieces[i] + " " + i);
                    //Debug.Log("id: " + cakeID + " index return: " + indexReturn);
                    break;
            }

        }
        if (indexReturn == -1)
        {
            indexOfNewPiece = listPieceTemp.Count;
            indexReturn = listPieceTemp[listPieceTemp.Count - 1].currentRotateIndex + 1;
            //Debug.Log("id: "+ cakeID +" index return: " + indexReturn);
        }
        return indexReturn;
    }

    public bool IsHaveCakeID(int cakeID)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID == cakeID)
            {
                return true;
            }
        }
        return false;
    }

    public void StartRotateOtherPieceForNewPiece(UnityAction actionCallBack) {
        //Debug.Log("rotate other+ " + currentPlate);
        actionCallBackRotateDone = actionCallBack;
        if (!cakeDone)
        {
            flagDoActionCallBack = false;
            RotateOtherPiece(indexOfNewPiece + 1);
        }
    }
 
    void RotateOtherPiece(int pieceIndex) {
        indexRotate = pieceIndex;
        while (indexRotate < pieces.Count)
        {
            pieces[indexRotate].currentRotateIndex++;
            if (pieces[indexRotate].currentRotateIndex >= rotates.Count) pieces[indexRotate].currentRotateIndex = 0;
            vectorRotateTo = new Vector3(0, rotates[pieces[indexRotate].currentRotateIndex], 0);
            tweenAnimations.Add(pieces[indexRotate].transform.DORotate(vectorRotateTo, timeRotate).SetEase(curveRotate).OnComplete(() => {
                if (indexRotate == pieces.Count - 1)
                {
                    flagDoActionCallBack = true;
                    actionCallBackRotateDone();
                }
            }));
            indexRotate++;
        }
        if (!flagDoActionCallBack) actionCallBackRotateDone();

    }
  
    public void RotateOtherPieceRight(UnityAction actionCallRotateDone) {
        //Debug.Log("rotate right way+ "+currentPlate);
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
        RotateOtherPieceRightWay();
       
    }

    void RotateOtherPieceRightWay() {
        while (indexRotateRW < pieces.Count) {
            if (currentRotateIndex >= rotates.Count)
                currentRotateIndex = 0;
            if (pieces[indexRotateRW].currentRotateIndex != currentRotateIndex)
                pieces[indexRotateRW].currentRotateIndex = currentRotateIndex;
            vectorRotateTo = new Vector3(0, rotates[pieces[indexRotateRW].currentRotateIndex], 0);
            tweenAnimations.Add(pieces[indexRotateRW].transform.DORotate(vectorRotateTo, timeRotate).SetEase(curveRotate));
            DOVirtual.DelayedCall(timeRotate - .15f, () =>
            {
                if (indexRotateRW == pieces.Count)
                {
                    callBackRotateDone = true;
                    if (rotateRWDone != null) 
                        rotateRWDone();
                }
            });
            indexRotateRW++;
            currentRotateIndex++;
            //yield return new WaitForSeconds(timeRotate-.15f);
        }

        if (!callBackRotateDone && rotateRWDone != null)
            rotateRWDone();
    }

    public Piece GetPieceMove(int cakeID)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID == cakeID)
            {
                piece = pieces[i];
                //Debug.Log("Piece before: "+ piece.cakeID);
                int indexID = pieceCakeID.IndexOf(pieces[i].cakeID);
                pieces.Remove(pieces[i]);
                if (indexID != -1)
                if (pieceCakeIDCount[indexID] > 0)
                {
                    pieceCakeIDCount[indexID]--;
                        if (pieceCakeIDCount[indexID] <= 0)
                        {
                            pieceCakeIDCount.RemoveAt(indexID);
                            pieceCakeID.RemoveAt(indexID);
                        }
                }
               
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
        //Debug.Log("Best plate: "+currentPlate);
        int pieceCount = 0;
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID == cakeID)
                pieceCount++;
        }
        //Debug.Log("IS DONE: " + (pieceCount >= totalPieces));
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
        pieces.Add(null);
        for (int i = pieces.Count-1 ; i >= indexOfNewPiece + 1; i--)
        {
            pieces[i] = pieces[i - 1];
        }
        pieces[indexOfNewPiece] = piece;
        if (!pieceCakeID.Contains(piece.cakeID))
        {
            pieceCakeID.Add(piece.cakeID);
            pieceCakeIDCount.Add(1);
        }
        else {
            pieceCakeIDCount[pieceCakeID.IndexOf(piece.cakeID)]++;
        }
    }

    public bool CheckCakeIsDone(int cakeID) {
        if (pieces.Count < 6) return false;
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].cakeID != cakeID)
               return false;
        }
        cakeDone = true;
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
        GameManager.Instance.audioManager.PlaySoundEffect(SoundId.SFX_TapCube);
        GameManager.Instance.questManager.AddProgress(QuestType.CompleteCake, 1);
        GameManager.Instance.quickTimeEventManager.AddProgess(pieces[0].cakeID, transform);
        if (panelTotal == null)
            panelTotal = UIManager.instance.panelTotal;

        GameManager.Instance.cakeManager.AddStreak(this);
        int cakeLevel = 0;
        if (pieces.Count > 0)
            cakeLevel = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCakeLevel(pieces[0].cakeID);

        GameManager.Instance.AddPiggySave(GameManager.Instance.GetDefaultCakeProfit(pieces[0].cakeID, cakeLevel, true));
        if(!ProfileManager.Instance.playerData.playerResourseSave.AddExp(GameManager.Instance.GetDefaultCakeProfit(pieces[0].cakeID, cakeLevel)))
        {
            GameManager.Instance.cakeManager.AddCakeCount();
        }
        ProfileManager.Instance.playerData.playerResourseSave.AddMoney(GameManager.Instance.GetDefaultCakeProfit(pieces[0].cakeID, cakeLevel, true));
        ProfileManager.Instance.playerData.playerResourseSave.AddTrophy((int)GameManager.Instance.GetDefaultCakeProfit(pieces[0].cakeID, cakeLevel));
        DOVirtual.DelayedCall(0.18f, () => {
            tweens.Add(transform.DOScale(Vector3.one * .8f, .13f));
            tweens.Add(transform.DOScale(Vector3.one * 1.32f, .5f).SetDelay(.13f).SetEase(Ease.OutBack));
            tweenAnimations.Add(transform.DORotate(CacheSourse.rotateY360, .75f, RotateMode.WorldAxisAdd).SetEase(Ease.OutQuad).OnComplete(() => {
                EffectDoneCake();
            }));
        });
       
    }

    void EffectDoneCake() {
        if (pieces.Count == 0)
            return;
        int cakeLevel = ProfileManager.Instance.playerData.cakeSaveData.GetOwnedCakeLevel(pieces[0].cakeID);
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
        expEffect.ChangeText((GameManager.Instance.GetDefaultCakeProfit(pieces[0].cakeID, cakeLevel)).ToString());
        expEffect.gameObject.SetActive(true);

        tweenAnimations.Add(transform.DOScale(0f, .3f).SetEase(Ease.InQuad));

        DOVirtual.DelayedCall(CacheSourse.float05, () => {
            //Debug.Log("Destroy now");
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
        tweens.Add(transform.DOScale(scaleDefault-.1f, CacheSourse.float013).SetEase(Ease.InSine));
        tweens.Add(transform.DOScale(scaleDefault+.1f, CacheSourse.float013).SetEase(Ease.InOutSine).SetDelay(CacheSourse.float013));
        tweens.Add(transform.DOScale(scaleDefault, CacheSourse.float013).SetEase(Ease.OutSine).SetDelay(CacheSourse.float026));
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

    public bool CakeIsNull() {
        //Debug.Log(currentPlate+" have cake null: "+pieces.Count);
        return pieces.Count == 0; 
    }
    
    public void ClearAnimation() {
        if (tweenAnimations.Count > 0)
        {
            for (int i = 0; i < tweenAnimations.Count; i++)
            {
                if (tweenAnimations[i] != null) { tweenAnimations[i].Kill(); }
            }
        }
    }

    private void OnDestroy()
    {
        if (tweens.Count > 0)
            for (int i = 0; i < tweens.Count; i++)
                tweens[i].Kill();
    }
}
