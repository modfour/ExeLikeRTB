using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMaster : MonoBehaviour
{
    // Floorを格納する二次元配列
    private GameObject[,] battleField = new GameObject[(int)CommonConstants.fieldColumn+2,(int)CommonConstants.fieldRow+2];
    // マス目上に存在するゲームオブジェクトを格納する二次元配列
    private GameObject[,] battleObjects = new GameObject[(int)CommonConstants.fieldColumn + 2, (int)CommonConstants.fieldRow + 2];

    public GameObject floorPrefab;
    public Sprite floorSprite;

    public GameObject[,] BattleObjects
    {
        get { return this.battleObjects; }
    }
    public GameObject[,] BattleField
    {
        get { return this.battleField; }
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int column = 0; column < (int)CommonConstants.fieldColumn+2; column++)
        {
            for (int row = 0; row < (int)CommonConstants.fieldRow + 2; row++)
            {
                // floorPrefabからfloorのインスタンスを1枚分生成
                battleField[column,row] = Instantiate(floorPrefab);

                // 配置場所初期値セット
                Vector3 pos = battleField[column, row].transform.position;
                pos.x = (float)CommonConstants.positionZero.x + (float)CommonConstants.floorLength * column;
                pos.y = (float)CommonConstants.positionZero.y - (float)CommonConstants.floorWidth * row;
                pos.z = 1.0f;
                battleField[column, row].transform.position = pos;

                SpriteRenderer sr = battleField[column, row].GetComponent<SpriteRenderer>();
                Floor fl = battleField[column, row].GetComponent<Floor>();
                fl.Column = column;
                fl.Row = row;
                if (column > 0 && column < (int)CommonConstants.fieldColumn + 1 && row > 0 && row < (int)CommonConstants.fieldRow + 1)
                {
                    sr.sprite = floorSprite;
                    fl.IsMovable = true;
                }
                else
                {
                    sr.sprite = null;
                    fl.IsMovable = false;
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 指定した行列の移動可否を取得する
    public Boolean getIsMovable(int column,int row)
    {
        Floor fl = battleField[column, row].GetComponent<Floor>();
        return fl.IsMovable;
    }
}
