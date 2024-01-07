using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] Animator anim;
    public Transform pointStay;
    public Cake currentCake;

    public void SetCurrentCake(Cake cake) { currentCake = cake; }

    public void CakeDone() { currentCake = null; }

    public void Active() {
        anim.SetBool("Active", true);
    }

    public void Deactive()
    {
        anim.SetBool("Active", false);
    }
}
