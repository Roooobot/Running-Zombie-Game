using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;    //�ӵ�Ԥ����
    private Transform firePos;                //ǹ��
    AudioSource shootSouce;

    private void OnEnable()
    {
        firePos = transform.GetChild(0).transform;
        shootSouce = GetComponent<AudioSource>();
    }

    public void Shooting()
    {
        GameObject bullet = BulletPoolManager.Instance.GetBullet();
        bullet.transform.position = firePos.transform.position;
        bullet.transform.rotation = firePos.transform.rotation;
        firePos.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        StartCoroutine(Close());
        shootSouce.Play();
    }

    private IEnumerator Close()
    {
        yield return new WaitForSeconds(0.05f);
        firePos.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }
}
