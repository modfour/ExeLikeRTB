using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // マウスカーソルの座標をVector2型変数CursorPosに取得
        Vector2 cursorPos = Input.mousePosition;

        // cursorPos無いの座標データをスクリーン座標からワールド座標に変換
        cursorPos = Camera.main.ScreenToWorldPoint(cursorPos);

        // trasformコンポーネントのpositionに計算したcursorPosを代入
        transform.position = cursorPos;
        Debug.Log(cursorPos);
    }
}
