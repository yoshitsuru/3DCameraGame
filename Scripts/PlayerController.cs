using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // CameraChanger
    public CameraChanger cameraChanger;

    // �J�����̌���
    public Transform cameraTransform;

    void Update()
    {

        // ���_�ړ��ɍ��킹�ăv���C���[�̌��������킹��
        // �J�����̕������擾
        if (cameraChanger.isFPS)
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0; // ���������̉�]�݂̂��l��

            // �J�����̕����Ɋ�Â��ăv���C���[�̉�]��ݒ�
            if (cameraForward != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���W�A�C�e���Ɠ���������itempickflg��true�ɂ���Action�{�^���̉摜(�A�C�e�����W)�������ւ���
        if (other.gameObject.CompareTag("pickItem"))
        {
            cameraChanger.itemPickFlg = true;
            cameraChanger.actionImg.sprite = cameraChanger.itemPickSprite;
        }
    }
 
    private void OnTriggerExit(Collider other)
    {
        // ���W�A�C�e���𓊂�����itempickflg��false�ɂ���Action�{�^���̉摜(�B�e)�������ւ���
        if (other.gameObject.CompareTag("pickItem"))
        {
            cameraChanger.itemPickFlg = false;
            cameraChanger.actionImg.sprite = cameraChanger.screenShotSprite;
        }
    }
}
