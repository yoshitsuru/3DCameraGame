using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using System.Threading.Tasks;
using UnityEngine.InputSystem.XR;

public class CameraChanger : MonoBehaviour
{
    // PlayerControllerスクリプト
    public PlayerController playerController;

    // UIControllerスクリプト
    public UIController uIController;

    // TimeManagerスクリプト
    public TimeManager timeManager;

    // SoundManagerスクリプト
    public SoundManager soundManager;

    // EffectControllerスクリプト
    public EffectController _effectController;

    // 一人称視点の参照
    [SerializeField]
    private CinemachineVirtualCamera _firstPersonCamera;


    // 三人称視点の参照
    [SerializeField]
    private CinemachineVirtualCamera _thirdPersonCamera;

    /// STAGE名
    [SerializeField]
    private string _stage;

    // プレイヤーのゲームオブジェクト
    [SerializeField]
    private GameObject _player;

    // 撮影判定true
    [SerializeField]
    private GameObject _trueImg;

    // 撮影判定false
    [SerializeField]
    private GameObject _falseImg;

    // itempickのイメージ
    public Sprite itemPickSprite;

    // itemthrowのイメージ
    public Sprite itemThrowSprite;

    // ScreenShotのイメージ
    public Sprite screenShotSprite;

    // Actionボタンのイメージ
    public Image actionImg;

    // 撮影用UIButton
    [SerializeField]
    private GameObject _actionUIButton;

    // 移動用UIButton
    [SerializeField]
    private GameObject _moveUIButton;

    // item(シーン上のアイテム)
    [SerializeField]
    private GameObject _itemObject;

    // item(投げるアイテム)
    [SerializeField]
    private GameObject _itemThrowObject;

    // 一人称フラグ
    public bool isFPS;

    // itempickフラグ
    public bool itemPickFlg = false;

    // itemthrowフラグ
    public bool itemThrowFlg = false;

    // 撮影判定フラグ
    public bool filmFlg = false;

    // rayCastフラグ
    [SerializeField]
    private bool _rayCastFlg = false;

    // 撮影回数テキスト
    public TextMeshProUGUI cameraShotText;

    // 撮影対象テキスト
    public TextMeshProUGUI targetText;

    // アイテムテキスト
    public TextMeshProUGUI itemText;

    // 撮影回数カウント
    public int cameraShotCount = 10;

    // 撮影対象カウント
    public int targetCount = 1;

    // 検出可能な距離
    public float distance = 7.0f;

    // Captureクラス(キャプチャデータ保存用)
    [System.Serializable]
    public class Capture{
        public string stageName { get; set; }
        public string capturePath { get; set; }
        public bool judge { get; set; }
    }
    
    // キャプチャリスト
    public List<Capture> cameraShotList =  new List<Capture>();
    
    // キャプチャリスト（データ保存用）
    public static List<Capture> saveCaptureList =  new List<Capture>();

    void Start()
    {
        // 最初は三人称から
        SetThirdPersonCamera();

        // 撮影判定イメージの設定
        _trueImg.SetActive(false);
        _falseImg.SetActive(true);

        // 開始時のテキストを設定
        cameraShotText.text = "残り撮影回数:" + cameraShotCount;
        targetText.text = "残り撮影対象:" + targetCount;
        itemText.text = "アイテム：なし";

        // ActionUIButtonの初期イメージ
        actionImg.sprite = screenShotSprite;
    }

    void Update()
    {
        // テキスト表示
        cameraShotText.text = "残り撮影回数:" + cameraShotCount;
        targetText.text = "残り撮影対象:" + targetCount;

        // アイテムを持っている場合
        if (itemThrowFlg)
        {
            itemText.text = "アイテム：あり";
        }
        else
        {
            itemText.text = "アイテム：なし";
        }

        // 一人称判定
        if(isFPS){

            //カメラ内外判定
            IsVisibleByCamera();
            
            // 撮影対象のレイキャスト当たり判定
            if(_rayCastFlg)
            {
                // 撮影判定を〇とする
                _trueImg.SetActive(true);
                _falseImg.SetActive(false);
            }
            else
            {
                // 撮影判定を×とする
                _trueImg.SetActive(false);
                _falseImg.SetActive(true);
            }
        }
        // ゲームオーバー判定(2秒待機)
        Invoke(nameof(GameOver), 2f);

    }

