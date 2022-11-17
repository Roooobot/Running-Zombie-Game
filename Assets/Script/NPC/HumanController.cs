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

    public GameObject zombiePrefab;                                         //死后生成的丧尸Prefab
    private bool isCreat=false;                                                      //是否已经生成丧尸
    private GameObject currentZombie;                                       //当前要逃离的对象
    public GameObject CurrentZombie { get { return currentZombie; } set { currentZombie = value; } }
    private CharacterStats currentZombieCharacterStats;             //当前要逃离的对象的状态数据
    public CharacterStats CurrentZombieCharacterStats { get { return currentZombieCharacterStats; } set { currentZombieCharacterStats = value; } }
    AudioSource stepSource;

    public bool isGuard;                                                //是否为原地不动
    public float Speed = 2.5f;
    private bool isEmptyHealth = false;
    private bool isRunning = false;
    private bool isWalking = false;
    private bool isHurt = false;
    private bool isPlayerDead = false;
    private Vector3 direction;                                        //逃命方向
    private readonly float StopDistance = 15f;              //停止距离
    private float CurrentDistance;                                  //当前距离
    private readonly float patrolRange = 5f;                 //巡逻半径
    private Vector3 wayPoint;                                       //巡逻点
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
            if (!IsCurrentZombieDead())                                                             //要逃离的目标未死
            {
                CurrentDistance = (transform.position - CurrentZombie.transform.position).magnitude;                //CurrentZombie有修改
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
            else                                                                                                       //要逃离的目标已死
            {
                humanStates = HumanStates.GUARD;
                CurrentZombie = null;
                CurrentZombieCharacterStats = null;
            }
        }
        switch (humanStates)
        {
            case HumanStates.GUARD:                             //原地
                if (!m_observer.gameObject.activeSelf)
                    m_observer.gameObject.SetActive(true);
                isRunning = false;
                isWalking = false;
                break;
            case HumanStates.PATROL:                            //巡逻
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

            case HumanStates.RUNAWAY:                       //逃命
                if (m_observer.gameObject.activeSelf)
                    m_observer.gameObject.SetActive(false);
                isRunning = true;
                isWalking = false;
                agent.speed = Speed;
                direction = transform.position - CurrentZombie.transform.position;
                direction = Vector3.ClampMagnitude(direction, 1);
                agent.SetDestination(transform.position + direction);
                break;

            case HumanStates.HURT:                              //受伤
                m_Collider.enabled = false;
                agent.enabled = false;
                isHurt = true;
                Invoke("CreatZombie", 5f);
                Destroy(gameObject, 6f);
                break;
            case HumanStates.DEAD:                              //死亡
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
