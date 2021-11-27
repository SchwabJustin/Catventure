using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : MonoBehaviour
{
    public float jumpPhaseDuration;
    public float jumpStunPhaseDuration;
    public float barkPhaseDuration;
    public float barkStunPhaseDuration;
    public float throwingPhaseDuration;
    public float throwingStunPhaseDuration;

    private Enemy enemy;
    public GameObject tpParticleSystemPrefab;
    private Boss1 boss1;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        boss1 = GetComponent<Boss1>();
        while (enemy.currentHealth > 0)
        {
            StartCoroutine(Teleportation(new Vector3(886, -2.1F, 0)));
            boss1.enabled = true;
            StartCoroutine(Timer(jumpPhaseDuration));
            boss1.enabled = false;
            StartCoroutine(Timer(jumpStunPhaseDuration));

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
