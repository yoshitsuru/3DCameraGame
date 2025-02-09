using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // 自動移動時の経路のポイント
    public Transform[] wayPoints;
    
    // 現在のウェイポイント
    private int currentPoint = 0;
    
    // エージェントの位置
    private NavMeshAgent agent;

    // プレイヤーの位置
    public Transform player;

    //ターゲット(撮影対象)
    private SphereCollider targetCollider;

    // オブジェクトの移動速度を格納する変数
    public float moveSpeed;

    // オブジェクトがターゲットに向かって移動を開始する距離を格納する変数
    public float moveDistance;

    // オブジェクトがターゲットに向かっての移動を止める距離を格納する変数
    public float stopDistance;

    // PlayerController
    public PlayerController playerController;

    private Animator animator;

    void Start()
    {

        // エージェントの取得
        agent = GetComponent<NavMeshAgent>();
        // 撮影対象のコライダーの取得
        targetCollider = GameObject.FindGameObjectWithTag("Target").GetComponent<SphereCollider>();

        // 最初のエージェント位置へ移動
        GoToNextPoint();
    }

    void Update()
    {
        // プレイヤーの位置を取得
        Vector3 playerPosition = player.position;

        // 変数 distance を作成してオブジェクトの位置とターゲットオブジェクトの距離を格納
        float distance = Vector3.Distance(transform.position, playerPosition);

        //GetComponentを用いてAnimatorコンポーネントを取り出す.
        animator = this.GetComponent<Animator>();

        //あらかじめ設定していたintパラメーター「trans」の値を取り出す.
        bool dead = animator.GetBool("Dead");

        // にんにくを持っている場合、動きが止まる
        if (playerController._itemFlag)
        {
            // エージェント停止
            agent.isStopped = true;
            // deadのアニメーションを流す
            dead = true;
            animator.SetBool("Dead", dead);

            // Colliderの設定変更(撮影範囲とオブジェクトの滑り防止)
            targetCollider.radius = 3.0f;
            targetCollider.isTrigger = true;
        }
        else
        {

            // オブジェクトがターゲットに向かって移動を開始する距離内の場合プレイヤーを追尾する
            if (distance < moveDistance)
            {
                // 撮影対象がプレイヤーを追尾する
                agent.SetDestination(playerPosition);
            }
            else
            {
                // 距離外の場合は自動で移動
                // エージェント位置を次々に移動していく
                if (!agent.pathPending && agent.remainingDistance < 0.1f)
                {
                    GoToNextPoint();
                }
            }
        }
    }

    void GoToNextPoint()
    {
        // wayPoints配列内にウェイポイントがないかどうか
        if (wayPoints.Length == 0)
            // ない場合はリターン
            return;

        // 現在のウェイポイント（wayPoints[currentPoint]）の位置を設定
        agent.SetDestination(wayPoints[currentPoint].position);
        // 次にエージェントが向かうウェイポイントのインデックスを保持
        currentPoint = (currentPoint + 1) % wayPoints.Length;

    }

}
