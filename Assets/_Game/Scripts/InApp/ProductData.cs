using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ProductsData" ,menuName = "ScriptableObjects/ProductData")]
public class ProductData : ScriptableObject
{
    public List<OffersIAP> productIds;
}
