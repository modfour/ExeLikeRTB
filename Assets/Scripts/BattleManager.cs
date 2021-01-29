﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // playerを操作するためのコンポーネント
    private PlayerController playerController;
    private EnemyController enemyController;

    // floorを処理する場合はFloorMasterを経由する
    private FloorMaster floorMaster;

    public FloorMaster FloorMaster
    {
        get { return this.floorMaster; }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        enemyController = GameObject.Find("Enemy").GetComponent<EnemyController>();
        floorMaster = GameObject.Find("Floor").GetComponent<FloorMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        // 自キャラと敵キャラの位置に応じて左右反転させる
        SetChallactersDirection();

        // ダメージ処理
        DamageProcess(playerController);
        DamageProcess(enemyController);
    }

    // 敵と自分の位置に応じて左右反転させる
    void SetChallactersDirection()
    {
        if (playerController.Column <= enemyController.Column)
        {
            playerController.SetDirection(CommonConstants.rightward);
            enemyController.SetDirection(CommonConstants.leftward);
        }
        else
        {
            playerController.SetDirection(CommonConstants.leftward);
            enemyController.SetDirection(CommonConstants.rightward);
        }
    }

    // ダメージを与える処理
    void DamageProcess(CharacterController characterController)
    {
        GameObject characterFloor = floorMaster.BattleField[characterController.Column, characterController.Row];
        List<DamageContainer> damageContainers = characterFloor.GetComponent<Floor>().DamageContainers;

        for (int index = damageContainers.Count - 1; index >= 0; index--)
        {
            int damage = damageContainers[index].GetDamage(characterController.IsPlayer);
            if (damage != 0)
            {
                characterController.HitPoint -= damage;
                characterController.Animator.SetInteger("MotionIndex", (int)CommonConstants.MotionIndex.damage);
                if (damageContainers[index].HitOnceOnly)
                {
                    DamageContainer damageContainer = damageContainers[index];
                    damageContainers.Remove(damageContainers[index]);
                    Destroy(damageContainer.gameObject);
                }
                if (characterController.HitPoint < 1)
                {
                    characterController.gameObject.SetActive(false);
                }
            }
        }
    }
}
