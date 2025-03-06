using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    /// 現在アクティブなシーン
    public string sceneName;

    // ポーズ画面
    public GameObject pauseCanvas;

    // ゲームオーバー画面
    public GameObject gameOverCanvas;

    // ゲームクリア画面
    public GameObject gameClearCanvas;

    //ポーズフラグ
    public bool pauseFlg;

    // エフェクトパネル(撮影時のフラッシュ)
    public GameObject effectPanel;

    void Start(){
        /// アクティブシーンを取得
        sceneName = SceneManager.GetActiveScene ().name;
        Time.timeScale = 1.0f;
        pauseFlg = false;
    }

    public void OnClickRetryButton(){
	    SceneManager.LoadScene (sceneName);
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Camera3DGameScene");
    }

    public void OnClickEndButton()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void OnClickPauseButton()
    {
        if(!pauseFlg){
            pauseCanvas.SetActive(true);
            Time.timeScale = 0.0f;
            pauseFlg =true;
        }
        else{
            pauseCanvas.SetActive(false);
            Time.timeScale = 1.0f;
            pauseFlg =false;
        }
    }
    public void ActiveGameOver()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void ActiveGameClear()
    {
        gameClearCanvas.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
