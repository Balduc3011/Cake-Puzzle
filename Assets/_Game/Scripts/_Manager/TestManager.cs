using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public List<CakeOnWait> cakeOnPlate = new();

    public List<Transform> trsSpawn = new();

    public GroupCake groupCakeTemp;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) {
            Spawn();
        }
    }

    int indexGroupCake = 0;

    void Spawn() {
        indexGroupCake = 0;
        StartCoroutine(IE_WaitToInitGroupCakeFromSave());
    }

    IEnumerator IE_WaitToInitGroupCakeFromSave()
    {
        while (indexGroupCake < 3)
        {
            if (cakeOnPlate[indexGroupCake].cakeSaves.Count > 0)
            {
                groupCakeTemp = GameManager.Instance.objectPooling.GetGroupCake();
                groupCakeTemp.transform.position = trsSpawn[indexGroupCake].position;
                groupCakeTemp.InitData(cakeOnPlate[indexGroupCake].cakeSaves.Count, trsSpawn[indexGroupCake], indexGroupCake, cakeOnPlate[indexGroupCake].cakeSaves);
                yield return new WaitForSeconds(.25f);
            }
            indexGroupCake++;
        }
    }
}
