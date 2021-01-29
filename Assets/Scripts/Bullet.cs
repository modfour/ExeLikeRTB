using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : DamageContainer
{
    private float speed; // 弾速
    private int direction; // 発射する方向(1 or -1)
    private float deleteTime; // 弾を消すまでの秒数
    private float time = 0.0f; // 生成してからの経過時間

    // プロパティ定義
    public float Speed
    {
        set { this.speed = value; }
        get { return this.speed; }
    }
    public int Direction
    {
        set { this.direction = value; }
        get { return this.direction; }
    }
    public float DeleteTime
    {
        set { this.deleteTime = value; }
        get { return this.deleteTime; }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.Priority = 1;
        this.Speed = 0.2f;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed, 0.0f, 0.0f);
        time += Time.deltaTime;

        // 消滅処理
        if (time > deleteTime){
            Destroy(gameObject);
        }
    }
}
