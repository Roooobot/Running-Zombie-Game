using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Vector3 primitivePos;
    public float activeTime = 4f;
    float leftTime;
    private float distance;

    private void OnEnable()
    {
        transform.SetPositionAndRotation(GameManager.Instance.GetGameObject("firePos").transform.position, GameManager.Instance.GetGameObject("firePos").transform.rotation);
        leftTime = activeTime;   
    }

    private void Start()
    {
        distance = 0;
    }

    private void Update()
    {
        primitivePos = transform.position;
        transform.Translate(100 * Time.deltaTime * Vector3.forward);
        Hit();
        leftTime-=Time.deltaTime;
        if(leftTime < 0)
        {
            BulletPoolManager.Instance.RecoverBullet(this.gameObject);
        }
    }

    private void Hit()
    {
        Ray ray = new Ray(primitivePos, (transform.position - primitivePos).normalized) ;
        distance = Vector3.Distance(transform.position, primitivePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.transform.CompareTag("Player")|| hit.transform.CompareTag("Zombie"))
            {
                hit.transform.SendMessage("Damage",10,SendMessageOptions.DontRequireReceiver);
                BulletPoolManager.Instance.RecoverBullet(this.gameObject);
            }
        }
    }
}
