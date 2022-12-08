using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Thief : MonoBehaviour
{
    private Rigidbody2D rb;

    public bool move = false; 
    private static int index; 
    private int currentPos;
    private bool arrived = false; 
    private float speed = 5f;
    private bool once = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        index = 0;
    }

    private void Update()
    {
        if (move && !arrived)
        {
            Escape();
        } 
    }

    public void Escape()
    {
        if (currentPos != Hook.instance.ropePositions.Count)
        {
            if (transform.position != Hook.instance.ropePositions[currentPos])
            {
                Vector3 pos = Vector3.MoveTowards(transform.position, Hook.instance.ropePositions[currentPos], speed * Time.deltaTime);
                transform.position = pos;
            }
            else
            {
                currentPos++;
            }
        }
        else 
        {
            GameManager.instance.arrivedThief++;
            GameManager.instance.UpdateText();
            index++;
            Hook.instance.activeThief--;
            arrived = true;
            transform.DOMove(Hook.instance.finishPlatform.transform.GetChild(index).position, 0.5f);
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!once && collision.gameObject.CompareTag("Saw")) 
        {
            Crash();

            Vector2 force = (rb.position - collision.contacts[0].point).normalized;
            rb.AddForce(new Vector2(force.x * Random.Range(15, 20), force.y * Random.Range(5, 7)), ForceMode2D.Impulse);
        }
    }

    public void Crash()
    {
        if (!once)
        {
            once = true;
            GameManager.instance.thiefs.Remove(gameObject);
            Hook.instance.currentThief--;
            GameManager.instance.CheckThiefCount();
            move = false;
            rb.gravityScale = 1;
            Hook.instance.activeThief--;
        }
    }
}
