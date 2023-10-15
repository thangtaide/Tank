using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Base.DesignPattern;
using UnityEngine.SceneManagement;

public class TOPICNAME
{
    public const string ENEMY_DIE = "Enemy_Die";
    public const string PLAYER_DIE = "Player_Die";
    public const string BOSS_DIE = "Boss_Die";
    public const string EARNED_GOLD = "Earn_gold";
}

public class GameController : MonoBehaviour
{
    static int totalMoney;
    int currentMoney;
    public bool isPause = false;
    [SerializeField] TextMeshProUGUI textScore;
    [SerializeField] TextMeshProUGUI textScoreDie;
    [SerializeField] TextMeshProUGUI textGold;
    [SerializeField] TextMeshProUGUI textGoldDie;
    [SerializeField] TextMeshProUGUI textTotalGold;
    [SerializeField] GameObject panelPlayerDie;

    public GameObject pausePanel;
    int score = 0;
    private void Awake()
    {
        DataManager.Instance.LoadData();
        totalMoney = GetInt("Money");
        currentMoney = 0;
    }
    private void Start()
    {
        pausePanel.SetActive(false);
        ObServer.Instance.AddObserver(TOPICNAME.ENEMY_DIE, OnEnemyDie);
        ObServer.Instance.AddObserver(TOPICNAME.PLAYER_DIE, OnPlayerDie);
        ObServer.Instance.AddObserver(TOPICNAME.BOSS_DIE, OnBossDie);
        ObServer.Instance.AddObserver(TOPICNAME.EARNED_GOLD, OnChangeMoney);
    }
    void OnChangeMoney(object data)
    {
        if (Player.Instance != null)
        {
            currentMoney = Player.Instance.currenMoney;
            if (textGold == null) { Debug.Log("TextGold null"); }
            else
            {
                textGold.text = "Gold: " + currentMoney.ToString();
                textGoldDie.text = "Gold: " + currentMoney.ToString();
            }
        }
    }
    void OnBossDie(object data)
    {
        BossController enemy = (BossController)data;
        float goldRand = Random.Range(0, 10);
        for (int i = 0; i < enemy.lvController.Level; i++)
        {
            
            Create.Instance.CreateItem(GetRandomPos(enemy.gameObject.transform.position, 8), ITEMNAME.GOLD_ITEM);
            Create.Instance.CreateItem(GetRandomPos(enemy.gameObject.transform.position, 8), ITEMNAME.GOLD_ITEM);
            Create.Instance.CreateItem(GetRandomPos(enemy.gameObject.transform.position, 8), ITEMNAME.HP_ITEM);
        }
        
        score += enemy.lvController.Level * 150;
        textScore.text = "Score: " + score.ToString();
        textScoreDie.text = "Score: " + score.ToString();
    }
    Vector3 GetRandomPos(Vector3 position, float rangeRandom)
    {
        return position+new Vector3(Random.Range(-rangeRandom, rangeRandom), Random.Range(-rangeRandom, rangeRandom));
    }
    void OnEnemyDie(object data)
    {
        EnemyController enemy = (EnemyController)data;
        float goldRand = Random.Range(0, 10);
        if(goldRand <=1)
        {
            Create.Instance.CreateItem(enemy.gameObject.transform.position, ITEMNAME.GOLD_ITEM);
        }
        score += enemy.lvController.Level * 10;
        textScore.text = "Score: "+score.ToString();
        textScoreDie.text = "Score: " + score.ToString();
    }
    void OnPlayerDie(object data)
    {
        totalMoney += currentMoney;
        textTotalGold.text = "Total Gold: " + totalMoney.ToString();
        SetInt("Money", totalMoney);
        panelPlayerDie.SetActive(true);
    }
    private void OnDestroy()
    {
        ObServer.Instance.RemoveObserver(TOPICNAME.ENEMY_DIE, OnEnemyDie);
        ObServer.Instance.RemoveObserver(TOPICNAME.PLAYER_DIE, OnPlayerDie);
        ObServer.Instance.RemoveObserver(TOPICNAME.BOSS_DIE, OnBossDie);

    }
    public void OnStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void TogglePause()
    {
        isPause = !isPause;
        if (pausePanel)
        {
            Time.timeScale = isPause ? 0 : 1;
            pausePanel.SetActive(isPause);
        }
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void OnExit()
    {
        Application.Quit();
    }
    private int GetInt(string str)
    {
        return PlayerPrefs.GetInt(str, 0);
    }
    private void SetInt(string str, int i)
    {
        PlayerPrefs.SetInt(str, i);
    }
}
