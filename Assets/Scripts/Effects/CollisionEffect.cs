using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
    private float deleteTime = 0.1f; // 弾を消すまでの秒数
    private float time = 0.0f; // 生成してからの経過時間

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        // 消滅処理
        if (time > deleteTime)
        {
            Destroy(gameObject);
        }
    }
}
