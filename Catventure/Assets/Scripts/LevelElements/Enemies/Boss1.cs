using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Boss1 : MonoBehaviour
{
    public int waitTimeUntilNextJump = 5;
    public int stunDuration = 1;
    public bool jumpAround = true;
    Enemy enemy;
    private int currentStep;

    // Start is called before the first frame update
    public void Start()
    {
        enemy = GetComponent<Enemy>();
        StartCoroutine(JumpTimer());
    }

    void FixedUpdate()
    {
        if (enemy.currentHealth <= 0 && GetComponent<Boss3>() == null)
        {
            enemy.playerManager.currentCookies += enemy.cookieAmount;
            enemy.playerManager.level1Finished = true;
            SceneManager.LoadScene("Map");
        }
    }

    public void Stunned()
    {
        Debug.Log("Boss1Stunned");
        StartCoroutine(BossStunned());
    }

    public static IEnumerator AnimateJump(GameObject go, int step, float height, float time = 1.0f)
    {
        float distance = 0;

        if (step == 0 || step == 3)
        {
            distance = -10;
        }

        else
        {
            distance = +10;
        }

        Vector3 basePos = go.transform.position;
        float x1 = 0;
        float x2 = distance;
        float x3 = (x1 + x2) / 2.0f;
        float a = height / ((x3 - x1) * (x3 - x2));

        for (float passed = 0.0f; passed < time;)
        {
            passed += Time.deltaTime;
            float f = passed / time;
            if (f > 1) f = 1;

            float x = distance * f;
            float y = a * (x - x1) * (x - x2);
            go.transform.position = Change.X(go.transform.position, basePos.x + x);
            go.transform.position = Change.Y(go.transform.position, basePos.y + y);

            yield return 0;
        }
    }

    IEnumerator BossStunned()
    {
        jumpAround = false;
        yield return new WaitForSeconds(stunDuration);
        jumpAround = true;
        StartCoroutine(JumpTimer());
    }

    IEnumerator JumpTimer()
    {
        while (jumpAround)
        {
            yield return new WaitForSeconds(waitTimeUntilNextJump);
            StartCoroutine(AnimateJump(gameObject, currentStep, 5));
            if (currentStep != 3)
            {
                currentStep++;
            }
            else
            {
                currentStep = 0;
            }
            //yield return new WaitForSeconds(waitTimeUntilNextJump);
            //StartCoroutine(AnimateJump(gameObject, +10, 5));
            //yield return new WaitForSeconds(waitTimeUntilNextJump);
            //StartCoroutine(AnimateJump(gameObject, +10, 5));
            //yield return new WaitForSeconds(waitTimeUntilNextJump);
            //StartCoroutine(AnimateJump(gameObject, -10, 5));
        }

        yield break;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(HitAnimation(enemy.anim));
            col.gameObject.GetComponent<PlayerManager>().GotDamaged(enemy.damage);
        }
    }

    IEnumerator HitAnimation(Animator anim)
    {
        anim.SetBool("Hit",true);
        yield return new WaitForSeconds(1);
        anim.SetBool("Hit", false);
    }

    public void StopJumping()
    {
        StopAllCoroutines();
    }
}
