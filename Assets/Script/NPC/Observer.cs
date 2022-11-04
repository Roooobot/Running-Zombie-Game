using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Observer : MonoBehaviour
{
    private HumanController humanController;
    //获取首先发现的zombie的状态
    private CharacterStats currentZombieStats;
    //获取首先发现的zombie的对象
    private GameObject currentZombie;

    private bool m_IsZombieInRange;
    private bool foundPlayer = false;
    public bool FoundPlayer { get { return foundPlayer; } set { foundPlayer = value; } }

    private void Start()
    {
        humanController = GetComponentInParent<HumanController>();
    }

    void Update()
    {
        if (currentZombie == null && currentZombieStats == null)
        {
            FoundPlayer = false;
            m_IsZombieInRange = false;
            currentZombie = null;
            currentZombieStats = null;
        }
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
                    humanController.CurrentZombie = currentZombie;
                    humanController.CurrentZombieCharacterStats = currentZombieStats;
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
