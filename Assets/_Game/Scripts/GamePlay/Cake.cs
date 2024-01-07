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
    public float radiusCheck;
    public Plate currentPlate;
    GroupCake myGroupCake;

    public void InitData() {
        totalPieces = GameManager.Instance.cakeManager.GetPiecesTotal() + 1;
        for (int i = 0; i < pieces.Count; i++) {
            if (i >= totalPieces) pieces[i].gameObject.SetActive(false);
            else InitPiece(i);
        }
    }

    void InitPiece(int pieceInidex) {
        pieces[pieceInidex].transform.eulerAngles = Vector3.zero;
        int cakeID = GameManager.Instance.cakeManager.GetRandomPieces();
        GameObject objecPref = Resources.Load("Pieces/Piece_" + cakeID) as GameObject;
        pieces[pieceInidex].InitData(objecPref);

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

    public bool Drop()
    {
        if (currentPlate != null && currentPlate.currentCake == null)
        {
            transform.position = currentPlate.pointStay.position;
            currentPlate.SetCurrentCake(this);
            currentPlate.Deactive();
            return true;
        }
        return false;
    }

    public void GroupDropFail() {
        if (currentPlate != null)
        {
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
}