    /// <summary>
    /// 視点の切り替えを実行する
    /// </summary>
    [ContextMenu("SwitchCamera")]
    public void SwitchCamera()
    {
        if (isFPS)
        {
            // 三人称モード
            SetThirdPersonCamera();
        }
        else
        {
            // 一人称モード
            SetFirstPersonCamera();
        }
    }

    /// <summary>
    /// 一人称視点に切り替える
    /// </summary>
    private void SetFirstPersonCamera()
    {
        // カメラのPriorityを変更し一人称カメラ優先とする
        _firstPersonCamera.Priority = 10;
        _thirdPersonCamera.Priority = 0;

        // アクションボタン活性、移動ボタン非活性、一人称フラグ活性
        _actionUIButton.SetActive(true);
        _moveUIButton.SetActive(false);
        isFPS = true;
    }

    /// <summary>
    /// 三人称視点に切り替える
    /// </summary>
    private void SetThirdPersonCamera()
    {
        // カメラのPriorityを変更し三人称カメラ優先とする
        _firstPersonCamera.Priority = 0;
        _thirdPersonCamera.Priority = 10;

        // アクションボタン非活性、移動ボタン活性、一人称フラグ非活性
        _actionUIButton.SetActive(false);
        _moveUIButton.SetActive(true);
        isFPS = false;
    }


    /// <summary>
    /// アクション機能
    /// <summary>
    public void Action()
    {
        // itemPickFlgがtrue、itemThrowFlgがfalse
        if (itemPickFlg && !itemThrowFlg)
        {
            // アイテム収集を実行
            ItemPick();
        }
        // itemPickFlgがfalse、itemThrowFlgがtrue
        else if (!itemPickFlg && itemThrowFlg)
        {
            // アイテム投下を実行
            ItemThrow();
        }
        // それ以外
        else
        {
            // 撮影機能を実行
            ScreenShot();
        }
    }


    /// <summary>
    /// 撮影機能
    /// <summary>
    public void ScreenShot()
    {
        // シャッター音を鳴らす
        soundManager.SoundClip1();

        // 撮影回数カウントを増やす
        cameraShotCount--;
        //撮影エフェクトを表示
        _effectController.ShutterEffect();

        // カメラ撮影の判定
        if (_rayCastFlg)
        {
            // 撮影成功
            filmFlg = true;
            // 撮影対象カウントを減らす
            targetCount--;
        }
        else
        {
            // 撮影失敗
            filmFlg = false;
        }
        //Invorkeはキャプチャの保存とゲームクリア判定が正しく行われるための対策
        // スクリーンショットを保存する(1秒後)
        Invoke(nameof(SavePicture), 1f);
        // ゲームクリア判定(2秒後)
        Invoke(nameof(GameClear), 3f);
    }

    /// <summary>
    /// アイテム収集
    /// </summary>
    public void ItemPick()
    {
        // アイテム収集時の効果音
        soundManager.SoundClip2();

        // ボタンの画像差し替え
        actionImg.sprite = itemThrowSprite;
        
        // 収集アイテムを非表示
        _itemObject.gameObject.SetActive(false);
        
        // フラグの変更
        itemPickFlg = false;
        itemThrowFlg = true;
    }

