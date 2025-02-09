using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{

    // 撮影エフェクトイメージ
    private Image img;

    // 撮影エフェクトフラグを取得するためCameraChangeスクリプトを取得
    public CameraChanger cameraChanger;

    // Start is called before the first frame update
    void Start()
    {
        // 撮影時のエフェクト設定
        img = GetComponent<Image>();
        img.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        // 撮影された場合、エフェクトを発生させる
        if (cameraChanger.shotEffect)
        {
            img.color = new Color(1, 1, 1, 1);
        }
        else
        {
            // エフェクト発生後、少しずつ色を元に戻す。
             img.color = Color.Lerp(img.color, Color.clear, Time.deltaTime);
        }
    }
}
