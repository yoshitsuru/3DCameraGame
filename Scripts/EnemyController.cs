using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // �����ړ����̌o�H�̃|�C���g
    public Transform[] wayPoints;
    
    // ���݂̃E�F�C�|�C���g
    private int currentPoint = 0;
    
    // �G�[�W�F���g�̈ʒu
    private NavMeshAgent agent;

    // �v���C���[�̈ʒu
    public Transform player;

    //�^�[�Q�b�g(�B�e�Ώ�)
    private SphereCollider targetCollider;

    // �I�u�W�F�N�g�̈ړ����x���i�[����ϐ�
    public float moveSpeed;

    // �I�u�W�F�N�g���^�[�Q�b�g�Ɍ������Ĉړ����J�n���鋗�����i�[����ϐ�
    public float moveDistance;

    // �I�u�W�F�N�g���^�[�Q�b�g�Ɍ������Ă̈ړ����~�߂鋗�����i�[����ϐ�
    public float stopDistance;

    // PlayerController
    public PlayerController playerController;

    private Animator animator;

    void Start()
    {

        // �G�[�W�F���g�̎擾
        agent = GetComponent<NavMeshAgent>();
        // �B�e�Ώۂ̃R���C�_�[�̎擾
        targetCollider = GameObject.FindGameObjectWithTag("Target").GetComponent<SphereCollider>();

        // �ŏ��̃G�[�W�F���g�ʒu�ֈړ�
        GoToNextPoint();
    }

    void Update()
    {
        // �v���C���[�̈ʒu���擾
        Vector3 playerPosition = player.position;

        // �ϐ� distance ���쐬���ăI�u�W�F�N�g�̈ʒu�ƃ^�[�Q�b�g�I�u�W�F�N�g�̋������i�[
        float distance = Vector3.Distance(transform.position, playerPosition);

        //GetComponent��p����Animator�R���|�[�l���g�����o��.
        animator = this.GetComponent<Animator>();

        //���炩���ߐݒ肵�Ă���int�p�����[�^�[�utrans�v�̒l�����o��.
        bool dead = animator.GetBool("Dead");

        // �ɂ�ɂ��������Ă���ꍇ�A�������~�܂�
        if (playerController._itemFlag)
        {
            // �G�[�W�F���g��~
            agent.isStopped = true;
            // dead�̃A�j���[�V�����𗬂�
            dead = true;
            animator.SetBool("Dead", dead);

            // Collider�̐ݒ�ύX(�B�e�͈͂ƃI�u�W�F�N�g�̊���h�~)
            targetCollider.radius = 3.0f;
            targetCollider.isTrigger = true;
        }
        else
        {

            // �I�u�W�F�N�g���^�[�Q�b�g�Ɍ������Ĉړ����J�n���鋗�����̏ꍇ�v���C���[��ǔ�����
            if (distance < moveDistance)
            {
                // �B�e�Ώۂ��v���C���[��ǔ�����
                agent.SetDestination(playerPosition);
            }
            else
            {
                // �����O�̏ꍇ�͎����ňړ�
                // �G�[�W�F���g�ʒu�����X�Ɉړ����Ă���
                if (!agent.pathPending && agent.remainingDistance < 0.1f)
                {
                    GoToNextPoint();
                }
            }
        }
    }

    void GoToNextPoint()
    {
        // wayPoints�z����ɃE�F�C�|�C���g���Ȃ����ǂ���
        if (wayPoints.Length == 0)
            // �Ȃ��ꍇ�̓��^�[��
            return;

        // ���݂̃E�F�C�|�C���g�iwayPoints[currentPoint]�j�̈ʒu��ݒ�
        agent.SetDestination(wayPoints[currentPoint].position);
        // ���ɃG�[�W�F���g���������E�F�C�|�C���g�̃C���f�b�N�X��ێ�
        currentPoint = (currentPoint + 1) % wayPoints.Length;

    }

}
