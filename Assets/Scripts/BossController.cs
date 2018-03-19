using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

    private Rigidbody2D rb2d;
    public int hp;
    public float bossMaxSpeed;
    public float bossMinSpeed;
    private float enemySpeed;

    private float fireRate;
    private float lastShoot;

    public GameObject smokePrefab;
    private GameObject smoke;

    public GameObject explosionPrefab;
    private GameObject explosion;

    public GameObject enemyBulletPrefab;
    private GameObject enemyBullet;
    public float bossBulletMaxSpeed;
    public float bossBulletMinSpeed;
    private float enemyBulletSpeed;

    private float bossCurveSpeed;
    private float curveRate;
    private float lastCurveChange;
    private bool scoreAdded;
    private bool inScene;

    // Use this for initialization
    void Start()
    {
        inScene = false;
        scoreAdded = false;
        enemyBulletSpeed = Random.Range(bossBulletMinSpeed, bossBulletMaxSpeed);
        fireRate = Random.Range(2f, 5f);
        curveRate = Random.Range(1f, 8f);
        lastShoot = Time.time;
        lastCurveChange = Time.time;
        bossCurveSpeed = 0.2f;
        enemySpeed = Random.Range(-bossMinSpeed, -bossMaxSpeed);
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(enemySpeed, bossCurveSpeed);
        Destroy(this.gameObject, 25f);
    }

    // Update is called once per frame
    void Update()
    {
        handleScore();
        if (hp <= 0)
        {
            destroy();
        }
        if (Time.time > lastShoot + fireRate && hp > 0)
        {
            shoot();
            lastShoot = Time.time;
            fireRate = Random.Range(2f, 3.5f);
        }

        if (Time.time > lastCurveChange + curveRate && hp > 0)
        {
            lastCurveChange = Time.time;
            bossCurveSpeed *= -1;
            rb2d.velocity = new Vector2(enemySpeed, bossCurveSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WarriorBullet" && inScene)
        {
            if (hp > 0)
            {
                explosion = Instantiate(explosionPrefab,
                                        this.transform.position,
                                        this.transform.rotation);
                Destroy(collision.gameObject);
                Destroy(explosion, 2f);
            }
            hp--;
        }

        if (collision.gameObject.tag == "horizontalBorder")
        {
            lastCurveChange = Time.time;
            bossCurveSpeed *= -1;
            rb2d.velocity = new Vector2(enemySpeed, bossCurveSpeed);
        }

        if (collision.gameObject.tag == "EnemyWentFar" && !scoreAdded)
        {
            GameController.GameControllerInstance.score -= GameController.GameControllerInstance.bossWentFar;
            scoreAdded = true;
        }

        if (collision.gameObject.tag == "enemyBorder")
        {
            inScene = true;
        }
    }

    private void destroy()
    {
        transform.Rotate(0, 0, 0.4f);
        if (explosion != null)
        {
            explosion.transform.position = new Vector3(this.transform.position.x,
                                                    this.transform.position.y, -9f);
            explosion.transform.rotation = this.transform.rotation;
            smoke.transform.position = new Vector3(this.transform.position.x,
                                                    this.transform.position.y, -9f);
            smoke.transform.rotation = this.transform.rotation;
        }
        rb2d.velocity = new Vector2(enemySpeed / 2, -0.7f);
        Destroy(this.gameObject, 7f);
    }

    private void shoot()
    {
        enemyBullet = Instantiate(enemyBulletPrefab,
                                        this.transform.position,
                                        this.transform.rotation);
        enemyBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-enemyBulletSpeed, 0);
        Destroy(enemyBullet, 7f);
    }

    private void handleScore()
    {
        if (hp <= 0 && !scoreAdded)
        {
            smoke = Instantiate(smokePrefab,
                                        this.transform.position,
                                        this.transform.rotation);
            GameController.GameControllerInstance.score += GameController.GameControllerInstance.destroyBossScore;
            scoreAdded = true;
            Destroy(smoke, 4f);
        }
    }
}
