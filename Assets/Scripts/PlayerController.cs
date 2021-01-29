using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        this.IsPlayer = true;
        base.Start();
        this.Column = 3;
        this.Row = 3;
        CalcMyPos(this.Column, this.Row);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // プレイヤー用の移動ロジックを呼び出す
        PlayerBattleMove();

        // 場所を更新
        CalcMyPos(this.Column, this.Row);

        // 弾を撃つ
        if (Input.GetKeyDown(KeyCode.Z))
        {
            makeBullet();
        }
        // 斬撃を放つ
        if (Input.GetKeyDown(KeyCode.X))
        {
            makeSlash();
        }
    }

    // キーボード入力から移動先判定を行って移動する
    private void PlayerBattleMove()
    {
        // 左右に移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveToward((int)CommonConstants.Direction.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveToward((int)CommonConstants.Direction.right);
        }
        // 上下に移動
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveToward((int)CommonConstants.Direction.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveToward((int)CommonConstants.Direction.down);
        }
    }


}
