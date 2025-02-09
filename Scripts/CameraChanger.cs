using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.IO;
using TMPro;
using Unity.VisualScripting;

public class CameraChanger : MonoBehaviour
{

    // UIController
    public UIController uIController;
    // TimerScript
    public TimerScript timerScript;
    // PlayerController
    public PlayerController playerController;

    /// <summary>
    /// 一人称視点の参照
    /// </summary>
    [SerializeField]
    private CinemachineVirtualCamera firstPersonCamera;

    /// <summary>
    /// 三人称視点の参照
    /// </summary>
    [SerializeField]
    private CinemachineVirtualCamera thirdPersonCamera;

    /// STAGE名
    [SerializeField]
    private string stage;

    // プレイヤーのゲームオブジェクト
    [SerializeField]
    private GameObject player;

    // 撮影判定true
    [SerializeField]
    private GameObject trueImg;

    // 撮影判定false
    [SerializeField]
    private GameObject falseImg;

    // 撮影用UIButton
    [SerializeField]
    private GameObject CameraShotUIButton;

    // 一人称フラグ
    [SerializeField]
    private bool isFPS;

    // 撮影判定フラグ
    public bool filmflg = false;

    // 撮影判定フラグ
    public bool shotEffect = false;

    // pauseフラグ
    [SerializeField]
    private bool pauseFlag = false;

    // rayCastフラグ
    [SerializeField]
    private bool rayCastFlag = false;

    // ワールド座標フラグ
    [SerializeField]
    private bool isVisibleByCameraFlag = false;

    // 撮影回数テキスト
    public TextMeshProUGUI cameraShotText;

    // 撮影回数テキスト
    public TextMeshProUGUI targetText;

    // アイテムテキスト
    public TextMeshProUGUI itemText;

    // 撮影回数カウント
    public int cameraShotCount = 10;

    // 撮影対象カウント
    public int targetCount = 1;

