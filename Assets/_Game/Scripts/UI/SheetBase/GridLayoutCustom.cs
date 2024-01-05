using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLayoutCustom : MonoBehaviour
{
    [SerializeField] GridLayoutGroup myGrid;
    [SerializeField] RectTransform myRect;
    [SerializeField] int columnCount;
    [SerializeField] bool fixedHeight;
    private void OnEnable()
    {
        float dir = myGrid.cellSize.x / myGrid.cellSize.y;
        float width = (myRect.rect.width - myGrid.padding.left - myGrid.padding.right - myGrid.spacing.x * (columnCount - 1)) / columnCount;
        float height = myGrid.cellSize.y;
        if (!fixedHeight)
        {
            height = width / dir;
        }
        
        myGrid.cellSize = new Vector2(width, height);
    }
}
