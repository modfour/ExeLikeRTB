using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : DamageContainer
{
    private float deleteTime; // 斬撃を消すまでの秒数
    private float time = 0.0f; // 生成してからの経過時間

    // プロパティ定義
    public float DeleteTime
    {
        set { this.deleteTime = value; }
        get { return this.deleteTime; }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.Priority = 10;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        // 消滅処理
        if (time > deleteTime)
        {
            Destroy(gameObject);
        }
    }
}
