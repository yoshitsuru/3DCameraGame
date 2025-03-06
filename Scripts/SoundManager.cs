using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // �v���C���[�̈ʒu
    public Transform player;

    // �^�[�Q�b�g�̈ʒu
    public Transform target;
    
    // ���m�͈�
    public float detectionRange;

    // BGM2�t���O
    private bool _isBGM2Playing = false;

    // �^�[�Q�b�g���S�t���O
    public bool _isTargetDeathFlg = false;


    //BGM1
    private GameObject _bgm;
    
    //BGM2
    private GameObject _bgm2;
    
    // AudioSource�̕ϐ�
    private AudioSource _audioSource = null;

    // ���ʉ�1
    public AudioClip sound01;
   
    // ���ʉ�2
    public AudioClip sound02;
    
    // ���ʉ�3
    public AudioClip sound03;
    
    // ���ʉ�4
    public AudioClip sound04;
    
    // ���ʉ�5
    public AudioClip sound05;
    
    // ���ʉ�6
    public AudioClip sound06;

    void Start()
    {
        // BGM���Q�[���I�u�W�F�N�g����擾
        _bgm = GameObject.Find("BGM");
        _bgm2 = GameObject.Find("BGM2");

        // �X�^�[�g����BGM�𗬂�
        _audioSource = _bgm.GetComponent<AudioSource>();
        _audioSource.Play();

    }

    void Update()
    {
        // �^�[�Q�b�g���S�t���O�������Ă���ꍇ��BGM1
        if (_isTargetDeathFlg)
        {
            return;
        }

        // �v���C���[�ƎB�e�Ώۂ̋������擾
        float distance = Vector3.Distance(player.position, target.position);

        // detectionRange�͏������N�������m�͈�
        if (distance < detectionRange && !_isBGM2Playing)
        {
            // ���m�͈͓��̏ꍇ�ABGM2�𗬂�
            SoundBGM2();
        }
        else if (distance >= detectionRange && _isBGM2Playing)
        {
            // ���m�͈͊O�̏ꍇ�ABGM�𗬂�
            SoundBGM();
        }
    }

    public void SoundBGM()
    {
        _audioSource = _bgm2.GetComponent<AudioSource>();
        _audioSource.Stop();
        _audioSource = _bgm.GetComponent<AudioSource>();
        _audioSource.Play();
        _isBGM2Playing = false;
    }

    public void SoundBGM2()
    {
        _audioSource = _bgm.GetComponent<AudioSource>();
        _audioSource.Stop();
        _audioSource = _bgm2.GetComponent<AudioSource>();
        _audioSource.Play();
        _isBGM2Playing = true;
    }

    public void SoundClip1()
    {
        _audioSource.PlayOneShot(sound01);
    }

    public void SoundClip2()
    {
        _audioSource.PlayOneShot(sound02);
    }

    public void SoundClip3()
    {
        _audioSource.PlayOneShot(sound03);
    }

    public void SoundClip4()
    {
        _audioSource.PlayOneShot(sound04);
    }

    public void SoundClip5()
    {
        _audioSource.PlayOneShot(sound05);
    }

    public void SoundClip6()
    {
        _audioSource.PlayOneShot(sound06);
    }

}
