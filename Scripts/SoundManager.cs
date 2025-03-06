using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // プレイヤーの位置
    public Transform player;

    // ターゲットの位置
    public Transform target;
    
    // 検知範囲
    public float detectionRange;

    // BGM2フラグ
    private bool _isBGM2Playing = false;

    // ターゲット死亡フラグ
    public bool _isTargetDeathFlg = false;


    //BGM1
    private GameObject _bgm;
    
    //BGM2
    private GameObject _bgm2;
    
    // AudioSourceの変数
    private AudioSource _audioSource = null;

    // 効果音1
    public AudioClip sound01;
   
    // 効果音2
    public AudioClip sound02;
    
    // 効果音3
    public AudioClip sound03;
    
    // 効果音4
    public AudioClip sound04;
    
    // 効果音5
    public AudioClip sound05;
    
    // 効果音6
    public AudioClip sound06;

    void Start()
    {
        // BGMをゲームオブジェクトから取得
        _bgm = GameObject.Find("BGM");
        _bgm2 = GameObject.Find("BGM2");

        // スタート時のBGMを流す
        _audioSource = _bgm.GetComponent<AudioSource>();
        _audioSource.Play();

    }

    void Update()
    {
        // ターゲット死亡フラグが立っている場合はBGM1
        if (_isTargetDeathFlg)
        {
            return;
        }

        // プレイヤーと撮影対象の距離を取得
        float distance = Vector3.Distance(player.position, target.position);

        // detectionRangeは処理を起こす検知範囲
        if (distance < detectionRange && !_isBGM2Playing)
        {
            // 検知範囲内の場合、BGM2を流す
            SoundBGM2();
        }
        else if (distance >= detectionRange && _isBGM2Playing)
        {
            // 検知範囲外の場合、BGMを流す
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
