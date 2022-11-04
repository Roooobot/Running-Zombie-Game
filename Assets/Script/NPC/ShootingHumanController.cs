using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;


public enum ShootingHumanStates { SEARCH, SHOOT, HURT }
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class ShootingHumanController : MonoBehaviour,IEndGameObserver
{
    private ShootingHumanStates humanStates;
    private NavMeshAgent agent;
    private ShootingHumanObserver m_observer;
    private Animator m_Animator;
    private CharacterStats characterStats;
    private Collider m_Collider;

    public GameObject zombiePrefab;                                             //�������ɵ�ɥʬPrefab
    private bool isCreat = false;                                                        //�Ƿ��Ѿ�����ɥʬ
    private GameObject currentZombie;
    public GameObject CurrentZombie { get { return currentZombie; }  set{currentZombie=value;} }
    private CharacterStats currentZombieCharacterStats;
    public CharacterStats CurrentZombieCharacterStats { get { return currentZombieCharacterStats; } set{ currentZombieCharacterStats = value;} }

    private readonly float StopDistance = 15f;              //ֹͣ����
    private float CurrentDistance;                                  //��ǰ����

    private bool isEmptyHealth=false;
    private bool isSearching = false;
    private bool isShooting = false;
    private bool isHurt = false;
    private bool isPlayerDead=false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        m_observer = GetComponentInChildren<ShootingHumanObserver>();
        m_Animator = GetComponentInChildren<Animator>();
        characterStats = GetComponent<CharacterStats>();
        m_Collider = GetComponentInChildren<Collider>();
    }

    private void Start()
    {
        humanStates = ShootingHumanStates.SEARCH;
    }

    private void OnEnable()
    {
        GameManager.Instance.AddObserver(this);
    }

    private void OnDisable()
    {
        if (!GameManager.IsIntialized) return;
        GameManager.Instance.RemoveObserver(this);
    }
    private void Update()
    {
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
            humanStates = ShootingHumanStates.HURT;
        else if (m_observer.FoundPlayer)
        {
            if (!IsCurrentZombieDead())                                                             //Ҫ�����Ŀ��δ��
            {
                CurrentDistance = (transform.position - currentZombie.transform.position).magnitude;
                if (CurrentDistance <= StopDistance)
                {
                    humanStates = ShootingHumanStates.SHOOT;
                }
                else
                {
                    humanStates = ShootingHumanStates.SEARCH;
                }
            }
            else                                                                                                       //Ҫ�����Ŀ������
            {
                humanStates = ShootingHumanStates.SEARCH;
                CurrentZombie = null;
                CurrentZombieCharacterStats = null;
            }

        }

        switch (humanStates)
        {
            case ShootingHumanStates.SEARCH:                             
                if (!m_observer.gameObject.activeSelf)
                    m_observer.gameObject.SetActive(true);
                isSearching = true;
                isShooting = false;
                break;
            case ShootingHumanStates.SHOOT:
                m_observer.gameObject.SetActive(false);
                transform.LookAt(currentZombie.transform);
                isSearching = false;
                isShooting = true;
                break;
            case ShootingHumanStates.HURT:                              //����
                m_Collider.enabled = false;
                agent.enabled = false;
                isHurt = true;
                Invoke("CreatZombie", 5f);
                Destroy(gameObject, 6f);
                break;
        }
    }

    private void SwitchuStates()
    {
        m_Animator.SetBool("IsSearching", isSearching);
        m_Animator.SetBool("IsShooting", isShooting);
        m_Animator.SetBool("IsHurting", isHurt);
    }

    bool IsCurrentZombieDead()
    {
        if (CurrentZombie != null && CurrentZombieCharacterStats != null)
            return false;
        else return true;
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
        isSearching = true;
        isShooting = false;
        isPlayerDead=true;
    }
}
