using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Bow : MonoBehaviour
{
    public GameObject arrow;

    public float launchForce;
    public float maxLaunchForce;
    public float launchForceAddUp;
    public Transform shotPoint;

    public GameObject markerPoint;
    private GameObject[] points;
    public int numberOfPoints;
    public float spaceBetweenPoints;
    public Vector2 direction;
    private bool shot;
    private bool doubleShot;
    private bool poisonShot;
    private bool burnShot;
    private bool paralyzeShot;
    private bool shooting;
    private bool arrowRain;

    public Sprite poisonSprite;
    public Sprite burnSprite;
    public Sprite paralyzeSprite;

    private PlayerMovement playerMovement;
    private PlayerManager playerManager;
    private GameObject parentPoint;
    public Vector2 mousePosition;

    public GameObject arrowRainGo;
    public int shotCooldown = 1;
    public int doubleShotCooldown = 20;
    public int arrowRainCooldown = 30;
    public int poisonShotCooldown = 15;
    public int burnShotCooldown = 25;
    public int paralyzeShotCooldown = 30;


    private Image shotImg;
    private Image doubleShotImg;
    private Image poisonShotImg;
    private Image burnShotImg;
    private Image paralyzeShotImg;
    private Image arrowRainImg;
    private void Start()
    {
        parentPoint = GameObject.Find("ParentPoint");
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

        shotImg = GameObject.Find("Präziser Schuss Cooldown").GetComponent<Image>();
        poisonShotImg = GameObject.Find("Vergiften Cooldown").GetComponent<Image>();
        doubleShotImg = GameObject.Find("Doppelter Treffer Cooldown").GetComponent<Image>();
        burnShotImg = GameObject.Find("Verbrennen Cooldown").GetComponent<Image>();
        arrowRainImg = GameObject.Find("Pfeilregen Cooldown").GetComponent<Image>();
        paralyzeShotImg = GameObject.Find("Paralyse Cooldown").GetComponent<Image>();

        var skills = new List<SkillSO>();
        skills.AddRange(playerManager.learnedSkills);
        skills.AddRange(playerManager.multiSkillableSkills);
        skills = skills.Distinct().ToList();

        foreach (var skill in skills)
        {
            if (skill.name == "Präziser Schuss")
            {
                shotImg.fillAmount = 0;
            }
            if (skill.name == "Vergiften")
            {
                poisonShotImg.fillAmount = 0;
            }
            if (skill.name == "Doppelter Treffer")
            {
                doubleShotImg.fillAmount = 0;
            }
            if (skill.name == "Verbrennen")
            {
                burnShotImg.fillAmount = 0;
            }
            if (skill.name == "Pfeilregen")
            {
                arrowRainImg.fillAmount = 0;
            }
            if (skill.name == "Paralyse")
            {
                paralyzeShotImg.fillAmount = 0;
            }
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
        if (points.Length > 0)
        {
            for (int i = 0; i < numberOfPoints; i++)
            {
                points[i].transform.position = PointPosition(i * spaceBetweenPoints);
            }
        }

        if (Input.GetMouseButtonDown(1) && !poisonShot)
        {
            if (playerManager.learnedSkills.FirstOrDefault(x => x.name == "Vergiften") || playerManager.multiSkillableSkills.FirstOrDefault(x => x.name == "Vergiften"))
            {
                poisonShot = true;
                StartCoroutine(PoisonShot());
            }

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!doubleShot)
            {
                doubleShot = true;
                StartCoroutine(DoubleShot());
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && !burnShot)
        {
            if (playerManager.learnedSkills.FirstOrDefault(x => x.name == "Verbrennen") || playerManager.multiSkillableSkills.FirstOrDefault(x => x.name == "Verbrennen"))
            {
                burnShot = true;
                StartCoroutine(BurnShot());
            }

        }

        if (Input.GetKeyDown(KeyCode.E) && !arrowRain)
        {
            if (playerManager.learnedSkills.FirstOrDefault(x => x.name == "Pfeilregen") || playerManager.multiSkillableSkills.FirstOrDefault(x => x.name == "Pfeilregen"))
            {
                arrowRain = true;
                StartCoroutine(ArrowRain());
            }

        }

        if (Input.GetKeyDown(KeyCode.R) && !paralyzeShot)
        {
            if (playerManager.learnedSkills.FirstOrDefault(x => x.name == "Paralyse") || playerManager.multiSkillableSkills.FirstOrDefault(x => x.name == "Paralyse"))
            {
                paralyzeShot = true;
                StartCoroutine(ParalyzeShot());
            }

        }
    }

    IEnumerator PoisonShot()
    {

        GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
        newArrow.GetComponent<SpriteRenderer>().sprite = poisonSprite;
        newArrow.GetComponent<Arrow>().player = playerMovement.gameObject;
        newArrow.GetComponent<Arrow>().poisonShot = true;
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * maxLaunchForce;

        CoolDownSprite(poisonShotCooldown, poisonShotImg);
        yield return new WaitForSeconds(poisonShotCooldown);
        poisonShot = false;

    }

    IEnumerator BurnShot()
    {
        GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
        newArrow.GetComponent<SpriteRenderer>().sprite = burnSprite;
        newArrow.GetComponent<Arrow>().player = playerMovement.gameObject;
        newArrow.GetComponent<Arrow>().burnShot = true;
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * maxLaunchForce;

        CoolDownSprite(burnShotCooldown, burnShotImg);
        yield return new WaitForSeconds(burnShotCooldown);
        burnShot = false;

    }
    IEnumerator ArrowRain()
    {
        if (playerManager.learnedSkills.FirstOrDefault(x => x.name == "Pfeilregen"))
        {
            var spawnPos = shotPoint.position + new Vector3(0, 2, 0);
            GameObject newArrowRain = Instantiate(arrowRainGo, spawnPos, quaternion.Euler(Vector3.zero));
            if (mousePosition.x < transform.position.x)
            {
                newArrowRain.transform.localScale = new Vector3(newArrowRain.transform.localScale.x * -1,
                    newArrowRain.transform.localScale.y, newArrowRain.transform.localScale.z);
            }

            CoolDownSprite(arrowRainCooldown, arrowRainImg);
            yield return new WaitForSeconds(arrowRainCooldown);
            arrowRain = false;
        }
    }

    IEnumerator ParalyzeShot()
    {

        GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
        newArrow.GetComponent<SpriteRenderer>().sprite = paralyzeSprite;
        newArrow.GetComponent<Arrow>().player = playerMovement.gameObject;
        newArrow.GetComponent<Arrow>().paralyzeShot = true;
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * maxLaunchForce;

        CoolDownSprite(paralyzeShotCooldown, paralyzeShotImg);
        yield return new WaitForSeconds(paralyzeShotCooldown);
        paralyzeShot = false;

    }

    IEnumerator DoubleShot()
    {
        if (playerManager.learnedSkills.FirstOrDefault(x => x.name == "Doppelter Treffer") ||
            playerManager.multiSkillableSkills.FirstOrDefault(x => x.name == "Doppelter Treffer"))
        {
            GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
            newArrow.GetComponent<Arrow>().player = playerMovement.gameObject;
            newArrow.GetComponent<Arrow>().doubleShotArrow = true;
            newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * maxLaunchForce;
            yield return new WaitForSeconds(0.2F);
            newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
            newArrow.GetComponent<Arrow>().player = playerMovement.gameObject;
            newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * maxLaunchForce;



            CoolDownSprite(doubleShotCooldown, doubleShotImg);
            yield return new WaitForSeconds(doubleShotCooldown);
            doubleShot = false;
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
        newArrow.GetComponent<Arrow>().player = playerMovement.gameObject;
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        StartCoroutine(ShotCooldown());
    }

    private IEnumerator ShotCooldown()
    {
        CoolDownSprite(shotCooldown, shotImg);
        yield return new WaitForSeconds(shotCooldown);
        shot = false;
        shooting = false;
    }

    public void CoolDownSprite(int cooldownTime, Image img)
    {
        StartCoroutine(SpriteFillAmount(cooldownTime, img));
    }

    public IEnumerator SpriteFillAmount(float duration, Image img)
    {

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            img.fillAmount = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }
        img.fillAmount = 0;
    }

    Vector2 PointPosition(float t)
    {
        Vector2 position = (Vector2)shotPoint.position + (direction.normalized * launchForce * t) +
                           0.5f * Physics2D.gravity * (t * t);
        return position;
    }
}
