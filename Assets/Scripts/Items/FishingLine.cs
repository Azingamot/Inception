using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FishingLine : MonoBehaviour
{
    [SerializeField] private bool showStartingPoint = true;
    [SerializeField] private Transform startPoint;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private GameObject lineObject;
    [SerializeField] private SpriteRenderer baitRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject particlesObject;

    private LineRenderer lineRenderer;
    private bool isCasted = false;
    private float flightTime = 1f;
    private Vector2 targetPosition;

    public bool IsCasted => isCasted;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetLine();
    }

    private void SetLine()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.01f;
    }

    private void FixedUpdate()
    {
        if (lineRenderer.enabled)
            DrawLine();
    }
    
    private void DrawLine()
    {
        lineRenderer.SetPosition(0, startPoint.transform.position);
        lineRenderer.SetPosition(1, lineObject.transform.position);
    }

    public void Cast(Vector2 endPoint, Sprite baitSprite, float flightTime = 1)
    {
        baitRenderer.sprite = baitSprite;
        lineObject.SetActive(true);
        lineRenderer.enabled = true;

        targetPosition = endPoint;

        this.flightTime = flightTime;

        MoveBait(startPoint.transform.position, endPoint, flightTime);

        StartCoroutine(ReachDestination(endPoint, 0.1f));

        isCasted = true;
    }

    public void Uncast()
    {
        MoveBait(rb.transform.position, startPoint.transform.position, flightTime);

        StartCoroutine(ReachDestination(startPoint.transform.position, 0.25f, true));
    }

    private void MoveBait(Vector2 startPoint, Vector2 endPoint, float flightTime)
    {
        rb.position = startPoint;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = gravityScale;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.None;

        Vector2 p0 = startPoint;
        Vector2 p1 = endPoint;
        float T = flightTime;
        float g = Physics2D.gravity.y * gravityScale;

        float vx = (p1.x - p0.x) / T;
        float vy = (p1.y - p0.y - 0.5f * g * T * T) / T;

        rb.linearVelocity = new Vector2(vx, vy);
    }

    private IEnumerator ReachDestination(Vector3 destination, float stopDistance, bool deactivateOnFinish = false)
    {
        while (Vector2.Distance(rb.transform.position, destination) > stopDistance)
        {
            yield return null;
        }

        StopAtTarget();

        if (deactivateOnFinish)
        {
            lineObject.SetActive(false);
            isCasted = false;
            lineRenderer.enabled = false;
        }
        else 
            ObjectPoolController.SpawnObject(particlesObject, destination, Quaternion.identity);
    }

    void StopAtTarget()
    {
        rb.position = targetPosition;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnDrawGizmos()
    {
        if (showStartingPoint)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(startPoint.transform.position, 0.25f);
        }
    }
}
