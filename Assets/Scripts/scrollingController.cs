using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollingController : MonoBehaviour {

    private Rigidbody2D rb2d;

 
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        if (Warrior.warriorInstance.clickLeft) moveRight();
        else if (Warrior.warriorInstance.clickRight) moveLeft();
        else stop();
    }

    void moveRight()
    {
        rb2d.velocity = new Vector2(1f, 0);
    }

    void moveLeft()
    {
        rb2d.velocity = new Vector2((-1) * 1f, 0);
    }

    void stop()
    {
        rb2d.velocity = new Vector2(0, 0);
    }
    
}
