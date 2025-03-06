using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // UIController(�Q�[���I�[�o�[�p)
    public UIController uiController = default;

    // SoundManager
    public SoundManager soundManager;

    // �����ړ����̌o�H�̃|�C���g
    public Transform[] wayPoints;
    
    // ���݂̃E�F�C�|�C���g
    private int _currentPoint = 0;
    
    // �G�[�W�F���g�̈ʒu
    private NavMeshAgent _agent;

    // �v���C���[�̈ʒu
    public Transform player;

    //�^�[�Q�b�g(�B�e�Ώ�)
    private SphereCollider _targetCollider;

    // �I�u�W�F�N�g�̈ړ����x���i�[����ϐ�
    public float moveSpeed;

    // �I�u�W�F�N�g���^�[�Q�b�g�Ɍ������Ĉړ����J�n���鋗�����i�[����ϐ�
    public float moveDistance;

    // �B�e�Ώۂ̓���
    private Animator _animator;

    void Start()
    {
        // �G�[�W�F���g�̎擾
        _agent = GetComponent<NavMeshAgent>();
        // �B�e�Ώۂ̃R���C�_�[�̎擾
        _targetCollider = GameObject.FindGameObjectWithTag("Target").GetComponent<SphereCollider>();

        // �ŏ��̃G�[�W�F���g�ʒu�ֈړ�
        GoToNextPoint();
    }

    void Update()
    {
        // �v���C���[�̈ʒu���擾
        Vector3 playerPosition = player.position;

        // �ϐ� distance ���쐬���ăI�u�W�F�N�g�̈ʒu�ƃ^�[�Q�b�g�I�u�W�F�N�g�̋������i�[
        float distance = Vector3.Distance(transform.position, playerPosition);

        // �I�u�W�F�N�g���^�[�Q�b�g�Ɍ������Ĉړ����J�n���鋗�����̏ꍇ�v���C���[��ǔ�����
        if (distance < moveDistance)
        {
            // �B�e�Ώۂ��v���C���[��ǔ�����
            _agent.SetDestination(playerPosition);
        }
        else if (distance >= moveDistance) 
        {
            // �����O�̏ꍇ�͎����ňړ�
            // �G�[�W�F���g�ʒu�����X�Ɉړ����Ă���
            if (!_agent.pathPending && _agent.remainingDistance < 0.1f)
            {
                GoToNextPoint();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Target�Ɠ���������Q�[���I�[�o�[
        if (collision.gameObject.CompareTag("Player"))
        {
            // ���ʉ����o��
            soundManager.SoundClip6();
            uiController.ActiveGameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // item�Ɠ���������|���
        if (other.gameObject.CompareTag("Item"))
        {

            soundManager._isTargetDeathFlg = true;

            // BGM��߂�
            soundManager.SoundBGM();

            // ���ʉ����o��
            soundManager.SoundClip4();
            soundManager.SoundClip5();

            // ���������A�C�e���͍폜
            Destroy(other.gameObject);

            // GetComponent��p����Animator�R���|�[�l���g�����o��.
            _animator = this.GetComponent<Animator>();

            // ���炩���ߐݒ肵�Ă���int�p�����[�^�[�utrans�v�̒l�����o��.
            bool dead = _animator.GetBool("Dead");

            // �G�[�W�F���g��~
            _agent.isStopped = true;
            // dead�̃A�j���[�V�����𗬂�
            dead = true;
            _animator.SetBool("Dead", dead);

            // Collider�̐ݒ�ύX(�B�e�͈͂ƃI�u�W�F�N�g�̊���h�~)
            _targetCollider.radius = 3.0f;
            _targetCollider.isTrigger = true;
        }
    }

    void GoToNextPoint()
    {
        // wayPoints�z����ɃE�F�C�|�C���g���Ȃ����ǂ���
        if (wayPoints.Length == 0)
            // �Ȃ��ꍇ�̓��^�[��
            return;

        // ���݂̃E�F�C�|�C���g�iwayPoints[currentPoint]�j�̈ʒu��ݒ�
        _agent.SetDestination(wayPoints[_currentPoint].position);
        // ���ɃG�[�W�F���g���������E�F�C�|�C���g�̃C���f�b�N�X��ێ�
        _currentPoint = (_currentPoint + 1) % wayPoints.Length;

    }

}