    // 検出可能な距離
    public float distance = 6.0f;

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
        trueImg.SetActive(false);
        falseImg.SetActive(true);
        // 開始時のテキストを設定
        cameraShotText.text = "残り撮影回数:" + cameraShotCount;
        targetText.text = "残り撮影対象:" + targetCount;
        itemText.text = "にんにく：なし";
    }

    void Update()
    {
        // テキスト表示
        cameraShotText.text = "残り撮影回数:" + cameraShotCount;
        targetText.text = "残り撮影対象:" + targetCount;

        // アイテムフラグがtrueの場合
        if (playerController._itemFlag)
        {
            itemText.text = "にんにく：あり";
        }
        
        // ポーズ判定時は以降の処理を行わない
        if(pauseFlag)
        {
            return;
        }

        // 撮影を行わない限りはOFF
        shotEffect = false;

        if(isFPS){
            //カメラ内外判定
            IsVisibleByCamera();
            // フラグによるオブジェクト判定
            if(isVisibleByCameraFlag && rayCastFlag)
            {
                trueImg.SetActive(true);
                falseImg.SetActive(false);
            }
            else
            {
                trueImg.SetActive(false);
                falseImg.SetActive(true);
            }
        }
        // ゲームオーバー判定
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
            // 三人称
            SetThirdPersonCamera();
        }
        else
        {
            // 一人称
            SetFirstPersonCamera();
        }
    }

    /// <summary>
    /// 一人称視点に切り替える
    /// </summary>
    private void SetFirstPersonCamera()
    {
        firstPersonCamera.Priority = 10;
        thirdPersonCamera.Priority = 0;
        // カメラ撮影ボタンを活性
        CameraShotUIButton.SetActive(true);
        isFPS = true;
    }

    /// <summary>
    /// 三人称視点に切り替える
    /// </summary>
    private void SetThirdPersonCamera()
    {
        firstPersonCamera.Priority = 0;
        thirdPersonCamera.Priority = 10;
        // カメラ撮影ボタンを活性
        CameraShotUIButton.SetActive(false);
        isFPS = false;
    }

    /// <summary>
    /// 撮影機能
    /// <summary>
    public void ScreenShot()
    {
        // シャッター音を鳴らす
        GetComponent<AudioSource>().Play();

        // 撮影回数カウントを増やす
        cameraShotCount--;
        //撮影エフェクトを表示
        shotEffect = true;
        // カメラ撮影の判定
        if(isVisibleByCameraFlag && rayCastFlag)
        {
            // 撮影成功
            filmflg = true;
            // 撮影対象カウントを減らす
            targetCount--;
        }
        else
        {
            // 撮影失敗
            filmflg = false;
        }
        //Invorkeはキャプチャの保存とゲームクリア判定が正しく行われるための対策
        // スクリーンショットを保存する(1秒後)
        Invoke(nameof(SavePicture), 1f);
        // ゲームクリア判定(2秒後)
        Invoke(nameof(GameClear), 3f);
    }

    /// <summary>
    /// スクリーンショット保存
    /// <summary>
    private void SavePicture()
    {
        Capture c = new Capture();
        string date = System.DateTime.Now.ToString("yyyyMMddHHmmss");

        var directory = Application.persistentDataPath + "/" + stage;
        var screenShotPicture = directory + "/" + stage + "_"+ (10 - cameraShotCount) + "_" + date +  ".png";
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

        //撮影結果のリストにスクリーンショットを入れる(成功、失敗ともに)
        c.stageName = stage;
        c.capturePath = screenShotPicture;
        if(filmflg){
            c.judge = true;
        }
        else
        {
            c.judge = false;
        }
        cameraShotList.Add(c);
    }

    /// カメラ内外撮影判定
    public void IsVisibleByCamera() {
        // 一人称時点の場合に判定処理を行う
        // RayCastによるオブジェクト判定
        // Rayはカメラの位置からとばす
        var rayStartPosition = firstPersonCamera.transform.position;
        // Rayはカメラが向いてる方向にとばす
        var rayDirection = firstPersonCamera.transform.forward.normalized;

        // Hitしたオブジェクト格納用
        RaycastHit raycastHit;

        // Rayを飛ばす（out raycastHit でHitしたオブジェクトを取得する）
        var isHit = Physics.Raycast(rayStartPosition, rayDirection, out raycastHit, distance);

        // Debug.DrawRay (Vector3 start(rayを開始する位置), Vector3 dir(rayの方向と長さ), Color color(ラインの色));
        Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);

        // なにかを検出したら
        if (isHit)
        {
            rayCastFlag = true;
        }
        else
        {
            rayCastFlag = false;
        }
        // 検出したオブジェクトが対象のオブジェクトの場合、処理を行う
        if(rayCastFlag)
        {
            if(raycastHit.collider.gameObject.CompareTag("Target")){
                // ワールド座標によるカメラ内外判定
                // プレイヤーの位置
                Vector3 playerPos = player.transform.position;

                // 撮影対象の位置(Raycastで見つけたオブジェクト)
                Vector3 targetPos = raycastHit.collider.gameObject.transform.position;
                // 撮影対象とプレイヤーの距離を取得
                float dis = Vector3.Distance(targetPos, playerPos);

                // 撮影対象のワールド座標を取得
                Vector3 viewPos = Camera.main.WorldToViewportPoint(targetPos);

                // viewPosのx座標とy座標が0以上1以下かつzが0以上だったらみえる
                if (viewPos.x >= 0 && viewPos.x <=1 &&
                    viewPos.y >= 0 && viewPos.y <=1 && viewPos.z >=0) {
                    if (dis <= distance){
                        isVisibleByCameraFlag = true;
                    }
                    else {
                        isVisibleByCameraFlag = false;
                    }
                }
            }
            else
            {
                isVisibleByCameraFlag = false;
            }
        }
        else
        {
            isVisibleByCameraFlag = false;
        }
    }

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
            uIController.ActiveGameClear();
        }
    }

    public void GameOver(){
        // 撮影回数が0または制限時間が0になったらゲームオーバー
        if(cameraShotCount == 0 || !timerScript.timerIsRunning)
        {
            uIController.ActiveGameOver();
        }
    }
}
