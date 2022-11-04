using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingHumanObserver : MonoBehaviour
{
    private ShootingHumanController shootingHumanController;
    //��ȡ���ȷ��ֵ�zombie��״̬
    private CharacterStats currentZombieStats;
    //��ȡ���ȷ��ֵ�zombie�Ķ���
    private GameObject currentZombie;
    //ɥʬ�Ƿ�����Ұ��
    private bool m_IsZombieInRange;
    //�Ƿ�����ɥʬ
    private bool foundPlayer = false;
    public bool FoundPlayer { get { return foundPlayer; } set { foundPlayer = value; } }

    private void Start()
    {
        shootingHumanController = GetComponentInParent<ShootingHumanController>();
    }

    void Update()
    {
        if (currentZombie == null&& currentZombieStats==null)
        {
            FoundPlayer = false;
            m_IsZombieInRange=false;
            currentZombie = null;
            currentZombieStats = null;
        }
        //ɥʬ����Ұ�ڵ�ʱ���������ж�����֮���Ƿ����赲
        if (m_IsZombieInRange)
        {
            Vector3 direction = currentZombie.transform.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                if (raycastHit.collider.CompareTag("Player")|| raycastHit.collider.CompareTag("Zombie"))
                {
                    FoundPlayer = true;
                    currentZombieStats = currentZombie.GetComponent<CharacterStats>();
                    shootingHumanController.CurrentZombie = currentZombie;
                    shootingHumanController.CurrentZombieCharacterStats = currentZombieStats;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")||other.CompareTag("Zombie")) 
        {
            currentZombie = other.GameObject();
            m_IsZombieInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Zombie"))
        {
            currentZombie = null;
            m_IsZombieInRange = false;
        }
    }
}
