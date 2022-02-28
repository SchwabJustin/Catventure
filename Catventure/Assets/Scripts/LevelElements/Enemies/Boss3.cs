using System.Collections;
using UnityEngine;

public enum Boss3Phases
{
    Jump = 0,
    Bark = 1,
    Throw1 = 2,
    Throw2 = 3
}

public class Boss3 : MonoBehaviour
{
    public float jumpPhaseDuration;
    public float jumpStunPhaseDuration;
    public float barkPhaseDuration;
    public float barkStunPhaseDuration;
    public float halfThrowPhaseDuration;
    public float throwStunPhaseDuration;
    public Vector3 jumpPhaseStartPosition = new Vector3(886, -2.1F, 0);
    public Vector3 barkPhaseStartPosition = new Vector3(886, 9.4F, 0);
    public Vector3 throwPhaseStartPosition1 = new Vector3(860, -2, 0);
    public Vector3 throwPhaseStartPosition2 = new Vector3(910, -2, 0);

    public float barkRotationPerSecond = 45;

    private GameObject barkGO;
    private float currentStunTime;
    private bool stunned;
    public Boss3Phases currentPhase;
    private Enemy enemy;
    private Bark barkAttack;
    public GameObject tpParticleSystemPrefab;
    private Boss1 boss1;
    public GameObject bossProjectilePrefab;
    public GameObject shootPosition;
    private GameObject player;
    private bool firstJumpPhase = true;

    private Vector3 scale;
    private Animator anim;

    private bool dead;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scale = transform.localScale;
        enemy = GetComponent<Enemy>();
        boss1 = GetComponent<Boss1>();
        barkAttack = GetComponentInChildren<Bark>();
        barkAttack.damage = enemy.damage;
        barkAttack.barkRotationPerSecond = barkRotationPerSecond;
        barkGO = barkAttack.gameObject.transform.parent.gameObject;
        barkAttack.barkGO = barkGO;
        barkGO.SetActive(false);
        anim = GetComponent<Animator>();
        currentPhase = Boss3Phases.Jump;
        Phase1();
    }

    void FixedUpdate()
    {
        if (!dead)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void Dead()
    {
        dead = true;
        StartCoroutine(Dying(enemy.playerManager));
    }

    IEnumerator Dying(PlayerManager playerManager)
    {
        playerManager.level3Finished = true;
        yield return new WaitForSeconds(1);
        playerManager.StartLevel("WinningScreen");
    }

    public void Stunned()
    {
        switch (currentPhase)
        {
            case Boss3Phases.Jump:
                currentStunTime = jumpStunPhaseDuration;
                break;

            case Boss3Phases.Bark:
                currentStunTime = barkStunPhaseDuration;
                break;

            case Boss3Phases.Throw1:
                currentStunTime = throwStunPhaseDuration;
                break;

            case Boss3Phases.Throw2:
                currentStunTime = throwStunPhaseDuration;
                break;

            default:
                currentStunTime = 0;
                break;
        }

        if (currentStunTime == 0) return;
        stunned = true;
        anim.SetBool("Cooldown",true);
        StartCoroutine(Timer(currentStunTime));
        stunned = false;
        anim.SetBool("Cooldown", false);
    }

    void Phase1()
    {
        transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        if (!firstJumpPhase)
        {
            StartCoroutine(Teleportation(jumpPhaseStartPosition));
        }
        firstJumpPhase = false;
        boss1.jumpAround = true;
        boss1.enabled = true;
        boss1.Start();
        currentPhase = Boss3Phases.Jump;
        StartCoroutine(Timer(jumpPhaseDuration));

    }

    void Phase2()
    {
        boss1.StopJumping();
        boss1.enabled = false;
        StartCoroutine(Teleportation(barkPhaseStartPosition));
        currentPhase = Boss3Phases.Bark;
        barkGO.transform.Rotate(Vector3.forward, Random.Range(1, 360));
        barkGO.SetActive(true);
        StartCoroutine(Timer(barkPhaseDuration));
    }

    void Phase3()
    {
        barkGO.SetActive(false);
        StartCoroutine(Teleportation(throwPhaseStartPosition1));
        currentPhase = Boss3Phases.Throw1;
        StartCoroutine(Timer(halfThrowPhaseDuration));
        Shoot(player.transform);
    }
    void Phase4()
    {
        StartCoroutine(Teleportation(throwPhaseStartPosition2));
        currentPhase = Boss3Phases.Throw2;
        transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        StartCoroutine(Timer(halfThrowPhaseDuration));
    }

    public void Shoot(Transform playerTransform)
    {
        anim.SetBool("Hit", true);
        var projectile = Instantiate(bossProjectilePrefab, shootPosition.transform.position, Quaternion.Euler(0,0,0));
        projectile.GetComponent<EnemyProjectile>().damage = enemy.damage;
        projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.MoveTowards(shootPosition.transform.position, playerTransform.position, 3) * transform.localScale.x);
        StartCoroutine(ResetCooldown());
    }
    IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(halfThrowPhaseDuration / 3);
        anim.SetBool("Hit", false);
        if (currentPhase == Boss3Phases.Throw1 || currentPhase == Boss3Phases.Throw2)
            Shoot(player.transform);
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        switch (currentPhase)
        {
            case Boss3Phases.Jump:
                Phase2();
                break;
            case Boss3Phases.Bark:
                Phase3();
                break;
            case Boss3Phases.Throw1:
                Phase4();
                break;
            case Boss3Phases.Throw2:
                Phase1();
                break;
        }
    }

    IEnumerator Teleportation(Vector3 newPos)
    {
        GameObject currentParticleSystem = Instantiate(tpParticleSystemPrefab, transform.position, transform.rotation);
        yield return new WaitForSeconds(currentParticleSystem.GetComponent<ParticleSystem>().main.duration / 2);
        transform.position = newPos;
        Destroy(currentParticleSystem);
        yield return new WaitForSeconds(currentParticleSystem.GetComponent<ParticleSystem>().main.duration / 2);
        yield return new WaitForSeconds(0.5f);
    }
}
