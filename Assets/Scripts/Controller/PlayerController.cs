using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.DesignPattern;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : TankController
{
    [SerializeField] FillerTargetController fillerTarget;
    [SerializeField] JoystickController moveJoystick;
    [SerializeField] JoystickController shootJoystick;

    [SerializeField] ShieldController shieldController;
    FireRocketSkillController fireRocketSkill;

    [SerializeField] PanelCDController panelShieldCD;
    [SerializeField] PanelCDController panelRocketCD;
    public int currenMoney;
    protected override void Awake()
    {
        base.Awake();
        currenMoney = 0;
        fireRocketSkill = GetComponentInChildren<FireRocketSkillController>();
        ObServer.Instance.AddObserver(TOPICNAME.ENEMY_DIE, OnEnemyDie);
        ObServer.Instance.AddObserver(TOPICNAME.BOSS_DIE, OnBossDie);
    }
    void OnEnemyDie(object data)
    {
        EnemyController enemy = (EnemyController)data;
        lvController.CurrentValue += enemy.lvController.Level * 10;
    }
    void OnBossDie(object data)
    {
        BossController enemy = (BossController)data;
        lvController.CurrentValue += enemy.lvController.Level * 150;
    }
    protected override void OnDie()
    {
        Create.Instance.CreateExplosionTank(transform);
        ObServer.Instance.RemoveObserver(TOPICNAME.ENEMY_DIE,OnEnemyDie);
        ObServer.Instance.Notify(TOPICNAME.PLAYER_DIE, this);
        ObServer.Instance.RemoveObserver(TOPICNAME.PLAYER_DIE, OnEnemyDie);
        ObServer.Instance.RemoveObserver(TOPICNAME.BOSS_DIE, OnBossDie);
        SoundController.instance.PlaySound("GameOver");
        Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !panelShieldCD.isActiveAndEnabled)
        {
            shieldController.ActiveShield();
            panelShieldCD.OnPress();
        }
        if (Input.GetKeyDown(KeyCode.E) && !panelRocketCD.isActiveAndEnabled)
        {
            fireRocketSkill.ActiveSkill();
            panelRocketCD.OnPress();
        }
        Vector3 playerDirection;
        if (moveJoystick.Direction == Vector2.zero)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            playerDirection = new Vector3(horizontal, vertical, 0);
        }
        else { playerDirection = moveJoystick.Direction; }
        Move(playerDirection);

        Vector3 gunDirection = Vector3.zero;
        if (shootJoystick.Direction == Vector2.zero)
        {
            gunDirection = -(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        }
        else
        {
            gunDirection = -shootJoystick.Direction;
        }
        RotateGun(gunDirection);

        Transform target = TargetController.GetTarget(fillerTarget);
        if (target != null)
        {
            gunDirection = transform.position - target.position;
            RotateGun(gunDirection);
        }

        if( Input.GetMouseButton(0)&&!shootJoystick.isDragging && !moveJoystick.isDragging || shootJoystick.isDragging )
        {
            Shoot();
        }
        
    }
    protected override void OnLevelUp(int level)
    {
        base.OnLevelUp(level);
        if (level != 1)
        {
            SoundController.instance.PlaySound("LevelUp");
        }
    }
    protected override TankInfo GetTankInfor(int level)
    {
        return DataManager.Instance.playerVO.GetTankInfo(level);
    }

}

public class Player : SingletonMonoBehaviour<PlayerController> { }
