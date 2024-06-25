using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float TotalHealth = 100;
    public float CurrentHealth;
    public float AttackDamage;
    public float MovementSpeed;

    private Animator anim;
    private CapsuleCollider cap;

    public float lookRadius = 10f;
    public Transform target;
    public NavMeshAgent agent;

    bool IsReady;

    void Start()
    {
        anim = GetComponent<Animator>();
        cap = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();

        CurrentHealth = TotalHealth;
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= lookRadius)
        {
            agent.isStopped = false;
            if(!anim.GetBool("attacking"))
            {
                agent.SetDestination(target.position);
                anim.SetInteger("transition", 2);
                anim.SetBool("walking", true);
            }

            if (distance <= agent.stoppingDistance)
            {
                StartCoroutine(Attack());
                LookTarget();
            }
        }
        else
        {
            anim.SetInteger("transition", 0);
            anim.SetBool("walking", false);
            anim.SetBool("attacking", false);
            agent.isStopped = true;
        }
    }

    IEnumerator Attack()
    {
        if (!IsReady)
        {
            IsReady = true;
            anim.SetBool("attacking", true);
            anim.SetBool("walking", false);
            anim.SetInteger("transition", 1);
            yield return new WaitForSeconds(1f);
            IsReady = false;
        }
    }

    void LookTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void GetHit(float Damage)
    {
        CurrentHealth -= Damage;
        if (CurrentHealth > 0)
        {
            anim.SetInteger("transition", 3);
            StartCoroutine(RecoveryFromHit());
        }
        else
        {
            anim.SetInteger("transition", 4);
            cap.enabled = false;
        }
    }

    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(1f);
        anim.SetInteger("transition", 0);
    }

    void Die()
    {
        if(CurrentHealth <= 0)
        {
            anim.SetInteger("transition", 4);
            //Destroy(gameObject, 2f);
        }
    }
}
