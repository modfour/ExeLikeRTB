using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletIcon : MonoBehaviour
{
    private bool isPlayers; // プレイヤーのものか、エネミーのものか
    private int index; // 何個目の銃弾か
    private RectTransform rectTransform; // 自分のRectTransform(位置調整に使用)

    // プロパティ定義
    public int Index
    {
        set { this.index = value; }
        get { return this.index; }
    }

    public bool IsPlayers
    {
        set { this.isPlayers = value; }
        get { return this.isPlayers; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
        Vector3 anchorPosition = new Vector3(50.0f * this.index + 50.0f, 0.0f, 0.0f);
        rectTransform.anchoredPosition = anchorPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
