using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnMouseDown()
    {
        
    }

    [SerializeField] float radiusCheck;
    [SerializeField] Vector3 vectorCheckOffset;
    RaycastHit hitInfor;
    Plate currentPlate;

    public void CheckOnMouse()
    {
        if (Physics.SphereCast(transform.position, radiusCheck, -transform.up * .1f + vectorCheckOffset, out hitInfor))
        {
            if (hitInfor.collider.gameObject.layer == 6)
            {
                Plate plate = hitInfor.collider.gameObject.GetComponent<Plate>();
                if (currentPlate != null)
                    currentPlate.Deactive();
                currentPlate = plate;
                plate.ActiveByItem();
            }
            else DeActiveCurrentPlate();
        }
        else DeActiveCurrentPlate();
    }

    void DeActiveCurrentPlate() { 
        if (currentPlate != null) { currentPlate.DeActiveByItem(); }
    }
}
