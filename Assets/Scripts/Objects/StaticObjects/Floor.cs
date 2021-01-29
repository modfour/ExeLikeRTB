using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    // battleManagerを格納する変数
    private BattleManager battleManager;

    // 床が移動できるか、自分のものかを判定するための変数
    private Boolean isMovable;

    // 自分の座標
    private int column;
    private int row;

    // マス上に存在するダメージ判定を格納するリスト
    private List<DamageContainer> damageContainers;

    // プロパティ定義
    public Boolean IsMovable
    {
        set { this.isMovable = value;}
        get { return this.isMovable; }
    }
    public int Column
    {
        set { this.column = value; }
        get { return this.column; }
    }
    public int Row
    {
        set { this.row = value; }
        get { return this.row; }
    }
    public List<DamageContainer> DamageContainers
    {
        get { return this.damageContainers; }
    }

    // Start is called before the first frame update
    void Start()
    {
        damageContainers = new List<DamageContainer>();
    }

    // 攻撃エフェクトが接触した際の動作
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // マス目にダメージ判定を追加する
        DamageContainer damageContainer = collider.gameObject.GetComponent<DamageContainer>();
        this.damageContainers.Add(damageContainer);

        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        sr.color = new Color(1.0f,1.0f,0.5f,1.0f);
    }

    // 攻撃エフェクトが出た際の動作
    private void OnTriggerExit2D(Collider2D collider)
    {
        // マス目からダメージ判定を削除する
        DamageContainer damageContainer = collider.gameObject.GetComponent<DamageContainer>();
        this.damageContainers.Remove(damageContainer);

        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        sr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
