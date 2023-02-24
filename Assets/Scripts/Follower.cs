using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float curBulletDelay = 0f;
    public float maxBulletDelay = 1f;
    public GameObject bulletObj;

    public int followDelay;
    public Vector3 followPos;
    public Transform parent;
    public Queue<Vector3> parentPos;

    private void Awake()
    {
        parentPos = new Queue<Vector3>();
    }
    void Update()
    {
        Watch();
        Follow();
        Fire();
        ReloadBullet();
    }

    void Watch()
    {   //FIFO 부모 포지션
        if(!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position);

        //팔로워 포지션
        if(parentPos.Count > followDelay)
        followPos = parentPos.Dequeue();
        else if (parentPos.Count < followDelay)
            followPos = parent.position;
    }

    void Follow()
    {
        transform.position = followPos;
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        if (curBulletDelay < maxBulletDelay)
            return;

        GameObject bullet = Instantiate(bulletObj, transform.position, Quaternion.identity);
        Rigidbody2D rd = bullet.GetComponent<Rigidbody2D>();
        rd.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        curBulletDelay = 0;
    }

    void ReloadBullet()
    {
        curBulletDelay += Time.deltaTime;
    }


}
