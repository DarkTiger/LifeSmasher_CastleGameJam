using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] float timeBetweenWaves = 30;
    [SerializeField] int firstWaveEnemiesCount = 10;
    [SerializeField] float wavesMultiplier = 1.5f;
    [SerializeField] float timeBetweenSpawn = 1.5f;
    [SerializeField] int experiencePerEnemy = 150;
    [SerializeField] Image image;

    public static GameManager Instance;

    public int ExperiencePoints => experiencePoint;

    public bool gameStarted = true;
    public int currentWave = 1;
    public float currentGameTime = 0;
    public int currentEnemiesCount = 0;
    public int waveEnemiesKilled = 0;
    public int totalEnemiesKilled = 0;
    public int currentWaveEnemiesToSpawn = 0;

    EnemySpawner[] enemySpawners;
    bool waveStarted = false;
    bool waveSpawnedStarted = false;
    public int experiencePoint = 0;
    float pauseStartedTime = 0;
    float playerHealth = 100f;
    Volume volume;
    bool exiting = false;

    public int RebelLevel = 0;
    public int MagicianLevel = 0;
    public int InnocentLevel = 0;
    public int ExplorerLevel = 0;



    private void Awake()
    {
        Instance = this;
        enemySpawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);
        volume = FindFirstObjectByType<Volume>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        HUD.Instance.UpdateExperienceText(0);

        HUD.Instance.SetEventText("face off the life's challenges and grow yourself.\n\ndon't let it overwhelm you!", 9f, 2000f, true);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !exiting)
        {
            StartCoroutine(QuitCO());
            exiting = true;
        }

        if (gameStarted)
        {
            currentGameTime += Time.deltaTime;

            if (waveStarted)
            {
                System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(currentGameTime);
                HUD.Instance.SetTimeText("TIME: " + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00"));
            }
            else if (!waveStarted && !waveSpawnedStarted)
            {
                if (pauseStartedTime + timeBetweenWaves < currentGameTime)
                {
                    StartCoroutine(WaveStarted());
                }
                else
                {
                    HUD.Instance.SetTimeText("NEXT WAVE IN: " + (Mathf.Max((int)((pauseStartedTime + timeBetweenWaves) - Time.time), 0)).ToString("00"));
                }
            }
        }
    }
    
    public void EnemyDead()
    {
        waveEnemiesKilled++;
        totalEnemiesKilled++;

        experiencePoint += experiencePerEnemy;
        HUD.Instance.UpdateExperienceText(experiencePoint);

        if (waveEnemiesKilled == currentWaveEnemiesToSpawn)
        {
            StartCoroutine(WaveFinished());
        }
    }

    IEnumerator SpawnEnemies()
    {
        currentEnemiesCount = 0;
        
        while (currentEnemiesCount < currentWaveEnemiesToSpawn)
        {
            EnemySpawner enemySpawner = enemySpawners[Random.Range(0, enemySpawners.Length)];
            enemySpawner.SpawnEnemy();
            currentEnemiesCount++;

            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }

    IEnumerator WaveStarted()
    {
        HUD.Instance.CloseShop();
        HUD.Instance.SetWaveText(currentWave);
        waveStarted = true;
        waveSpawnedStarted = true;
        currentWaveEnemiesToSpawn = (int)(currentWave == 1 ? firstWaveEnemiesCount : firstWaveEnemiesCount * (currentWave - 1 * wavesMultiplier) + firstWaveEnemiesCount);
        StartCoroutine(SpawnEnemies());
        HUD.Instance.SetEventText("WAVE " + currentWave + " STARTED!", 5f, 130);
        yield return null;
    }

    IEnumerator WaveFinished()
    {
        if (currentWave == 10)
        {
            StartCoroutine(GameFinishedWin());
        }

        pauseStartedTime = Time.time;

        if (gameStarted)
        {
            HUD.Instance.SetEventText("WAVE " + currentWave + " FINISHED!", 5f, 130);
        }
        
        currentWave++;
        waveEnemiesKilled = 0;
        currentEnemiesCount = 0;
        waveStarted = false;
        waveSpawnedStarted = false;

        yield return new WaitForSeconds(5);

        if (gameStarted)
        {
            HUD.Instance.OpenShop();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator GameFinishedLose()
    {
        gameStarted = false;

        HUD.Instance.CloseShop();
        HUD.Instance.SetEventText("LIFE FINISHED!\n\nKILLS: " + totalEnemiesKilled + "\nTIME: " + (int)currentGameTime, 5f, 530, false, Color.red);
        
        yield return new WaitForSeconds(5f);

        while (image.color.a < 1f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.5f * Time.deltaTime);
            yield return null;
        }

        SceneManager.LoadScene(0);
    }

    IEnumerator GameFinishedWin()
    {
        gameStarted = false;

        HUD.Instance.CloseShop();
        HUD.Instance.SetEventText("LIFE COMPLETED!\n\nKILLS: \" + totalEnemiesKilled\nWAVES: " + currentWave + "\nTIME: " + (int)currentGameTime, 5f, 530, false, Color.green);

        yield return new WaitForSeconds(5f);
                
        while (image.color.a < 1f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.5f * Time.deltaTime);
            yield return null;
        }

        SceneManager.LoadScene(0);
    }

    IEnumerator QuitCO()
    {
        while (image.color.a < 1f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.5f * Time.deltaTime);
            yield return null;
        }

        SceneManager.LoadScene(0);
    }

    public void SetPlayerDamage(float damage)
    {
        if (!gameStarted) return;

        playerHealth -= damage;
        HUD.Instance.SetHealth(playerHealth / 100f);

        if (playerHealth <= 0)
        {
            StartCoroutine(GameFinishedLose());
        }

        StartCoroutine(ChangeHealthEffect(Color.red));
    }

    public void AddPlayerHealth()
    {
        playerHealth += 15;

        if (playerHealth > 100)
        {
            playerHealth = 100;
        }

        HUD.Instance.SetHealth(playerHealth / 100f);
        StartCoroutine(ChangeHealthEffect(Color.green));
    }

    IEnumerator ChangeHealthEffect(Color color)
    {
        if (volume.profile.TryGet(out Vignette vignette))
        {
            vignette.color.value = color;
            vignette.active = true;
        }

        yield return new WaitForSeconds(0.05f);

        if (vignette && gameStarted)
        {
            vignette.active = false;
        }
    }

    public void AddArchetypeLevel(int archetypeIndex, int cost)
    {
        switch (archetypeIndex)
        {
            case 0: 
                if (RebelLevel < 3) 
                {
                    RebelLevel++;
                } 
                break;
            case 1: 
                if (MagicianLevel < 3) 
                {
                    MagicianLevel++; 
                } 
                break;
            case 2: 
                if (InnocentLevel < 3) 
                {
                    InnocentLevel++; 
                } 
                break;
            case 3: 
                if (ExplorerLevel < 3) 
                {
                    ExplorerLevel++; 
                } 
                break;
        }

        experiencePoint -= cost;
        HUD.Instance.UpdateExperienceText(experiencePoint);
    }
}
