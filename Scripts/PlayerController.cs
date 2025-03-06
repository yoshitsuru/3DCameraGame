using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // CameraChanger
    public CameraChanger cameraChanger;

    // カメラの向き
    public Transform cameraTransform;

    void Update()
    {

        // 視点移動に合わせてプレイヤーの向きも合わせる
        // カメラの方向を取得
        if (cameraChanger.isFPS)
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0; // 水平方向の回転のみを考慮

            // カメラの方向に基づいてプレイヤーの回転を設定
            if (cameraForward != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 収集アイテムと当たったらitempickflgをtrueにしてActionボタンの画像(アイテム収集)を差し替える
        if (other.gameObject.CompareTag("pickItem"))
        {
            cameraChanger.itemPickFlg = true;
            cameraChanger.actionImg.sprite = cameraChanger.itemPickSprite;
        }
    }
 
    private void OnTriggerExit(Collider other)
    {
        // 収集アイテムを投げたらitempickflgをfalseにしてActionボタンの画像(撮影)を差し替える
        if (other.gameObject.CompareTag("pickItem"))
        {
            cameraChanger.itemPickFlg = false;
            cameraChanger.actionImg.sprite = cameraChanger.screenShotSprite;
        }
    }
}
