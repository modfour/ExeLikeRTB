using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonConstants
{
    // フィールドの縦・横
    public static int fieldColumn = 8;
    public static int fieldRow = 5;

    // フロア1枚あたりの縦・横
    public static float floorLength = 1.83f;
    public static float floorWidth = 0.88f;

    // 左上をゼロとした時の起点
    public static Vector3 positionZero = new Vector3(
        -1 * (float)CommonConstants.floorLength * ((int)CommonConstants.fieldColumn + 1) / 2,
        (float)CommonConstants.floorWidth,
        0.0f
    );


    // キャラクター画像の向きを定義する定数値
    public static readonly Vector3 rightward = new Vector3(-2.0f, 2.0f, 1.0f);
    public static readonly Vector3 leftward = new Vector3(2.0f, 2.0f, 1.0f);

    // キャラクターの初期HPを定義する定数値
    public static int initHitPoint = 150;

    // キャラクターのモーションの種類を定義する列挙型
    public enum MotionIndex: int
    {
        idle        = 0,
        frontstep   = 1,
        backstep    = -1,
        attack      = 10,
        attack2     = 20,
        charge      = 30,
        damage      = 40,
        guard       = 50,
        victory     = 60
    }

    public enum Direction : int
    {
        left    = 1,
        right   = 2,
        up      = 3,
        down    = 4
    }
}
