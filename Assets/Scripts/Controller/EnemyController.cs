using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.DesignPattern;

public class EnemyController : TankController
{
    [SerializeField] float rangeOfShooting = 30f;
    Vector3 randPos;
    float rangeRand = 10;
    protected override void Start()
    {
        base.Start();
        GetRandomPosition();
    }
    void Update()
    {
        if (Player.Instance != null)
        {
            Vector3 gunDirection = -(Player.Instance.transform.position - transform.position).normalized;
            RotateGun(gunDirection);
            if (!IsWithinRangeOfPlayer())
            {
                Vector3 direction = Player.Instance.transform.position - transform.position;
                Move(direction.normalized);
            }
            else
            {/*
                if (Vector3.Distance(randPos, transform.position) < 1 || Vector3.Distance(randPos, transform.position)>10)
                {
                    while (true)
                    {
                        GetRandomPosition();
                        if (Vector3.Distance(randPos, Player.Instance.transform.position) < rangeOfShooting) break;
                    }
                }
                Move(randPos - transform.position);*/
                EnemyShoot();
            }
        }

    }
    protected virtual void EnemyShoot()
    {
        Shoot();
    }
    void GetRandomPosition()
    {
        randPos = transform.position + new Vector3(Random.Range(-rangeRand, rangeRand), Random.Range(-rangeRand, rangeRand));
    }
    bool IsWithinRangeOfPlayer()
    {
        if (Player.Instance != null)
        {
            Vector3 directionPlayer = transform.position - Player.Instance.transform.position;
            float distancePlayer = directionPlayer.magnitude;
            if (distancePlayer > rangeOfShooting) return false;
        }
        return true;
    }

    protected override TankInfo GetTankInfor(int level)
    {
        return DataManager.Instance.enemyVO.GetTankInfo(level);
    }
    protected override void OnDie()
    {
        ObServer.Instance.Notify(TOPICNAME.ENEMY_DIE, this);
        Create.Instance.CreateExplosionTank(transform);
        PollingObject.DestroyPolling(this);
    }
}

