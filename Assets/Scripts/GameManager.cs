using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;


    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;

    public float curEnemySpawnDelay;
    public float nextEnemySpawnDelay;

    public long score;
    public GameObject player;
    public Text ScoreText;
    public Text GAMEOVERScoreText;
    public Image[] lifeImages;
    public Image[] BoomImages;
    public GameObject gameOverSet;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;


    void Awake()
    {
        spawnList = new List<Spawn>();
        if (null == instance)
        {
            instance = this;
           // DontDestroyOnLoad(this.gameObject);
        }
        else
        {

            Destroy(this.gameObject);
        }
        ReadSpawnFile();
    }
    public static GameManager Instance => instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curEnemySpawnDelay += Time.deltaTime;
        if (curEnemySpawnDelay > nextEnemySpawnDelay)
        {
            SpawnEnemy();

            nextEnemySpawnDelay = Random.Range(0.5f, 3.0f);
            curEnemySpawnDelay = 0;
        }

        ScoreText.text = string.Format("{0:n0}", score);
        GAMEOVERScoreText.text = string.Format("{0:n0}", score);
    }
    void ReadSpawnFile()
    {
        //변수 초기화 
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // 스폰 파일
        TextAsset textFile = Resources.Load("Stage0") as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);
        //한줄씩 데이터 저장
        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line) ;

            if (line == null)
                break;
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);

        }

        stringReader.Close();
        //첫번
        curEnemySpawnDelay = spawnList[0].delay;



    }


    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type)
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
        }
        int ranType = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 7);
        GameObject goEnemy = Instantiate(enemyPrefabs[ranType], spawnPoints[ranPoint].position, Quaternion.identity);
        Enemy enemyLogic = goEnemy.GetComponent<Enemy>();
        enemyLogic.playerObject = player;
        enemyLogic.Move(ranPoint);
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
        ScoreText.gameObject.SetActive(false);
    }

    public void RespawnPlayer()
    {
        Invoke("AlivePlayer", 2.0f);
    }
    public void UpdateLifeIcon(int life)
    {
        for (int index = 0; index < lifeImages.Length; index++)
        {
            lifeImages[index].color = new Color(1, 1, 1, 0);
        }
        for (int index=0; index < life; index++)
        {
            lifeImages[index].color = new Color(1, 1, 1, 1);
        }
    }
    public void UpdateBoomIcon(int Boom)
    {
        for (int index = 0; index < BoomImages.Length; index++)
        {
            BoomImages[index].color = new Color(1, 1, 1, 0);
        }
        for (int index = 0; index < Boom; index++)
        {
            BoomImages[index].color = new Color(1, 1, 1, 1);
        }
    }

    void AlivePlayer()
    {
        player.transform.position = Vector3.down * 4.2f;
        player.SetActive(true);
        Player playrLogic = player.GetComponent<Player>();
        playrLogic.isHit = false;
    }
    public void RespawnPlayerEXE()
    {
       Invoke("AlivePlayer", 1.0f);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
