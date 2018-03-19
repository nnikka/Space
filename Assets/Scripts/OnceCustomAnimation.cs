using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnceCustomAnimation : MonoBehaviour {

    public Sprite[] sprites;
    private float delay = 0.08f;

    void Awake()
    {

    }

    private void Start()
    {
        StartCoroutine("animateGo");
    }

    IEnumerator animateGo()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            this.GetComponent<SpriteRenderer>().sprite = sprites[i];
            yield return new WaitForSeconds(delay);
        }
    }
}
