using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Cake : MonoBehaviour
{
    public List<Pieces> pieces = new List<Pieces>();
    public List<int> rotates = new List<int>();
    public int totalPieces;
    public int totalCakeID;
    public float radiusCheck;
    public Plate currentPlate;
    GroupCake myGroupCake;
    public List<int> pieceCakeIDCount = new List<int>();
    public List<int> pieceCakeID = new List<int>();
    public void InitData() {
        totalPieces = GameManager.Instance.cakeManager.GetPiecesTotal() + 1;
        SetupPiecesCakeID();
        pieceIndex = 0;
        SetUpCakeID();
        for (int i = 0; i < pieceCakeIDCount.Count; i++)
        {
            InitPiecesSame(pieceCakeIDCount[i], pieceCakeID[i]);
        }
        for (int i = totalPieces; i < pieces.Count; i++) {
            pieces[i].gameObject.SetActive(false);
        }
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
        cakeID = ProfileManager.Instance.playerData.cakeSaveData.cakeID;
        System.Random rd = new System.Random();
        for (int i = 0; i < pieceCakeID.Count; i++)
        {
            int randomIndexX = (int)(rd.Next(cakeID.Count));
            pieceCakeID[i] = cakeID[randomIndexX];
        }
    }

    int pieceIndex;
    void InitPiecesSame(int totalPiecesSame, int pieceCakeID) {
        int pieceCountSame = 0;
        while (pieceCountSame < totalPiecesSame) {
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

    public void DropDone() {
        transform.position = currentPlate.pointStay.position;
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

    public void CheckOnMouse() {
        if (Physics.SphereCast(transform.position, radiusCheck, -transform.up * .1f, out hitInfor))
        {
            if (hitInfor.collider.gameObject.layer == 6)
            {
                Plate plate = hitInfor.collider.gameObject.GetComponent<Plate>();
                if (currentPlate != null)
                    currentPlate.Deactive();
                if (plate.currentCake != null)
                {
                    currentPlate = null;
                    return;
                }
                currentPlate = plate;
                plate.Active();
            }
        }
        else if (currentPlate != null) {
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
    Pieces piece;
    public bool GetCakePieceSame(int cakeID) {
        piece = pieces.Find(e => e.cakeID == cakeID);
        return piece != null;
    }
}
