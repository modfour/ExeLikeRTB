using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // playerを操作するためのコンポーネント
    private PlayerController playerController;
    private EnemyController enemyController;

    // floorを処理する場合はFloorMasterを経由する
    private FloorMaster floorMaster;

    // MessageWindow
    public GameObject messageWindow;

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
        messageWindow = GameObject.Find("/Canvas/MessageWindow");
        messageWindow.SetActive(false);
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
    void DamageProcess(Character character)
    {
        GameObject characterFloor = floorMaster.BattleField[character.Column, character.Row];
        List<DamageContainer> damageContainers = characterFloor.GetComponent<Floor>().DamageContainers;

        for (int index = damageContainers.Count - 1; index >= 0; index--)
        {
            int damage = damageContainers[index].GetDamage(character.IsPlayer);
            if (damage != 0)
            {
                character.HitPoint -= damage;
                character.Animator.SetInteger("MotionIndex", (int)CommonConstants.MotionIndex.damage);
                if (damageContainers[index].HitOnceOnly)
                {
                    DamageContainer damageContainer = damageContainers[index];
                    damageContainers.Remove(damageContainers[index]);
                    Destroy(damageContainer.gameObject);
                }
                if (character.HitPoint < 1)
                {
                    character.gameObject.SetActive(false);
                    messageWindow.SetActive(true);
                }
            }
        }
    }
}
