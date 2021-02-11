using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class EnemyController : Character
{
    // 移動する間隔(秒)
    public float moveInterval;
    // 移動制御用の秒数カウント
    private float time = 0.0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        this.IsPlayer = false; // 敵なので
        base.Start();
        this.Column = 8;
        this.Row = 3;
        CalcMyPos(this.Column, this.Row);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        time += Time.deltaTime;

        if (time > moveInterval)
        {
            // 敵用の移動ロジックを呼び出して場所を更新
            EnemyBattleMove();
            CalcMyPos(this.Column, this.Row);
            EnemyBattleAction();
            // カウンタをリセット
            time = 0.0f;
        }
    }

    // 敵用の移動ロジック
    // 縦、横のいずれかにランダムで移動する
    private void EnemyBattleMove()
    {
        // 乱数で移動方向を決定(0:移動しない、1:左、2:右、3:上、4:下)
        int direction = UnityEngine.Random.Range(0,5);
        MoveToward(direction);
    }

    // 敵用の攻撃ロジック
    private void EnemyBattleAction()
    {
        // 乱数で攻撃方法を決定(～10:銃撃、～13:斬撃)
        int action = UnityEngine.Random.Range(0, 20);
        if (action < 10) {
            makeBullet();
        }else if (action < 13)
        {
            makeSlash();
        }
    }
}
