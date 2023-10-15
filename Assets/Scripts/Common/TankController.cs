using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class TankInfo
{
    public int damage;
    public int hp;
}
public abstract class TankController : MoveController, IHit
{
    [SerializeField] Transform body;
    [SerializeField] Transform gun;
    [SerializeField] BulletController prefabBullet;
    public HpController hpController;
    public LevelController lvController;
    [SerializeField] float timeToNextShoot = 1.5f;
    public bool isShieldActive = false;
    HideHP hideHP;
    public float currentDmg;
    float spawnShoot;
    public float TimeToNextShoot { get {return timeToNextShoot; } set { timeToNextShoot = value; } }
    protected virtual void Awake()
    {
        hpController.onDie = OnDie;
        lvController.OnLevelUp = OnLevelUp;
        lvController.CurrentValue = 0;
        hideHP = GetComponentInChildren<HideHP>();
    }
    protected virtual void Start()
    {
        OnLevelUp(lvController.Level);
        spawnShoot = Time.time;
    }
    protected override void Move(Vector3 direction)
    {
        body.up = direction;
        base.Move(direction);
    }
    protected void Shoot()
    {
        if (gun != null)
        {
            if (Time.time - spawnShoot > timeToNextShoot)
            {
                spawnShoot = Time.time;
                BulletController clone = Create.Instance.CreateBullet(gun, prefabBullet);
                clone.DmgBullet = currentDmg;
                SoundController.instance.PlaySound("laser_shot");
            }
        }
        else
        {
            Debug.LogError("Barrel is null!");
        }
    }
    protected void RotateGun(Vector3 direction)
    {
        direction.z = 0f;
        gun.transform.up = direction;
    }

    
    public void OnHit(BulletController bulletController)
    {
        if (!isShieldActive)
        {
            if (hideHP != null)
            {
                hideHP.ShowAlpha();
            }
            hpController.TakeDamage(bulletController.DmgBullet);
        }
    }
    protected abstract void OnDie();
    protected virtual void OnLevelUp(int level)
    {
        TankInfo tankInfo = GetTankInfor(level);
        hpController.HP = tankInfo.hp;
        currentDmg = tankInfo.damage;
        
    }
    protected abstract TankInfo GetTankInfor(int level);
}
