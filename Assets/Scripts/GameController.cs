using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController GameControllerInstance;

    public Text scoreText;
    public int score;
    public int destroyEnemyScore;
    public int destroyBossScore;
    public int enemyWentFar;
    public int bossWentFar;

    public Text waveDuration;
    private float startWave;

    public GameObject[] bossesPefabs;
    public GameObject[] enemiePreFab;
    public Transform[] spawnPonts;

    private int numberOfEnemies;
    private float enemyMaxSpeed;
    private float enemyMinSpeed;
    private float enemyBulletMaxSpeed;
    private float enemyBulletMinSpeed;

    private int enemiesInOneWave;
    private int waveNumber;
    private float spawnTime;
    private float delayBetweenWaves;

    private bool bossCreated;
    private int bossHp;
    private float bossMaxSpeed;
    private float bossMinSpeed;
    private float bossBulletMaxSpeed;
    private float bossBulletMinSpeed;

    public bool gameOver;


    private void Awake()
    {
        bossCreated = false;
        bossHp = 5;
        bossMaxSpeed = 0.6f;
        bossMinSpeed = 0.2f;
        bossBulletMaxSpeed = 3f;
        bossBulletMinSpeed = 2f;

        numberOfEnemies = 0;
        enemyMaxSpeed = 0.5f;
        enemyMinSpeed = 0.1f;
        enemyBulletMaxSpeed = 2.5f;
        enemyBulletMinSpeed = 2f;

        enemiesInOneWave = 7;
        waveNumber = 1;
        spawnTime = 2f;
        delayBetweenWaves = 10f;

        score = 0;
        destroyEnemyScore = 5;
        destroyBossScore = 20;
        enemyWentFar = 15;
        bossWentFar = 45;

        gameOver = false;
        if (GameControllerInstance == null)
        {
            GameControllerInstance = this;
        }
    }

    void Start () {
        startWave = Time.time;
        StartCoroutine("createWave");
    }

    void Update() {
        handleDurationText();
        if (score < 0)
        {
            score = 0;
            scoreText.text = "Score: " + score;
        } else
        {
            scoreText.text = "Score: " + score;
        }
        if (gameOver)
        {
            lostGame();
        }
    }

    IEnumerator createWave()
    {
        while (true)
        {
            startWave = Time.time;
            for (int i = 0; i < enemiesInOneWave; i++)
            {
                createEnemy();
                if (waveNumber % 3 == 0 && !bossCreated) 
                    createBoss();
                yield return new WaitForSeconds(spawnTime);
            }
            yield return new WaitForSeconds(delayBetweenWaves);
            upgradeEnemy();
            increaseEnemyRate();
            increaseScores();
            enemiesInOneWave += 4;
            if (waveNumber % 3 == 0) upgradeBoss();
            waveNumber++;
            bossCreated = false;
        }
    }

    void createEnemy()
    {
        int enemyNumber = Random.Range(0, enemiePreFab.Length);
        int position = Random.Range(0, spawnPonts.Length);
        GameObject enemy = Instantiate(enemiePreFab[enemyNumber], spawnPonts[position].position, spawnPonts[position].rotation);
        enemy.GetComponent<standartEnemyController>().hp = 1;
        enemy.GetComponent<standartEnemyController>().enemyMinSpeed = enemyMinSpeed;
        enemy.GetComponent<standartEnemyController>().enemyMaxSpeed = enemyMaxSpeed;
        enemy.GetComponent<standartEnemyController>().enemyBulletMinSpeed = enemyBulletMinSpeed;
        enemy.GetComponent<standartEnemyController>().enemyBulletMaxSpeed = enemyBulletMaxSpeed;
        numberOfEnemies++;
    }

    void createBoss()
    {
        int bossNumber = Random.Range(0, bossesPefabs.Length);
        int position = Random.Range(0, spawnPonts.Length);
        GameObject boss = Instantiate(bossesPefabs[bossNumber], spawnPonts[position].position, spawnPonts[position].rotation);
        boss.GetComponent<BossController>().hp = bossHp;
        boss.GetComponent<BossController>().bossMaxSpeed = bossMaxSpeed;
        boss.GetComponent<BossController>().bossMinSpeed = bossMinSpeed;
        boss.GetComponent<BossController>().bossBulletMaxSpeed = bossBulletMaxSpeed;
        boss.GetComponent<BossController>().bossBulletMinSpeed = bossBulletMinSpeed;
        bossCreated = true;
    }

    void lostGame()
    {
        SceneManager.LoadScene("GameOver");
    }

    void handleDurationText()
    {
        int sec = (int) (startWave + delayBetweenWaves + (enemiesInOneWave * spawnTime) - Time.time);
        print(sec);
        waveDuration.text = "Next Wave: " + sec +  "s";
    }

    void upgradeEnemy()
    {
        enemyMinSpeed += 0.1f;
        enemyMaxSpeed += 0.1f;
        enemyBulletMinSpeed += 0.1f;
        enemyBulletMaxSpeed += 0.1f;
    }

    void upgradeBoss()
    {
        bossMaxSpeed += 0.3f;
        bossMinSpeed += 0.03f;
        bossBulletMaxSpeed += 0.3f;
        bossBulletMinSpeed += 0.3f;
    }

    void increaseEnemyRate()
    {
        if (spawnTime >= 0.5f)
        {
            spawnTime -= 0.2f;
        }
    }

    void increaseScores()
    {
        destroyBossScore += 10;
        destroyEnemyScore += 5;
        enemyWentFar += 10;
        bossWentFar += 25;
    }
}
