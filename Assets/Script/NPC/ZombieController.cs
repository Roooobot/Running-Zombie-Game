using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;


public enum ZombieStates { IDLE, CHASE, DEAD }
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class ZombieController : MonoBehaviour,IEndGameObserver
{
    private ZombieStates zombieStates;
    private NavMeshAgent agent;
    private Animator m_Animator;
    private CharacterStats characterStats;
    private Collider m_Collider;
    public GameObject attackTarget;
    public GameObject Hand;

    private float lastAttackTime;
    private bool isEmptyHealth=false;
    private bool isRun = false;
    private bool isDead = false;
    private bool isPlayerDead=false;
    AudioSource walkStep;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponentInChildren<Animator>();
        characterStats = GetComponent<CharacterStats>();
        m_Collider = GetComponentInChildren<Collider>();
        walkStep = GetComponent<AudioSource>();

    }

    private void Start()
    {
        zombieStates = ZombieStates.IDLE;
    }

    private void OnEnable()
    {
        GameManager.Instance.AddObserver(this);
        GameManager.Instance.AddZombie(this.gameObject);
    }
    private void OnDisable()
    {
        if (!GameManager.IsIntialized) return;
        GameManager.Instance.RemoveObserver(this);
        GameManager.Instance.RemoveZombie(this.gameObject);
    }

    private void Update()
    {
        lastAttackTime-= Time.deltaTime;
        if(characterStats.CurrentHealth==0)
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
            zombieStates = ZombieStates.DEAD;
        else if (attackTarget==null)
        {
            zombieStates = ZombieStates.IDLE;
        }
        else if (attackTarget != null)
        {
            zombieStates = ZombieStates.CHASE;
        }

        switch (zombieStates)
        {
            case ZombieStates.IDLE:
                isRun=false;
                break;
            case ZombieStates.CHASE:
                isRun = true;
                agent.isStopped = false;
                if (TargetInAttackRange())
                {
                    isRun = false;
                    agent.isStopped = true;
                    transform.LookAt(attackTarget.transform);
                    m_Animator.SetTrigger("IsAttacking");
                    if (lastAttackTime < 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDowm;
                    }
                }
                else
                    agent.destination = attackTarget.transform.position;
                break;

            case ZombieStates.DEAD:
                m_Collider.enabled = false;
                agent.enabled = false;
                isDead = true;
                Destroy(gameObject, 2f);
                break;
        }
    }

    bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= 1;
        else
            return false;
    }

    private void SwitchuStates()
    {
        m_Animator.SetBool("IsRunning", isRun);
        m_Animator.SetBool("IsDead", isDead);
    }


    public void EndNotify()
    {
        isRun=false;
        isPlayerDead=true;
    }

    void HitBegin()
    {
        Hand.SetActive(true);
    }
    void HitEnd()
    {
        Hand.SetActive(false);
    }
    void PlayStepSource()
    {
        walkStep.Play();
    }
    public void WinNotify()
    {
        return;
    }

}
