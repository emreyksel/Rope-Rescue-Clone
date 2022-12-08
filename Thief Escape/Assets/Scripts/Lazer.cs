using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lazer : MonoBehaviour
{
    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        transform.DOMoveY(0, 3).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        LazerPosition();
    }

    private void LazerPosition()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 7);

        if (hit && hit.transform.CompareTag("Obstacle")) 
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, hit.point);
        }
        else
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position + new Vector3(7, 0, 0));
        }

        if (hit && hit.transform.TryGetComponent(out Thief thief)) 
        {
            thief.Crash();
            hit.rigidbody.AddForce(new Vector2(Random.Range(2, 4), Random.Range(2, 4)), ForceMode2D.Impulse);
        }
    }
}
