using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateBuildingRow : MonoBehaviour {

    public BoxCollider2D groundCollider;
    private float groundHorizontalLength;

    // Use this for initialization
    void Start ()
    {
        groundHorizontalLength = groundCollider.size.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.x < -groundHorizontalLength)
        {
            RepositionBackgroundRight();
        }
        if (transform.position.x > groundHorizontalLength)
        {
            RepositionBackgroundLeft();
        }
    }

    private void RepositionBackgroundRight()
    {
        Vector3 groundOffSet = new Vector3(groundHorizontalLength, 0, 0);
        transform.position = (Vector3)transform.position + groundOffSet;
    }

    private void RepositionBackgroundLeft()
    {
        Vector3 groundOffSet = new Vector3(groundHorizontalLength, 0, 0);
        transform.position = (Vector3)transform.position - groundOffSet;
    }
}
