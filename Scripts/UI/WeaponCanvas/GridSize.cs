using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSize : MonoBehaviour {

    public Scrollbar bar;
    public GridLayoutGroup layout;
    public RectTransform rect;

    private void Start()
    {
        bar.value = 1;
        
    }
    private void Update()
    {
        layout.cellSize = new Vector2(rect.rect.xMax * 2, rect.rect.xMax / 2);
    }
}
