using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;


public enum HumanStates { GUARD, PATROL, RUNAWAY, HURT, DEAD }
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class HumanController : MonoBehaviour, IEndGameObserver
{
    private HumanStates humanStates;
    private NavMeshAgent agent;
    private Observer m_observer;
    private Animator m_Animator;
    private CharacterStats characterStats;
    private Collider m_Collider;

    public GameObject zombiePrefab;                                         //�������ɵ�ɥʬPrefab
    private bool isCreat=false;                                                      //�Ƿ��Ѿ�����ɥʬ
    private GameObject currentZombie;                                       //��ǰҪ����Ķ���
    public GameObject CurrentZombie { get { return currentZombie; } set { currentZombie = value; } }
    private CharacterStats currentZombieCharacterStats;             //��ǰҪ����Ķ����״̬����
    public CharacterStats CurrentZombieCharacterStats { get { return currentZombieCharacterStats; } set { currentZombieCharacterStats = value; } }
    AudioSource stepSource;

    public bool isGuard;                                                //�Ƿ�Ϊԭ�ز���
    public float Speed = 2.5f;
    private bool isEmptyHealth = false;
    private bool isRunning = false;
    private bool isWalking = false;
    private bool isHurt = false;
    private bool isPlayerDead = false;
    private Vector3 direction;                                        //��������
    private readonly float StopDistance = 15f;              //ֹͣ����
    private float CurrentDistance;                                  //��ǰ����
    private readonly float patrolRange = 5f;                 //Ѳ�߰뾶
    private Vector3 wayPoint;                                       //Ѳ�ߵ�
    private Vector3 guardPoint;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        m_observer = GetComponentInChildren<Observer>();
        m_Animator = GetComponentInChildren<Animator>();
        characterStats = GetComponent<CharacterStats>();
        m_Collider = GetComponentInChildren<Collider>();
        stepSource=GetComponentInChildren<AudioSource>();
        guardPoint = transform.position;
    }

    private void Start()
    {
        if (isGuard)
        {
            humanStates = HumanStates.GUARD;
        }
        else
        {
            humanStates = HumanStates.PATROL;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.AddObserver(this);
        GameManager.Instance.AddWinObserver(this);
    }

    private void OnDisable()
    {
        if (!GameManager.IsIntialized) return;
        {
            GameManager.Instance.RemoveObserver(this);
            GameManager.Instance.RemoveWinObserver(this);
        }
    }
    private void Update()
    {
        if (characterStats.CurrentHealth == 0)
            isEmptyHealth = true;
        if (!isPlayerDead)
        {
            SwitchStates();
        }
        SwitchuStates();
    }

    void SwitchStates()
    {
        if (isEmptyHealth)
            humanStates = HumanStates.HURT;
        else if (!m_observer.FoundPlayer)
        {
            humanStates = HumanStates.GUARD;
        }
        else if (m_observer.FoundPlayer)
        {
            if (!IsCurrentZombieDead())                                                             //Ҫ�����Ŀ��δ��
            {
                CurrentDistance = (transform.position - CurrentZombie.transform.position).magnitude;                //CurrentZombie���޸�
                if (CurrentDistance <= StopDistance)
                {
                    humanStates = HumanStates.RUNAWAY;
                }
                else
                {
                    humanStates = HumanStates.GUARD;
                    GetNewWayPoint();
                }
            }
            else                                                                                                       //Ҫ�����Ŀ������
            {
                humanStates = HumanStates.GUARD;
                CurrentZombie = null;
                CurrentZombieCharacterStats = null;
            }
        }
        switch (humanStates)
        {
            case HumanStates.GUARD:                             //ԭ��
                if (!m_observer.gameObject.activeSelf)
                    m_observer.gameObject.SetActive(true);
                isRunning = false;
                isWalking = false;
                break;
            case HumanStates.PATROL:                            //Ѳ��
                if (!m_observer.gameObject.activeSelf)
                    m_observer.gameObject.SetActive(true);
                agent.speed = Speed * 0.5f;
                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    isWalking = false;
                    GetNewWayPoint();
                }
                else
                {
                    isWalking = true;
                    agent.SetDestination(wayPoint);
                }
                break;

            case HumanStates.RUNAWAY:                       //����
                if (m_observer.gameObject.activeSelf)
                    m_observer.gameObject.SetActive(false);
                isRunning = true;
                isWalking = false;
                agent.speed = Speed;
                direction = transform.position - CurrentZombie.transform.position;
                direction = Vector3.ClampMagnitude(direction, 1);
                agent.SetDestination(transform.position + direction);
                break;

            case HumanStates.HURT:                              //����
                m_Collider.enabled = false;
                agent.enabled = false;
                isHurt = true;
                Invoke("CreatZombie", 5f);
                Destroy(gameObject, 6f);
                break;
            case HumanStates.DEAD:                              //����
                break;
        }
    }

    bool IsCurrentZombieDead()
    {
        if (CurrentZombie != null && CurrentZombieCharacterStats != null)
            return false;
        else return true;
    }

    private void SwitchuStates()
    {
        m_Animator.SetBool("IsRunning", isRunning);
        m_Animator.SetBool("IsWalking", isWalking);
        m_Animator.SetBool("IsHurting", isHurt);
    }

    private void GetNewWayPoint()
    {
        float randomX = UnityEngine.Random.Range(-patrolRange, patrolRange);
        float randomZ = UnityEngine.Random.Range(-patrolRange, patrolRange);
        Vector3 randomPoint = new(guardPoint.x + randomX, transform.position.y, guardPoint.z + randomZ);
        wayPoint = NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRange, 1) ? hit.position : transform.position;
    }

    private void CreatZombie()
    {
        if (!isCreat)
        {
            Instantiate(zombiePrefab, transform.position, transform.rotation);
            isCreat = true;
        }
    }

    public void EndNotify()
    {
        isWalking = false;
        isRunning = false;
        isPlayerDead = true;
    }
    void PlayStepSource()
    {
        stepSource.Play();
    }

    public void WinNotify()
    {
        return;
    }
}
