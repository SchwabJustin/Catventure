using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

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
    private bool doubleShot;
    private bool shooting;
    private bool arrowRain;
    private PlayerMovement playerMovement;
    private PlayerManager playerManager;
    private GameObject parentPoint;
    public Vector2 mousePosition;
    public int shotCooldown = 1;
    public int doubleShotCooldown = 20;
    public int arrowRainCooldown = 30;

    private void Start()
    {
        parentPoint = new GameObject();
        parentPoint.name = "ParentPoint";
        playerMovement = transform.parent.gameObject.GetComponent<PlayerMovement>();
        playerManager = transform.parent.gameObject.GetComponent<PlayerManager>();
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(markerPoint, shotPoint.position, Quaternion.identity);
            points[i].transform.SetParent(parentPoint.transform);
        }
        foreach (GameObject point in points)
        {
            point.GetComponent<SpriteRenderer>().forceRenderingOff = true;
        }
    }

    private void Update()
    {
        Vector2 bowPosition = transform.position;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePosition - bowPosition;
        transform.right = direction;
        if (mousePosition.x >= transform.position.x)
        {
            transform.parent.localScale = new Vector3(Math.Abs(transform.parent.localScale.x), Math.Abs(transform.parent.localScale.y), Math.Abs(transform.parent.localScale.z)); ;
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), Math.Abs(transform.localScale.y), Math.Abs(transform.localScale.z));
        }
        if (mousePosition.x < transform.position.x)
        {
            transform.parent.localScale = new Vector3(-1 * Math.Abs(transform.parent.localScale.x), Math.Abs(transform.parent.localScale.y), Math.Abs(transform.parent.localScale.z));
            transform.localScale = new Vector3(-1 * Math.Abs(transform.localScale.x), -1 * Math.Abs(transform.localScale.y), Math.Abs(transform.localScale.z));
        }
        if (Input.GetMouseButtonDown(0) && !shot)
        {
            if (playerManager.learnedSkills.FirstOrDefault(x => x.name == "Präziser Schuss") || playerManager.multiSkillableSkills.FirstOrDefault(x => x.name == "Präziser Schuss"))
            {
                foreach (GameObject point in points)
                {
                    point.GetComponent<SpriteRenderer>().forceRenderingOff = false;
                }
                shooting = true;
                StartCoroutine(IncreaseLaunchForce());
            }
        }
        if (Input.GetMouseButtonUp(0) && !shot && shooting)
        {
            shot = true;
            Shoot();
            launchForce = 1;
            foreach (GameObject point in points)
            {
                point.GetComponent<SpriteRenderer>().forceRenderingOff = true;
            }
        }
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i].transform.position = PointPosition(i * spaceBetweenPoints);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!doubleShot)
            {
                doubleShot = true;
                StartCoroutine(DoubleShot());
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (!arrowRain)
            {
                arrowRain = true;
                StartCoroutine(ArrowRain());
            }
        }
    }

    IEnumerator ArrowRain()
    {
        yield return new WaitForSeconds(arrowRainCooldown);
    }

    IEnumerator DoubleShot()
    {
        if (playerManager.learnedSkills.FirstOrDefault(x => x.name == "Doppelter Treffer") || playerManager.multiSkillableSkills.FirstOrDefault(x => x.name == "Doppelter Treffer"))
        {
            GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
            newArrow.GetComponent<Arrow>().player = playerMovement.gameObject;
            newArrow.GetComponent<Arrow>().doubleShotArrow = true;
            newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * maxLaunchForce;
            yield return new WaitForSeconds(0.2F);
            newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
            newArrow.GetComponent<Arrow>().player = playerMovement.gameObject;
            newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * maxLaunchForce;

            yield return new WaitForSeconds(doubleShotCooldown);
            doubleShot = false;
        }

        Debug.Log("DoubleShot");
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
        newArrow.GetComponent<Arrow>().player = playerMovement.gameObject;
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        StartCoroutine(ShotCooldown());
    }

    private IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(shotCooldown);
        shot = false;
        shooting = false;
    }

    Vector2 PointPosition(float t)
    {
        Vector2 position = (Vector2)shotPoint.position + (direction.normalized * launchForce * t) +
                           0.5f * Physics2D.gravity * (t * t);
        return position;
    }
}
