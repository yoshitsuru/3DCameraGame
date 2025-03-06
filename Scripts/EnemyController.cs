using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // UIController(ゲームオーバー用)
    public UIController uiController = default;

    // SoundManager
    public SoundManager soundManager;

    // 自動移動時の経路のポイント
    public Transform[] wayPoints;
    
    // 現在のウェイポイント
    private int _currentPoint = 0;
    
    // エージェントの位置
    private NavMeshAgent _agent;

    // プレイヤーの位置
    public Transform player;

    //ターゲット(撮影対象)
    private SphereCollider _targetCollider;

    // オブジェクトの移動速度を格納する変数
    public float moveSpeed;

    // オブジェクトがターゲットに向かって移動を開始する距離を格納する変数
    public float moveDistance;

    // 撮影対象の動作
    private Animator _animator;

    void Start()
    {
        // エージェントの取得
        _agent = GetComponent<NavMeshAgent>();
        // 撮影対象のコライダーの取得
        _targetCollider = GameObject.FindGameObjectWithTag("Target").GetComponent<SphereCollider>();

        // 最初のエージェント位置へ移動
        GoToNextPoint();
    }

    void Update()
    {
        // プレイヤーの位置を取得
        Vector3 playerPosition = player.position;

        // 変数 distance を作成してオブジェクトの位置とターゲットオブジェクトの距離を格納
        float distance = Vector3.Distance(transform.position, playerPosition);

        // オブジェクトがターゲットに向かって移動を開始する距離内の場合プレイヤーを追尾する
        if (distance < moveDistance)
        {
            // 撮影対象がプレイヤーを追尾する
            _agent.SetDestination(playerPosition);
        }
        else if (distance >= moveDistance) 
        {
            // 距離外の場合は自動で移動
            // エージェント位置を次々に移動していく
            if (!_agent.pathPending && _agent.remainingDistance < 0.1f)
            {
                GoToNextPoint();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Targetと当たったらゲームオーバー
        if (collision.gameObject.CompareTag("Player"))
        {
            // 効果音を出す
            soundManager.SoundClip6();
            uiController.ActiveGameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // itemと当たったら倒れる
        if (other.gameObject.CompareTag("Item"))
        {

            soundManager._isTargetDeathFlg = true;

            // BGMを戻す
            soundManager.SoundBGM();

            // 効果音を出す
            soundManager.SoundClip4();
            soundManager.SoundClip5();

            // 当たったアイテムは削除
            Destroy(other.gameObject);

            // GetComponentを用いてAnimatorコンポーネントを取り出す.
            _animator = this.GetComponent<Animator>();

            // あらかじめ設定していたintパラメーター「trans」の値を取り出す.
            bool dead = _animator.GetBool("Dead");

            // エージェント停止
            _agent.isStopped = true;
            // deadのアニメーションを流す
            dead = true;
            _animator.SetBool("Dead", dead);

            // Colliderの設定変更(撮影範囲とオブジェクトの滑り防止)
            _targetCollider.radius = 3.0f;
            _targetCollider.isTrigger = true;
        }
    }

    void GoToNextPoint()
    {
        // wayPoints配列内にウェイポイントがないかどうか
        if (wayPoints.Length == 0)
            // ない場合はリターン
            return;

        // 現在のウェイポイント（wayPoints[currentPoint]）の位置を設定
        _agent.SetDestination(wayPoints[_currentPoint].position);
        // 次にエージェントが向かうウェイポイントのインデックスを保持
        _currentPoint = (_currentPoint + 1) % wayPoints.Length;

    }

}
