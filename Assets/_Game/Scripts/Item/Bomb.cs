using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (bang) return;
        needCheck = true;
        myAnim.SetBool("Using", true);
    }
    private void OnMouseUp()
    {
        myAnim.SetBool("Using", false);
        needCheck = false;
        if (currentPlate != null)
        {
            myAnim.SetBool("Active", true);
            bang = true;
            StartCoroutine(WaitBombUsing());
        }
        else { transform.position = GameManager.Instance.itemManager.GetPointItemIn(); }
    }

    [SerializeField] float radiusCheck;
    [SerializeField] Vector3 vectorCheckOffset;
    [SerializeField] Animator myAnim;
    RaycastHit hitInfor;
    Plate currentPlate;
    bool needCheck;
    bool bang;
    Vector3 pointFollowMouse;
    Vector3 vectorOffset = new Vector3(0, 0, .5f);

    private void Update()
    {
        if (needCheck) {
            pointFollowMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointFollowMouse.y = 1.5f;
            transform.position = pointFollowMouse + vectorOffset;
            CheckOnMouse();
        } else DeActiveCurrentPlate();
    }
    public void CheckOnMouse()
    {
        if (Physics.SphereCast(transform.position, radiusCheck, -transform.up * .1f + vectorCheckOffset, out hitInfor))
        {
            if (hitInfor.collider.gameObject.layer == 6)
            {
                Plate plate = hitInfor.collider.gameObject.GetComponent<Plate>();
                if (plate == currentPlate) return;
                if (currentPlate != null)
                    DeActiveCurrentPlate();
                currentPlate = plate;
                ActivePlate(currentPlate);
            }
            else ResetPoint();
        }
        else ResetPoint();
    }

    void ResetPoint() {
        DeActiveCurrentPlate();
        currentPlate = null;
        
    }


    PlateIndex plateIndex;
    void ActivePlate(Plate plate) {
        if (plateIndex == null) return;
        plate.ActiveByItem();
        plateIndex = plate.GetPlateIndex();
        GameManager.Instance.cakeManager.table.ActivePlate(plateIndex.indexX, plateIndex.indexY);

        GameManager.Instance.cakeManager.table.ActivePlate(plateIndex.indexX, plateIndex.indexY - 1);
        GameManager.Instance.cakeManager.table.ActivePlate(plateIndex.indexX, plateIndex.indexY + 1);

        GameManager.Instance.cakeManager.table.ActivePlate(plateIndex.indexX - 1, plateIndex.indexY);
        GameManager.Instance.cakeManager.table.ActivePlate(plateIndex.indexX - 1, plateIndex.indexY - 1);
        GameManager.Instance.cakeManager.table.ActivePlate(plateIndex.indexX - 1, plateIndex.indexY + 1);

        GameManager.Instance.cakeManager.table.ActivePlate(plateIndex.indexX + 1, plateIndex.indexY);
        GameManager.Instance.cakeManager.table.ActivePlate(plateIndex.indexX + 1, plateIndex.indexY - 1);
        GameManager.Instance.cakeManager.table.ActivePlate(plateIndex.indexX + 1, plateIndex.indexY + 1);
    }

    void DeActiveCurrentPlate() {
        if (currentPlate != null) {
            currentPlate.DeActiveByItem();
            plateIndex = currentPlate.GetPlateIndex();
            GameManager.Instance.cakeManager.table.DeActivePlate(plateIndex.indexX, plateIndex.indexY);

            GameManager.Instance.cakeManager.table.DeActivePlate(plateIndex.indexX, plateIndex.indexY - 1);
            GameManager.Instance.cakeManager.table.DeActivePlate(plateIndex.indexX, plateIndex.indexY + 1);

            GameManager.Instance.cakeManager.table.DeActivePlate(plateIndex.indexX - 1, plateIndex.indexY);
            GameManager.Instance.cakeManager.table.DeActivePlate(plateIndex.indexX - 1, plateIndex.indexY - 1);
            GameManager.Instance.cakeManager.table.DeActivePlate(plateIndex.indexX - 1, plateIndex.indexY + 1);

            GameManager.Instance.cakeManager.table.DeActivePlate(plateIndex.indexX + 1, plateIndex.indexY);
            GameManager.Instance.cakeManager.table.DeActivePlate(plateIndex.indexX + 1, plateIndex.indexY - 1);
            GameManager.Instance.cakeManager.table.DeActivePlate(plateIndex.indexX + 1, plateIndex.indexY + 1);
        }
    }

    void ClearCake() {
        currentPlate.DeActiveByItem();
        plateIndex = currentPlate.GetPlateIndex();
        GameManager.Instance.cakeManager.table.ClearPlateByBomb(plateIndex.indexX, plateIndex.indexY);

        GameManager.Instance.cakeManager.table.ClearPlateByBomb(plateIndex.indexX, plateIndex.indexY - 1);
        GameManager.Instance.cakeManager.table.ClearPlateByBomb(plateIndex.indexX, plateIndex.indexY + 1);

        GameManager.Instance.cakeManager.table.ClearPlateByBomb(plateIndex.indexX - 1, plateIndex.indexY);
        GameManager.Instance.cakeManager.table.ClearPlateByBomb(plateIndex.indexX - 1, plateIndex.indexY - 1);
        GameManager.Instance.cakeManager.table.ClearPlateByBomb(plateIndex.indexX - 1, plateIndex.indexY + 1);

        GameManager.Instance.cakeManager.table.ClearPlateByBomb(plateIndex.indexX + 1, plateIndex.indexY);
        GameManager.Instance.cakeManager.table.ClearPlateByBomb(plateIndex.indexX + 1, plateIndex.indexY - 1);
        GameManager.Instance.cakeManager.table.ClearPlateByBomb(plateIndex.indexX + 1, plateIndex.indexY + 1);

        GameManager.Instance.itemManager.UsingItemDone();

        ProfileManager.Instance.playerData.playerResourseSave.UsingItem(ItemType.Bomb);
    }

    IEnumerator WaitBombUsing() { 
        yield return new WaitForSeconds(1f);
        ClearCake();
    }
}
