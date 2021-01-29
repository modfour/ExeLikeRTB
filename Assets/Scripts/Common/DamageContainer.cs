using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageContainer: MonoBehaviour
{
    public GameObject collisionEffectPrefab;
    private int playersDamage;
    private int enemysDamage;
    private bool hitOnceOnly; // ヒットしたら消えるか否か
    private int priority; // 判定の強さ,同等以上の攻撃に当たると相殺される(floorは0)

    public int PlayersDamage
    {
        set { this.playersDamage = value; }
        get { return this.playersDamage; }
    }
    public int EnemysDamage
    {
        set { this.enemysDamage = value; }
        get { return this.enemysDamage; }
    }
    public bool HitOnceOnly
    {
        set { this.hitOnceOnly = value; }
        get { return this.hitOnceOnly; }
    }
    public int Priority
    {
        set { this.priority = value; }
        get { return this.priority; }
    }

    // ダメージを加算する
    public void AddDamageContainer(DamageContainer damageContainer)
    {
        playersDamage += damageContainer.PlayersDamage;
        enemysDamage += damageContainer.EnemysDamage;
        hitOnceOnly = damageContainer.HitOnceOnly;
    }
    // ダメージを減算する
    public void SubtractDamageContainer(DamageContainer damageContainer)
    {
        playersDamage -= damageContainer.PlayersDamage;
        enemysDamage -= damageContainer.EnemysDamage;
    }

    // 敵味方それぞれのダメージを取得する
    public int GetDamage(bool isPlayer)
    {
        if (isPlayer)
        {
            return this.playersDamage;
        }
        else
        {
            return this.enemysDamage;
        }
    }
    // 敵味方それぞれのダメージをセットする
    public void SetDamage(int damage, bool isPlayer)
    {
        if (isPlayer)
        {
            this.enemysDamage = damage;
        }
        else
        {
            this.playersDamage = damage;
        }
    }

    // 攻撃エフェクトが接触した際の動作
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 優先度判定によって負けたら消滅
        DamageContainer damageContainer = collision.gameObject.GetComponent<DamageContainer>();
        if(damageContainer != null)
        {
            if (this.gameObject.GetInstanceID() < damageContainer.gameObject.GetInstanceID())
            {
                GameObject collisionEffect;
                collisionEffect = Instantiate(collisionEffectPrefab);
                collisionEffect.transform.position = this.transform.position;

                if (this.Priority < damageContainer.Priority)
                {
                    Destroy(gameObject);
                }else if (this.Priority > damageContainer.Priority)
                {
                    Destroy(damageContainer.gameObject);
                }
                else if (this.Priority == damageContainer.Priority){
                    Destroy(damageContainer.gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }
}
