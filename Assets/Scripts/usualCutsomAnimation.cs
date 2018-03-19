using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class usualCutsomAnimation : MonoBehaviour
{

    public Sprite[] sprites;
    private float delay = 0.08f;
    private int current = 0;

    void Awake()
    {

    }

    private void Start()
    {
        StartCoroutine("animateJatPack");
    }

    IEnumerator animateJatPack()
    {
        while (true)
        {
            this.GetComponent<SpriteRenderer>().sprite = sprites[current];
            yield return new WaitForSeconds(delay);
            if (current == sprites.Length - 1) { current = 0; }
            else { current++; }
        }
    }

}
