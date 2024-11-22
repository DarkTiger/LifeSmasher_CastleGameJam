using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float attackDamage;
    [SerializeField] float attackDuration;
    [SerializeField] Vector2 shakeForce = new Vector2(0.05f, 0.025f);
    public float health = 100f;

    NavMeshAgent agent = null;
    Animator animator = null;
    CameraShake[] cameraShakes; 
    Rigidbody rb = null;
    Transform target = null;
    Vector3 lastPosition = Vector3.zero;
    float attackRate = 3f;
    float lastAttackTime = 0f;
    bool dead = false;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        target = FindFirstObjectByType<Player>().transform;
        cameraShakes = FindObjectsByType<CameraShake>(FindObjectsSortMode.None);
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (dead) return;

        if (lastAttackTime + attackDuration < Time.time)
        {
            agent.SetDestination(target.position);

            float speed = (lastPosition - transform.position).magnitude;
            animator.SetFloat("Speed", speed);
        }              

        float targetDistance = (target.position - transform.position).magnitude;
        if (targetDistance < 1.5f)
        {
            if (lastAttackTime + attackRate < Time.time)
            {
                transform.LookAt(target.transform);
                animator.SetTrigger("Attack");
                lastAttackTime = Time.time;
            }
        }

        lastPosition = transform.position;
    }

    public void SetDamage(float damage, float delay)
    {
        if (dead) return;

        StartCoroutine(SetDamageCO(damage, delay));
    }

    IEnumerator SetDamageCO(float damage, float delay)
    {
        yield return new WaitForSeconds(delay);

        health -= damage;

        if (health <= 0f)
        {
            animator.SetBool("IsDead", true);
            animator.SetTrigger("Die");
            GetComponent<CapsuleCollider>().enabled = false;
            agent.enabled = false;
            
            StartCoroutine(RemoveDeadBody());
            if (!dead)
            {
                dead = true;
                GameManager.Instance.EnemyDead();
            }
            
            enabled = false;
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    //CALLED BY ANIMATION
    void Attack()
    {
        if ((target.position - transform.position).magnitude < 1.5f)
        {
            PlayShakes();
            GameManager.Instance.SetPlayerDamage(attackDamage);
        }
    }

    void PlayShakes()
    {
        for (int i = 0; i < cameraShakes.Length; i++)
        {
            cameraShakes[i].Shake(shakeForce.x, shakeForce.y, 0f);
        }
    }

    IEnumerator RemoveDeadBody()
    {
        yield return new WaitForSeconds(10);

        Destroy(gameObject);
    }
}
