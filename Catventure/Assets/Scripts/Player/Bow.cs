using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject arrow;

    public float launchForce;
    public float maxLaunchForce;
    public float launchForceAddUp;
    public float cooldown;
    public Transform shotPoint;

    public GameObject markerPoint;
    private GameObject[] points;
    public int numberOfPoints;
    public float spaceBetweenPoints;
    private Vector2 direction;
    private bool shot;
    private bool shooting;

    private void Start()
    {
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(markerPoint, shotPoint.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePosition - bowPosition;
        transform.right = direction;
        if (Input.GetMouseButtonDown(0) && !shot)
        {
            shooting = true;
            StartCoroutine(IncreaseLaunchForce());
        }
        if (Input.GetMouseButtonUp(0) && !shot && shooting)
        {
            shot = true;
            Shoot();
            launchForce = 1;
        }
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i].transform.position = PointPosition(i * spaceBetweenPoints);
        }
    }

    private IEnumerator IncreaseLaunchForce()
    {
        while (!shot)
        {
            if (launchForce < maxLaunchForce)
            {
                launchForce += launchForceAddUp;
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                break;
            }
        }
    }

    void Shoot()
    {
        GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        StartCoroutine(AfterShot());
    }

    private IEnumerator AfterShot()
    {
        yield return new WaitForSeconds(cooldown);
        shot = false;
        shooting = false;
    }

    Vector2 PointPosition(float t)
    {
        Vector2 position = (Vector2) shotPoint.position + (direction.normalized * launchForce * t) +
                           0.5f * Physics2D.gravity * (t * t);
        return position;
    }
}
