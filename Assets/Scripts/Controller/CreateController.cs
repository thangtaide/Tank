using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.DesignPattern;

public class ITEMNAME
{
    public const string HP_ITEM = "Hp_Item";
    public const string GOLD_ITEM = "Gold_Item";
}
public class CreateController : MonoBehaviour
{
    [SerializeField] EnemyController prefabEnemy;
    [SerializeField] BossController prefabBoss;
    [SerializeField] RocketController prefabRocket;
    [SerializeField] float rangeRandom = 40f;
    [SerializeField] float minRangeRandom = 25f;
    [SerializeField] ItemController hpItem;
    [SerializeField] ItemController goldItem;
    [SerializeField] BagItemController bagItem;
    int enemyCount;
    [SerializeField] Explosion explosion;
    [SerializeField] Explosion explosionRocket;
    [SerializeField] Explosion explosionTank;
    int createCount;
    float spawnCreateEnemy;
    private void Start()
    {
        spawnCreateEnemy = Time.time - 2;
        createCount = 0;
        enemyCount = 1;
    }
    public ItemController CreateItem(Vector3 position, string name)
    {
        if (name == ITEMNAME.HP_ITEM)
        {
            return Instantiate(hpItem, position, Quaternion.identity);
        }
        else if (name == ITEMNAME.GOLD_ITEM)
        {
            return Instantiate(goldItem, position, Quaternion.identity);
        }
        return null;
    }
    BagItemController CreateBagItem(Vector3 position)
    {
        return Instantiate(bagItem, position, Quaternion.identity);
    }
    private void Update()
    {
        if (Player.Instance != null)
        {
            if (Time.time - spawnCreateEnemy > 5)
            {
                spawnCreateEnemy = Time.time;
                createCount++;
                if (enemyCount < 10)
                {
                    enemyCount = createCount / 5 + 1;
                }
                CreateEnemy(enemyCount);
                if (createCount % 5 == 4)
                {
                    Vector3 positon;
                    while (true)
                    {
                        Vector3 random = new Vector3(Random.Range(-rangeRandom + 5, rangeRandom - 5), Random.Range(-rangeRandom + 5, rangeRandom - 5));
                        if (random.magnitude >= minRangeRandom - 5)
                        {
                            positon = Player.Instance.transform.position + random;
                            break;
                        }
                    }
                    CreateBagItem(positon);
                }
                if (createCount % 10 == 0)
                {
                    CreateBoss(createCount/10);
                }
                }
        }
    }
    public BulletController CreateBullet(Transform tranShoot, BulletController prefabBullet)
    {
        BulletController bullet = Instantiate(prefabBullet, tranShoot.position - (tranShoot.up * 1.5f), Quaternion.identity);
        bullet.transform.position = tranShoot.position - (tranShoot.up * 1.5f);
        bullet.transform.up = -tranShoot.up;
        return bullet;
    }
    public RocketController CreateRocket(Transform tranShoot, bool leftRight)
    {
        RocketController bullet = Instantiate(prefabRocket, tranShoot.position, Quaternion.identity);
        bullet.transform.up = tranShoot.up;
        if (leftRight) { bullet.transform.Rotate(0, 0, 90); }
        else bullet.transform.Rotate(0, 0, -90);
        return bullet;
    }
    public Explosion CreateExplosion(Transform bullet)
    {
        return Instantiate(explosion, bullet.position, Quaternion.identity);
    }
    public Explosion CreateExplosionRocket(Transform bullet)
    {
        return Instantiate(explosionRocket, bullet.position, Quaternion.identity);
    }
    public Explosion CreateExplosionTank(Transform tank)
    {
        return Instantiate(explosionTank, tank.position, Quaternion.identity);
    }
    public void CreateEnemy(int count)
    {

        for (int i = 0; i < count; i++)
        {
            Vector3 positon;
            while (true)
            {
                Vector3 random = new Vector3(Random.Range(-rangeRandom, rangeRandom), Random.Range(-rangeRandom, rangeRandom));
                if (random.magnitude >= minRangeRandom)
                {
                    positon = Player.Instance.transform.position + random;
                    break;
                }
            }
            EnemyController enemy = Instantiate(prefabEnemy, positon, Quaternion.identity);
            enemy.transform.position = positon;
            enemy.lvController.Level = createCount / 4 + 1;
        }

    }
    void CreateBoss(int level)
    {
        Vector2 positionBoss = new Vector2(Player.Instance.transform.position.x, Player.Instance.transform.position.y+40);
        BossController bossController = Instantiate(prefabBoss,positionBoss, Quaternion.identity);
        bossController.lvController.Level = level;
    }
}
public class Create : SingletonMonoBehaviour<CreateController>
{

}
