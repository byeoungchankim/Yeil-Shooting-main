using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public int enemyScore;
    Animator anim;
    public GameObject player;

    public Sprite[] sprites;
    SpriteRenderer spriteRender;

    Rigidbody2D rd;

    public GameObject bulletPrefab;

    public GameObject ItmeCoin;
    public GameObject ItemBoom;
    public GameObject ItemMine;
    public GameObject ItemPower;
    public float curBulletDelay = 0f;
    public float maxBulletDelay = 1f;
    
    public GameObject playerObject;
    
    void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        //rd.velocity = Vector2.down * speed; => Move 함수로 이동
        spriteRender = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //rd = GetComponent<Rigidbody2D>();
        ////rd.velocity = Vector2.down * speed; => Move 함수로 이동
        //spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
        ReloadBullet();
    }

    void Fire()
    {
        if (curBulletDelay > maxBulletDelay)
        {
            Power();

            curBulletDelay = 0;
        }
    }

    void Power()
    {
        if (playerObject == null) return;
        if (playerObject.activeSelf == false) return;

        GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rdBullet = bulletObj.GetComponent<Rigidbody2D>();
        
        Vector3 dirVec = playerObject.transform.position - transform.position;
        rdBullet.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void ReloadBullet()
    {
        curBulletDelay += Time.deltaTime;
    }

    public void Move(int nPoint)
    {
        if (nPoint == 3 || nPoint == 4) // 오른쪽에 있는 스폰 포인트의 배열 인덳스값
        {
            transform.Rotate(Vector3.back * 90);
            rd.velocity = new Vector2(speed * (-1), -1);
        }
        else if (nPoint == 5 || nPoint == 6) // 왼쪽에 있는 스폰 포인트의 배열 인덳스값
        {
            transform.Rotate(Vector3.forward * 90);
            rd.velocity = new Vector2(speed, -1);
        }
        else
        {
            rd.velocity = Vector2.down * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.power);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "PlayerPang")
        {
            Pang pang = collision.gameObject.GetComponent<Pang>();
            OnHit(pang.power);
            //Destroy(collision.gameObject);
        }
    }

    public void OnHit(float BulletPower)
    {
        if (health <= 0)
            return;

        health -= BulletPower;
        spriteRender.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0)
        {

            GameManager.Instance.score += enemyScore;

            //아이템 랜덤으로 떨굼
            float ran = Random.value;
            Debug.Log("Item RNG: " + ran.ToString("0.00"));
            if (ran < 0.15f)//코인
            {
                Instantiate(ItmeCoin, transform.position, ItmeCoin.transform.rotation);
            }
            else if (ran < 0.30f)//파워
            {
                Instantiate(ItemPower, transform.position, ItemPower.transform.rotation);

            }
            else if (ran < 0.45f)//팡
            {
                Instantiate(ItemBoom, transform.position, ItemBoom.transform.rotation);
            }
            else if (ran < 0.60f)//마인
            {
                Instantiate(ItemMine, transform.position, ItemMine.transform.rotation);
            }
            else
            {
                //Debug.Log("Not Item");
            }

            Destroy(gameObject);
        }
    }


    void ReturnSprite()
    {
        spriteRender.sprite = sprites[0];
    }
}
