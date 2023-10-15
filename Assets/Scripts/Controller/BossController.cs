using Base.DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    [SerializeField] float TimeBossShoot;
    float currentTime;
    protected override TankInfo GetTankInfor(int level)
    {
        return DataManager.Instance.bossVO.GetTankInfo(level);
    }
    protected override void Start()
    {
        base.Start();
        currentTime = Time.time;
    }
    protected override void EnemyShoot()
    {
        if (Time.time - currentTime >= TimeBossShoot)
        {
            currentTime = Time.time;
            for(int i = 0; i < 6; i++)
            {
                StartCoroutine(ExampleCoroutine(i*0.1f));
            }
        }
    }
    private IEnumerator ExampleCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Shoot();
    }
    protected override void OnDie()
    {
        ObServer.Instance.Notify(TOPICNAME.BOSS_DIE, this);
        Create.Instance.CreateExplosionTank(transform);
        Destroy(gameObject);
    }
}


