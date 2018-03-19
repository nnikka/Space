using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{

    public static Warrior warriorInstance;

    public SpriteRenderer warriorIdle;
    public Sprite[] sprites;

    public float velocity = 0.05f;
    private Vector3 currPos;

    private bool clickUp;
    private bool clickDown;
    public bool clickLeft;
    public bool clickRight;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    private float bulletVelocity = 3.5f;
    private float fireRate = 0.2f;
    private float lastShot = 0f;

    private int hp = 1;
    public GameObject explosionPrefab;
    private GameObject explosion;

    private void Awake()
    {
        if (warriorInstance == null)
        {
            warriorInstance = this;
        }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) clickUp = true;
        else clickUp = false;
        if (Input.GetKey(KeyCode.S)) clickDown = true;
        else clickDown = false;
        if (Input.GetKey(KeyCode.A)) clickLeft = true;
        else clickLeft = false;
        if (Input.GetKey(KeyCode.D)) clickRight = true;
        else clickRight = false;
        if (Input.GetKey(KeyCode.Space)) Fire();

        if (hp <= 0) gameOver();
    }

    void FixedUpdate()
    {
        if (clickUp &&  hp > 0)
        {
            moveUp();
            jatPackVolumeUp();
        }
        if (clickDown && hp > 0)
        {
            moveDown();
            jatPackVolumeUp();
        }
        if (clickLeft && hp > 0)
        {
            moveLeft();
            jatPackVolumeUp();
        }
        if (clickRight && hp > 0)
        {
            moveRight();
            jatPackVolumeUp();
        }
        if (!clickRight && !clickDown && !clickLeft && !clickUp)
        {
            jatPackVolumeDown();
        }
            
    }

    void moveUp()
    {
        currPos = warriorInstance.transform.position;
        currPos.y += velocity;
        warriorInstance.transform.position = currPos;
    }

    void moveDown()
    {
        currPos = warriorInstance.transform.position;
        currPos.y -= velocity;
        warriorInstance.transform.position = currPos;
    }

    void moveLeft()
    {
        currPos = warriorInstance.transform.position;
        currPos.x -= velocity;
        warriorInstance.transform.position = currPos;
    }

    void moveRight()
    {
        currPos = warriorInstance.transform.position;
        currPos.x += velocity;
        warriorInstance.transform.position = currPos;
    }

    void jatPackVolumeUp()
    {
        this.GetComponent<AudioSource>().volume = 1;
    }

    void jatPackVolumeDown()
    {
        this.GetComponent<AudioSource>().volume = 0.2f;
    }

    void Fire()
    {
        if (Time.time > fireRate + lastShot)
        {
            GameObject bullet = Instantiate(bulletPrefab,
                                         bulletSpawn.position,
                                         bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletVelocity, 0);
            lastShot = Time.time;
            StartCoroutine("animateWarrior");
            Destroy(bullet, 2.0f);
        }
    }

    IEnumerator animateWarrior()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            warriorIdle.GetComponent<SpriteRenderer>().sprite = sprites[i];
            yield return new WaitForSeconds(fireRate / sprites.Length);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            explosion = Instantiate(explosionPrefab,
                                        warriorInstance.transform.position,
                                        warriorInstance.transform.rotation);
            hp--;
            warriorInstance.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = true;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "WarriorDied")
        {
            GameController.GameControllerInstance.gameOver = true;
        }
    }

    private void gameOver()
    {
        transform.Rotate(0, 0, -0.4f);
        explosion.transform.position = new Vector3(warriorInstance.transform.position.x, 
                                                    warriorInstance.transform.position.y, -9f);
        explosion.transform.rotation = warriorInstance.transform.rotation;
        warriorInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -2f);
        Destroy(warriorInstance.gameObject, 7f);
    }
}