    /// <summary>
    /// アイテム投下
    /// </summary>
    public async void ItemThrow()
    {
        // アイテム投下時の効果音
        soundManager.SoundClip3();

        // ボタンの画像差し替え
        actionImg.sprite = screenShotSprite;
        
        // 投下アイテムの生成
        GameObject item = Instantiate(_itemThrowObject,new Vector3(_player.transform.position.x,2, _player.transform.position.z),Quaternion.identity);
        
        // 投下時の設定
        item.GetComponent<Rigidbody>().AddForce(_player.transform.forward * 300);
        
        // フラグの変更
        itemPickFlg = false;
        itemThrowFlg = false;
        
        //1秒停止後、アイテム削除(撮影対象に当たらなかった場合の対処)
        await Task.Delay(1000);
        Destroy(item);
    }

    /// <summary>
    /// スクリーンショット保存
    /// <summary>
    private void SavePicture()
    {
        // ディレクトリ、キャプチャ等の名前を定義
        Capture c = new Capture();
        string date = System.DateTime.Now.ToString("yyyyMMddHHmmss");

        var directory = Application.persistentDataPath + "/" + _stage;
        var screenShotPicture = directory + "/" + _stage + "_"+ (10 - cameraShotCount) + "_" + date +  ".png";

        // ステージのディレクトリがない場合
        if (!System.IO.Directory.Exists(directory))
        {
            // ステージのディレクトリ作成とスクリーンショットを保存
            System.IO.Directory.CreateDirectory(directory);
            ScreenCapture.CaptureScreenshot(screenShotPicture);

        }
        // ステージのディレクトリがある場合
        else
        {
            // スクリーンショットを保存
            ScreenCapture.CaptureScreenshot(screenShotPicture);
        }

        // 撮影結果のリストにスクリーンショットを入れる(成功、失敗ともに)
        c.stageName = _stage;
        c.capturePath = screenShotPicture;
        if(filmFlg){
            c.judge = true;
        }
        else
        {
            c.judge = false;
        }
        cameraShotList.Add(c);
    }

    /// <summary>
    /// カメラ撮影判定(レイキャストによる対象物判定)
    /// </summary>
    public void IsVisibleByCamera() {
        // 一人称時点の場合に判定処理を行う
        // RayCastによるオブジェクト判定
        // Rayはカメラの位置からとばす
        var rayStartPosition = _firstPersonCamera.transform.position;
        // Rayはカメラが向いてる方向にとばす
        var rayDirection = _firstPersonCamera.transform.forward.normalized;

        // Hitしたオブジェクト格納用
        RaycastHit raycastHit;

        // Rayを飛ばす（out raycastHit でHitしたオブジェクトを取得する）
        var isHit = Physics.Raycast(rayStartPosition, rayDirection, out raycastHit, distance);

        // Debug.DrawRay (Vector3 start(rayを開始する位置), Vector3 dir(rayの方向と長さ), Color color(ラインの色));
        Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);

        // レイキャストで何かを確認かつそれが撮影対象であればレイキャストフラグを立てる
        if (isHit && raycastHit.collider.gameObject.CompareTag("Target"))
        {
            _rayCastFlg = true;
        }
        else
        {
            _rayCastFlg = false;
        }

    }

    /// <summary>
    /// ゲームクリア
    /// </summary>
    public void GameClear(){
        // 撮影対象カウントが0になったらゲームクリア
        if(targetCount == 0)
        {
            // 撮影したキャプチャリストを展開
            foreach (var cameraShot in cameraShotList){
                // 成功したキャプチャか判定する
                if(cameraShot.judge)
                {
                    // 保存キャプチャリストに保存
                    saveCaptureList.Add(cameraShot);
                }
                else
                {
                    // 撮影失敗したキャプチャを削除する
                    System.IO.File.Delete(cameraShot.capturePath);
                }
            }
            // 撮影したキャプチャを確認したらゲームクリア
            uIController.ActiveGameClear();
        }
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void GameOver(){
        // 撮影回数が0または制限時間が0になったらゲームオーバー
        if(cameraShotCount == 0 || !timeManager.timerIsRunning)
        {
            uIController.ActiveGameOver();
        }
    }
}
