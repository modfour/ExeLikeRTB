using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour
{
    // 攻撃のPrefab
    public GameObject bulletPrefab;
    public GameObject bulletIconPrefab;
    public GameObject slashPrefab;

    private bool isPlayer; // プレイヤーキャラかどうかを表す変数
    private int column; // フィールド上での位置
    private int row; // フィールド上での位置
    private int direction; // 1なら右向き、-1なら左向き

    private int hitPoint; // HP
    private int bulletCount = 6; // 残弾の数
    private float waitTime; // 硬直時間、これが0なら行動できる
    private bool waitTimeApplyFlag; // 硬直時間を反映させるためのフラグ、これがtrueなら反映させる

    // キャラクターのアニメーションコントローラ
    private Animator animator;

    // 子オブジェクトを保持するための変数群
    private GameObject hitPointText;
    private GameObject characterSprite;

    // ゲーム内の各種管理オブジェクトを格納するための変数
    private BattleManager battleManager;

    // UI要素を保持するための変数群
    GameObject charactersPannel;
    private GameObject[] bulletIcons = new GameObject[6];

    // プロパティ定義
    public bool IsPlayer
    {
        set { this.isPlayer = value; }
        get { return this.isPlayer; }
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
    public int HitPoint
    {
        set { this.hitPoint = value; }
        get { return this.hitPoint; }
    }
    public int Direction
    {
        set { this.direction = value; }
        get { return this.direction; }
    }
    public Animator Animator
    {
        get { return this.animator; }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        this.HitPoint = CommonConstants.initHitPoint;
        // 子のゲームオブジェクトを取得
        hitPointText = this.transform.Find("HitPointText").gameObject;
        characterSprite = this.transform.Find("CharacterSprite").gameObject;
        animator = characterSprite.GetComponent<Animator>();

        hitPointText.GetComponent<TextMeshPro>().text = hitPoint.ToString();

        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();

        // UI要素の初期化
        if (isPlayer)
        {
            charactersPannel = GameObject.Find("PlayersPannel");
        }
        else
        {
            charactersPannel = GameObject.Find("EnemysPannel");
        }
        // bulletIconを配置
        for (int index = 0; index < 6; index++)
        {
            bulletIcons[index] = Instantiate(bulletIconPrefab);
            bulletIcons[index].GetComponent<BulletIcon>().Index = index;
            bulletIcons[index].GetComponent<BulletIcon>().IsPlayers = isPlayer;
            bulletIcons[index].transform.SetParent(charactersPannel.transform);
        }
    }

    // 継承先のUpdateよりも先に呼ぶこと
    protected virtual void Update()
    {
        // 何事もなければモーション値をゼロに戻す
        animator.SetInteger("MotionIndex", (int)CommonConstants.MotionIndex.idle);
        hitPointText.GetComponent<TextMeshPro>().text = hitPoint.ToString();
        if (waitTimeApplyFlag)
        {
            waitTime = this.Animator.GetCurrentAnimatorStateInfo(0).length;
            waitTimeApplyFlag = false;
        }
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
    }

    // 自分の座標を計算する
    public void CalcMyPos(int column, int row)
    {
        Vector3 myPos = new Vector3();
        myPos.x = (float)CommonConstants.positionZero.x + (float)CommonConstants.floorLength * column;
        myPos.y = (float)CommonConstants.positionZero.y * 2.5f - (float)CommonConstants.floorWidth * row;
        this.transform.position = myPos;
    }

    // 移動先への移動が可能かどうか
    public bool IsMovable(int column, int row, bool isPlayer)
    {
        return battleManager.FloorMaster.getIsMovable(column, row);
    }

    // キャラクター画像の方向を変える
    public void SetDirection(Vector3 vec3)
    {
        characterSprite.transform.localScale = vec3;
        direction = System.Math.Sign(vec3.x) * -1;
    }
    // 移動するロジック
    public void MoveToward(int direction)
    {
        if(waitTime <= 0)
        {
            switch (direction)
            {
                case (int)CommonConstants.Direction.left: // 左に移動
                    if (IsMovable(this.Column - 1, this.Row, this.IsPlayer))
                    {
                        SetBattleObjects(this.Column, this.Row, this.Column - 1, this.Row);
                        this.Column -= 1;
                        SetStepMotion(direction, this.direction);
                    }
                    break;
                case (int)CommonConstants.Direction.right: // 右に移動
                    if (IsMovable(this.Column + 1, this.Row, this.IsPlayer))
                    {
                        SetBattleObjects(this.Column, this.Row, this.Column + 1, this.Row);
                        this.Column += 1;
                        SetStepMotion(direction, this.direction);
                    }
                    break;
                case (int)CommonConstants.Direction.up: // 上に移動
                    if (IsMovable(this.Column, this.Row - 1, this.IsPlayer))
                    {
                        SetBattleObjects(this.Column, this.Row, this.Column, this.Row - 1);
                        this.Row -= 1;
                    }
                    break;
                case (int)CommonConstants.Direction.down: // 下に移動
                    if (IsMovable(this.Column, this.Row + 1, this.IsPlayer))
                    {
                        SetBattleObjects(this.Column, this.Row, this.Column, this.Row + 1);
                        this.Row += 1;
                    }
                    break;
                default: // 移動しない
                    break;
            }
        }
    }
    // 弾を撃つためのロジック(削除処理はインスタンス側で実装)
    public void makeBullet()
    {
        if(waitTime <= 0)
        {
            waitTimeApplyFlag = true;
            if (bulletCount > 0)
            {
                this.Animator.SetInteger("MotionIndex", (int)CommonConstants.MotionIndex.attack);
                GameObject bullet;
                bullet = Instantiate(bulletPrefab);

                bullet.GetComponent<Bullet>().DeleteTime = 1.0f;
                bullet.GetComponent<Bullet>().Direction = this.Direction;
                bullet.GetComponent<Bullet>().SetDamage(10, this.isPlayer);
                bullet.GetComponent<Bullet>().HitOnceOnly = true;

                Vector3 initPosition = this.transform.position;
                initPosition.x = initPosition.x + 0.1f * this.Direction;
                initPosition.z = -1.0f;
                bullet.transform.position = initPosition;
                bullet.transform.localScale = new Vector3((float)this.Direction, 1.0f, 1.0f);
                bulletIcons[--bulletCount].SetActive(false);
            }
            else
            {
                this.Animator.SetInteger("MotionIndex", (int)CommonConstants.MotionIndex.charge);
                for (bulletCount = 0; bulletCount < 6; bulletCount++)
                {
                    bulletIcons[bulletCount].SetActive(true);
                }
            }
        }
    }

    // 斬撃を放つためのロジック(削除処理はインスタンス側で実装)
    public void makeSlash()
    {
        if(waitTime <= 0)
        {
            waitTimeApplyFlag = true;
            this.Animator.SetInteger("MotionIndex", (int)CommonConstants.MotionIndex.attack2);
            GameObject slash;
            slash = Instantiate(slashPrefab);

            slash.GetComponent<Slash>().DeleteTime = 0.1f;
            slash.GetComponent<Slash>().SetDamage(30, this.isPlayer);
            slash.GetComponent<Slash>().HitOnceOnly = true;

            Vector3 initPosition = this.transform.position;
            initPosition.x = initPosition.x + 1.9f * this.Direction;
            initPosition.z = -1.0f;
            slash.transform.position = initPosition;
            slash.transform.localScale = new Vector3((float)this.Direction, 1.0f, 1.0f);
        }
    }

    // 移動方向とキャラクターの向きに応じてステップのモーションを変える
    private void SetStepMotion(int moveDirection, int charDirection)
    {
        if (moveDirection == (int)CommonConstants.Direction.left)
        {
            if (charDirection == 1)
            {
                this.Animator.SetInteger("MotionIndex", (int)CommonConstants.MotionIndex.backstep);
            }
            else
            {
                this.Animator.SetInteger("MotionIndex", (int)CommonConstants.MotionIndex.frontstep);
            }
        }
        else if (moveDirection == (int)CommonConstants.Direction.right)
        {
            if (charDirection == 1)
            {
                this.Animator.SetInteger("MotionIndex", (int)CommonConstants.MotionIndex.frontstep);
            }
            else
            {
                this.Animator.SetInteger("MotionIndex", (int)CommonConstants.MotionIndex.backstep);
            }
        }
    }

    private void SetBattleObjects(int beforeColumn, int beforeRow, int afterColumn, int afterRow)
    {
        battleManager.FloorMaster.BattleObjects[beforeColumn, beforeRow] = null;
        battleManager.FloorMaster.BattleObjects[afterColumn, afterRow] = this.gameObject;
        // Todo:キャラが移動した後は確定で移動できるようにするのではなく、もともとの移動可否に戻す
        // 例えば、穴あきマスに浮遊スキルで侵入後、移動すると穴あきマスにも関わらず移動可能になってしまう
        battleManager.FloorMaster.BattleField[beforeColumn, beforeRow].GetComponent<Floor>().IsMovable = true;
        battleManager.FloorMaster.BattleField[afterColumn, afterRow].GetComponent<Floor>().IsMovable = false;
    }
}
