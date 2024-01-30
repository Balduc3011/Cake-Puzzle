using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnMouseDown()
    {
        needCheck = true;
    }
    private void OnMouseUp()
    {
        needCheck = false;
        if (currentPlate != null)
        {
            myAnim.SetBool("Active", true);
        }
    }

    [SerializeField] float radiusCheck;
    [SerializeField] Vector3 vectorCheckOffset;
    [SerializeField] Animator myAnim;
    RaycastHit hitInfor;
    Plate currentPlate;
    bool needCheck;
    Vector3 pointFollowMouse;
    Vector3 vectorOffset = new Vector3(0, 0, 2f);

    private void Update()
    {
        if (needCheck) {
            pointFollowMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointFollowMouse.y = 1.5f;
            transform.position = pointFollowMouse + vectorOffset;
            CheckOnMouse();
        }else DeActiveCurrentPlate();
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
                StartCoroutine(IE_WaitToActivePlate());
            }
            else DeActiveCurrentPlate();
        }
        else DeActiveCurrentPlate();
    }

    IEnumerator IE_WaitToActivePlate() {
        yield return new WaitForSeconds(0.1f);
        ActivePlate(currentPlate);
    }

    PlateIndex plateIndex;
    void ActivePlate(Plate plate) {
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
}
