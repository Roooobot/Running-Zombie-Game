using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;

    List<IEndGameObserver> endGameObservers = new();
    List<GameObject> otherZombie =new();
    public void RigisterPlayer(CharacterStats player)
    {
        playerStats = player;
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    public void AddZombie(GameObject zombie)
    {
        otherZombie.Add(zombie);
    }

    public void RemoveZombie(GameObject zombie)
    {
        otherZombie.Remove(zombie);
    }

    public void NotifyObservers()
    {
        foreach(var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }

    public Transform GetEntrance()
    {
        foreach(var item in FindObjectsOfType<GameObject>())
        {
            if (item.CompareTag("EnterPoint"))
            {
                return item.transform;
            }
        }
        return null;
    }

    public GameObject GetGameObject(string name)
    {
        foreach (var item in FindObjectsOfType<GameObject>())
        {
            if (item.name==name)
            {
                return item;
            }
        }
        return null;
    }

    public void GetAttackTarget(GameObject attackTarget)
    {
        foreach(var item in otherZombie)
        {
            ZombieController zombie= item.GetComponent<ZombieController>();
            zombie.attackTarget = attackTarget;
        }
    }
}
