using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : Singleton<BulletPoolManager>
{
    public GameObject bulletPrefab;
    Queue<GameObject> bulletPool;
    int poolInitNum = 5;

    protected override void Awake()
    {
        base.Awake();
        InitPool();
    }

    //初始化
    void InitPool()
    {
        bulletPool = new Queue<GameObject>();
        GameObject bullet = null;
        for(int i = 0; i < poolInitNum; i++)
        {
            bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    //获取
    public GameObject GetBullet()
    {
        if (bulletPool != null && bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            return Instantiate(bulletPrefab);
        }
    }


    //回收
    public void RecoverBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

}
