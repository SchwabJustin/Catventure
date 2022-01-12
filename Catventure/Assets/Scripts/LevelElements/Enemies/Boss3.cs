using System.Collections;
using UnityEngine;

public enum Boss3Phases
{
    Jump = 0,
    Bark = 1,
    Throw = 2,
    Teleport = 3
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
    public Vector3 barkPhaseStartPosition = new Vector3(885, 9.4F, 0);
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
    // Start is called before the first frame update
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        boss1 = GetComponent<Boss1>();
        barkAttack = GetComponentInChildren<Bark>();
        barkAttack.damage = enemy.damage;
        barkGO = barkAttack.gameObject.transform.parent.gameObject;
        barkGO.SetActive(false);
        currentPhase = Boss3Phases.Jump;
        Phases();
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

            case Boss3Phases.Throw:
                currentStunTime = jumpStunPhaseDuration;
                break;

            default:
                currentStunTime = 0;
                break;
        }

        if (currentStunTime == 0) return;
        stunned = true;
        StartCoroutine(Timer(currentStunTime));
        stunned = false;

    }

    void Phases()
    {
        while (enemy.currentHealth > 0)
        {
            currentPhase = Boss3Phases.Teleport;
            StartCoroutine(Teleportation(jumpPhaseStartPosition));
            boss1.enabled = true;
            currentPhase = Boss3Phases.Jump;
            StartCoroutine(Timer(jumpPhaseDuration));
            while (stunned) { }
            boss1.enabled = false;
            currentPhase = Boss3Phases.Teleport;
            StartCoroutine(Teleportation(barkPhaseStartPosition));
            barkGO.transform.Rotate(Vector3.forward, Random.Range(1, 360));
            barkGO.SetActive(true);
            Bark();
            currentPhase = Boss3Phases.Bark;
            StartCoroutine(Timer(barkPhaseDuration));
            while (stunned) { }
            barkGO.SetActive(false);
            currentPhase = Boss3Phases.Teleport;
            StartCoroutine(Teleportation(throwPhaseStartPosition1));
            currentPhase = Boss3Phases.Throw;
            StartCoroutine(Timer(halfThrowPhaseDuration));
            while (stunned) { }
            currentPhase = Boss3Phases.Teleport;
            StartCoroutine(Teleportation(throwPhaseStartPosition2));
            currentPhase = Boss3Phases.Throw;
            transform.localScale = new Vector3(-1, 1, 1);
            StartCoroutine(Timer(halfThrowPhaseDuration));
            while (stunned) { }
        }
    }

    void Bark()
    {
        while (currentPhase == Boss3Phases.Bark)
        {
            barkGO.transform.Rotate(Vector3.forward, barkRotationPerSecond * Time.deltaTime);
        }
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
    }

    IEnumerator Teleportation(Vector3 newPos)
    {
        GameObject currentParticleSystem = Instantiate(tpParticleSystemPrefab, transform.position, transform.rotation);
        yield return new WaitForSeconds(currentParticleSystem.GetComponent<ParticleSystem>().main.duration / 2);
        transform.position = newPos;
        yield return new WaitForSeconds(currentParticleSystem.GetComponent<ParticleSystem>().main.duration / 2);
    }
}
