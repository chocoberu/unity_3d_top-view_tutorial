using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(UnityEngine.AI.NavMeshAgent))]

public class Enemy : LivingEntity {

    public enum State { Idle, Chasing, Attacking }; //적의 상태를 나타냄
    UnityEngine.AI.NavMeshAgent pathfinder; // Player 추적을 위한 패스파인더

    State currentState;

    Transform target; // 공격 대상, Player
    Material skinMaterial;
    Color originalColor;
    LivingEntity targetEntity;

    float attackDistanceThreshold = .5f; // 공격 거리 임계값, 공격할 수 있는 한계거리 (1.5m)
    float timeBetweenAttacks = 1;

    float nextAttackTime;
    float myCollisionRadius; // 적의 충돌반경 반지름
    float targetCollisionRadius; // 플레이어의 충돌반경 반지름
    float damage = 1;
    bool hasTarget;

    // Use this for initialization
    protected override void Start () {
        base.Start(); // base의 Start()를 호출
        pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent>(); // 패스파인더의 컴포넌트를 가져온다
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Chasing;
            hasTarget = true;
            target = GameObject.FindGameObjectWithTag("Player").transform; // target을 Player로 지정
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath()); // 코루틴 시작 (플레이어 추적)
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }

            }
        }
    }
    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }
    IEnumerator Attack()
    {

        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= 0.5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        skinMaterial.color = originalColor;
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }
    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
