using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingHumanObserver : MonoBehaviour
{
    private ShootingHumanController shootingHumanController;
    //获取首先发现的zombie的状态
    private CharacterStats currentZombieStats;
    //获取首先发现的zombie的对象
    private GameObject currentZombie;
    //丧尸是否在视野内
    private bool m_IsZombieInRange;
    //是否发现了丧尸
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
        //丧尸在视野内的时候用射线判断两者之间是否有阻挡
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
