using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public static Hook instance;

    private Rigidbody2D rb;
    private LineRenderer rope;

    public List<Vector3> ropePositions = new List<Vector3>();

    public GameObject finishPlatform;
    public Transform finishPoint;
    public Transform startPoint;

    public Gradient colorYellow;
    public Gradient colorGreen;
    public Gradient colorGrey;

    public LayerMask collMaskOut; 
    public LayerMask collMaskIn; 

    public float minDistance;
    private bool inFinishPoint = false; 
    [HideInInspector] public bool ready = false;
    [HideInInspector] public int currentThief;
    private float timer;
    [HideInInspector] public int activeThief;

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        rope = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        AddPosToRope(startPoint.position);
    }

    private void Update()
    {
        if (GameManager.instance.isGameOver)
            return;


        UpdateRopePositions();
        LastSegmentGoToPlayerPos();

        DetectCollisionEnter();
        if (ropePositions.Count > 2)
        {
            DetectCollisionExits();
        }

        ThiefMove();

        if (inFinishPoint && Input.GetMouseButtonUp(0))
        {
            ready = true;
            transform.position = finishPoint.position;
        }

        #region COLOR
        if (ready && activeThief != 0)
        {
            rope.colorGradient = colorGrey;
        }
        else if (ready && activeThief == 0)
        {
            rope.colorGradient = colorGreen;
        }
        #endregion
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            inFinishPoint = true;
            rope.colorGradient = colorGreen;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            inFinishPoint = false;
            ready = false;
            rope.colorGradient = colorYellow;
        }
    }

    public void ThiefMove()
    {
        timer += Time.deltaTime;

        if (ready && Input.GetMouseButton(0) && timer > 0.15f && currentThief != GameManager.instance.startThief && inFinishPoint)
        {
            timer = 0;
            GameManager.instance.thiefs[currentThief].GetComponent<Thief>().move = true;
            activeThief++;
            currentThief++;
        }
    }

    private void DetectCollisionEnter()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, rope.GetPosition(ropePositions.Count - 2), collMaskOut);
        if (hit && System.Math.Abs(Vector3.Distance(rope.GetPosition(ropePositions.Count - 2), hit.point)) > minDistance)
        {
            ropePositions.RemoveAt(ropePositions.Count - 1);
            AddPosToRope(hit.point);
        }
    }

    private void DetectCollisionExits()
    {
        RaycastHit2D hit1 = Physics2D.Linecast(transform.position, rope.GetPosition(ropePositions.Count - 3), collMaskIn);
        if (!hit1)
        {
            ropePositions.RemoveAt(ropePositions.Count - 2);
        }
    }

    private void AddPosToRope(Vector3 _pos)
    {
        ropePositions.Add(_pos);
        ropePositions.Add(transform.position); 
    }

    private void UpdateRopePositions()
    {
        rope.positionCount = ropePositions.Count;
        rope.SetPositions(ropePositions.ToArray());
    }

    private void LastSegmentGoToPlayerPos()
    {
        rope.SetPosition(rope.positionCount - 1, transform.position);
        ropePositions[ropePositions.Count - 1] = transform.position;
    }

    private void OnMouseDrag()
    {
        if (GameManager.instance.isGameOver)
            return;

        ready = false;

        if (activeThief == 0)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            rb.MovePosition(pos);
        }
    }

    private void OnMouseDown()
    {
        ready = false;
    }
}
