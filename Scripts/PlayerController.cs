using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // UIコントローラ(ゲームオーバー用)
    public UIController _uiController = default;

    // アイテムフラグ
    public bool _itemFlag = false;

    void Start()
    {
        _itemFlag = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Targetと当たったらゲームオーバー
        if (collision.gameObject.CompareTag("Target") && _itemFlag == false)
        {
            _uiController.ActiveGameOver();
        }

        // itemと当たったらitemを削除し、itemフラグをTrueにする
        if (collision.gameObject.CompareTag("Item"))
        {
            GetComponent<AudioSource>().Play();
            collision.gameObject.SetActive(false);
            _itemFlag = true;
        }
    }
}
