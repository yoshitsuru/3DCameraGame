using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // UI�R���g���[��(�Q�[���I�[�o�[�p)
    public UIController _uiController = default;

    // �A�C�e���t���O
    public bool _itemFlag = false;

    void Start()
    {
        _itemFlag = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Target�Ɠ���������Q�[���I�[�o�[
        if (collision.gameObject.CompareTag("Target") && _itemFlag == false)
        {
            _uiController.ActiveGameOver();
        }

        // item�Ɠ���������item���폜���Aitem�t���O��True�ɂ���
        if (collision.gameObject.CompareTag("Item"))
        {
            GetComponent<AudioSource>().Play();
            collision.gameObject.SetActive(false);
            _itemFlag = true;
        }
    }
}
