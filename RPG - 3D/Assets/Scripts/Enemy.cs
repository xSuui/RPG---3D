using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    public float ColliderRadius;

    bool IsReady;
    public bool PlayerIsAlive;

    public Image healthBar;

    public GameObject canvasBar;

    void Start()
    {
        anim = GetComponent<Animator>();
        cap = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();

        CurrentHealth = TotalHealth;
        PlayerIsAlive = true;
    }

    void Update()
    {
        if(CurrentHealth > 0)
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
                    StartCoroutine("Attack");
                    LookTarget();
                }

                if (distance >= agent.stoppingDistance)
                {
                    anim.SetBool("attacking", false);
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
    }

    IEnumerator Attack()
    {
        if (!IsReady && PlayerIsAlive && !anim.GetBool("hiting"))
        {
            IsReady = true;
            anim.SetBool("attacking", true);
            anim.SetBool("walking", false);
            anim.SetInteger("transition", 1);
            yield return new WaitForSeconds(1f);
            GetEnemy();
            yield return new WaitForSeconds(1.7f);
            IsReady = false;
        }
        if(!PlayerIsAlive)
        {
            anim.SetInteger("transition", 0);
            anim.SetBool("walking", false);
            anim.SetBool("attacking", false);
            agent.isStopped = true;
        }
    }

    void GetEnemy()
    {
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * ColliderRadius), ColliderRadius))
        {
            if (c.gameObject.CompareTag("Player"))
            {
                c.gameObject.GetComponent<Player>().GetHit(25f);
                PlayerIsAlive = c.gameObject.GetComponent<Player>().IsAlive;
            }
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

        healthBar.fillAmount = CurrentHealth / TotalHealth;

        if (CurrentHealth > 0)
        {
            StopCoroutine("Attack");
            anim.SetInteger("transition", 3);
            anim.SetBool("hiting", true);
            StartCoroutine(RecoveryFromHit());
        }
        else
        {
            canvasBar.gameObject.SetActive(false);
            anim.SetInteger("transition", 4);
            cap.enabled = false;
        }
    }

    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(1f);
        anim.SetInteger("transition", 0);
        anim.SetBool("hiting", false);
        IsReady = false;
    }

}
