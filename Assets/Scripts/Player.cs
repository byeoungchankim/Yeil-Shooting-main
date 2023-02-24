using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 0f;

    public int Mine;
    public int maxMine;
    public int Boom;
    public int maxBoom;
    public int score;
    public int life = 3;
    public int power;
    public int maxpower;


    public bool isHit = false;

    public bool isTouchTop = false;
    public bool isTouchBottom = false;
    public bool isTouchRight = false;
    public bool isTouchLeft = false;

    SpriteRenderer spriteRender;

    Animator anim;
    private static GameManager Manager => GameManager.Instance;

    public GameObject bulletPrefabA;
    public GameObject bulletPrefabB;
    public GameObject bulletPrefabC;
    public GameObject bulletPrefabD;

    public GameObject Pang;
    public GameObject Follower;

    public float curBulletDelay = 0f;
    public float maxBulletDelay = 1f;
    public bool isRespawnTime ;



    public GameObject gameMgrObj;
    public bool die;
    public bool isBoomTime;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        KeyBoom();
        ReloadBullet();
    }
    void Move()
    {
        if (isHit) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))
        {
            h = 0;
        }
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
        {
            v = 0;
        }

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        anim.SetInteger("Input", (int)h);
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        float delay = maxBulletDelay;
        if (power == 5) delay *= 0.05f;
        if (curBulletDelay < delay)
            return;
        
        Power();
        //폭발 발동
        Pang.SetActive(false);
        //적 사라짐
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int index=0; index < enemies.Length; index++)
        {
            Enemy enemyLogic = enemies[index].GetComponent<Enemy>();
        }
        
        curBulletDelay = 0;
    }

    void ReloadBullet()
    {
        curBulletDelay += Time.deltaTime;
    }

    
    void Power()
    {
        switch(power)
        {
            case 1:
                {
                    GameObject bullet = Instantiate(bulletPrefabA, transform.position, Quaternion.identity);
                    Rigidbody2D rd = bullet.GetComponent<Rigidbody2D>();
                    rd.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;
            case 2:
                {
                    GameObject bulletR = Instantiate(bulletPrefabA, 
                        transform.position + Vector3.right * 0.1f, 
                        Quaternion.identity);
                    Rigidbody2D rdR = bulletR.GetComponent<Rigidbody2D>();
                    rdR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                    GameObject bulletL = Instantiate(bulletPrefabA, 
                        transform.position + Vector3.left * 0.1f, 
                        Quaternion.identity);
                    Rigidbody2D rdL = bulletL.GetComponent<Rigidbody2D>();
                    rdL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;
            case 3:
                {
                    GameObject bulletR = Instantiate(bulletPrefabA,
                        transform.position + Vector3.right * 0.25f,
                        Quaternion.identity);
                    Rigidbody2D rdR = bulletR.GetComponent<Rigidbody2D>();
                    rdR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                    GameObject bulletC = Instantiate(bulletPrefabB,
                        transform.position,
                        Quaternion.identity);
                    Rigidbody2D rdC = bulletC.GetComponent<Rigidbody2D>();
                    rdC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                    GameObject bulletL = Instantiate(bulletPrefabA,
                        transform.position + Vector3.left * 0.25f,
                        Quaternion.identity);
                    Rigidbody2D rdL = bulletL.GetComponent<Rigidbody2D>();
                    rdL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;
            case 4:
                {
                    GameObject bulletc = Instantiate(bulletPrefabC, transform.position, Quaternion.identity);
                    Rigidbody2D rdc = bulletc.GetComponent<Rigidbody2D>();
                    rdc.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
                break;

            case 5:
                {
                    GameObject bulletSL = Instantiate(bulletPrefabD);
                    bulletSL.transform.position = transform.position + new Vector3(-Mathf.Sin(Time.timeSinceLevelLoad * 15f) * 0.4f, 0f);
                    Rigidbody2D rdSL = bulletSL.GetComponent<Rigidbody2D>();
                    rdSL.AddForce(Vector2.up * 8, ForceMode2D.Impulse);
                    GameObject bulletSR = Instantiate(bulletPrefabD);
                    bulletSR.transform.position = transform.position + new Vector3(Mathf.Sin(Time.timeSinceLevelLoad * 15f) * 0.4f, 0f);
                    Rigidbody2D rdSR = bulletSR.GetComponent<Rigidbody2D>();
                    rdSR.AddForce(Vector2.up * 8, ForceMode2D.Impulse);
                }
                break;

        }
        
    }
    //리스폰
    void Awake()
    {

        spriteRender = GetComponent<SpriteRenderer>(); //색깔을 바꾸기 때문에 변수 선언 필요 
    }

    void OnEnable()
    {
        isRespawnTime = false;
        Unbeatable();

        Invoke(nameof(Unbeatable), 3);

    }

    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;

        if (isRespawnTime) //무적 타임 이펙트 (투명)
        {
            spriteRender.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            spriteRender.color = new Color(1, 1, 1, 1); //무적 타임 종료(원래대로)
        }
        //리스폰 여기까지
    }
    void KeyBoom()
    {
        if (!Input.GetButton("Fire2"))
            return;

        if (isBoomTime) 
        return;

        if (Boom == 0)
            return;

        Boom--;
        isBoomTime = true;
        Pang.SetActive(true);
        Manager.UpdateBoomIcon(Boom);
        Invoke("OffPang", 1f);
        //적 제거
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int index = 0; index < enemies.Length; index++)
        {
            // Enemy enemyLogic = enemies[index].GetComponent<Enemy>();
            //  enemyLogic.OnHit(1000);
        }
        //총알 제거
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int index = 0; index < bullets.Length; index++)
        {
            Destroy(bullets[index]);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBorder")
        {
            switch(collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (isRespawnTime) //무적 시간이면 적에게 맞지 않음
                return;
            if (isHit) return;

            isHit = true;
            anim.SetTrigger("Die");

            life--;
            Manager.UpdateLifeIcon(life);


            Invoke(nameof(ReallyDie), 1f);
            Invoke(nameof(DieBoom), 0.5f);


            if (life == 0)
            {
                Manager.GameOver();
            }
            else
            {
                Manager.RespawnPlayer();
            }
            

        }
        else if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            
            switch (item.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if(power == maxpower)
                        score += 500;
                    else
                    power++;
                    break;
                case "Boom":
                    if (Boom == maxBoom)
                        score += 500;
                    else
                    {
                        Boom++;
                        
                        Manager.UpdateBoomIcon(Boom);
                    }

                    break;
                case "Mine":
                    if (Mine == maxMine) score += 500;
                    else AddFollower();
                    break;

            }
            Destroy(collision.gameObject);
        }
    }

    void AddFollower()
    {
        Mine++;
        Follower.SetActive(true);
    }

    void OffPang()
    {
        Pang.SetActive(false);
    }

    void DieBoom()
    {
        anim.SetTrigger("Boom");
        isBoomTime = false;
    }

    private void ReallyDie()
    {
        if(Mine > 0)
        {
            Follower.SetActive(false);
            Mine = 0;
        }
        power = 1;
        



        var allBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (var bullet in allBullets)
        { Destroy(bullet); }
        gameObject.SetActive(false);
       // anim.ResetTrigger("Boom");
      //  anim.ResetTrigger("Die");
        if (life == 0)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            GameManager.Instance.RespawnPlayerEXE();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBorder")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
