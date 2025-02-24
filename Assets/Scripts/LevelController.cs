using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;

    [Serializable]
    public class Wave
    {
        public int _smallEnemyCount;
        public int _bigEnemyCount;
        public float timeNextWave;
    }
    [SerializeField]
    public List<Wave> _waves = new List<Wave>();

    public List<Transform> _spawnPointsWave = new List<Transform>();
    public List<Transform> _spawnPointsCurrentWave = new List<Transform>();
    public float timer,maxTimePerSpawn;
    public bool isWaveStart, isSpavnedEnemy;
    public bool isNeedKillAll;
    public int currentWave;
    public int enemysCountSpawned, allSpawnedEnemys, destroyEnemyCount;
    public int enemysBigCountSpawned, allSpawnedEnemysBig, destroyEnemyBigCount;

    [HideInInspector] public int PlayersCount = 4;
    public List<GameObject> smallEnemys = new List<GameObject>();
    public List<GameObject> bigEnemys = new List<GameObject>();
    public List<AIController> _AIControllers = new List<AIController>();
    
    private bool isEndLevel;
    private void Awake()
    {
        instance = this;
    }

    public void SetKillsAI(string name)
    {
        foreach (var controller in _AIControllers)
        {
            if (controller.gameObject.name == name)
            {
                controller.playerKills++;
            }
        }
    }

    void Start()
    {
        UIController.instance.SetPlayersCountInWave(PlayersCount);
        UIController.instance.SetWaveCountText(currentWave+1,_waves.Count);
        UIController.instance.SetWaveTimerState(true);
    }

    public void SetMinusPlayersCount()
    {
        PlayersCount--;
        UIController.instance.SetPlayersCountInWave(PlayersCount);
    }
    
    void Update()
    {
        if (isWaveStart && !isSpavnedEnemy)
        {
            if (isNeedKillAll)
            {
                if (timer < maxTimePerSpawn)
                {
                    timer += 1f * Time.deltaTime;
                    float value = maxTimePerSpawn - timer;
                    float seconds = value % 60;
                    float minutes = (value - seconds) / 60;
                    string text = string.Format("{0:00}:{1:00}", minutes, seconds);
                    UIController.instance.SetWaveTimerText(text);
                }
                else
                {
                    isSpavnedEnemy = true;
                    SetEnemyCountStartWawe();
                    UIController.instance.SetWaveTimerState(false);
                    StartCoroutine(CheckEnemyCount());
                }
            }
            else
            {
                if (currentWave == 0)
                {
                    if (timer < maxTimePerSpawn)
                    {
                        timer += 1f * Time.deltaTime;
                        float value = maxTimePerSpawn - timer;
                        float seconds = value % 60;
                        float minutes = (value - seconds) / 60;
                        string text = string.Format("{0:00}:{1:00}", minutes, seconds);
                        UIController.instance.SetWaveTimerText(text);
                    }
                    else
                    {
                        isSpavnedEnemy = true;
                        SetEnemyCountStartWawe();
                        UIController.instance.SetWaveTimerState(false, false);
                        StartCoroutine(SpawnEnemy());
                    }
                }
                else
                {
                    if (timer < _waves[currentWave-1].timeNextWave)
                    {
                        timer += 1f * Time.deltaTime;
                        float value = _waves[currentWave-1].timeNextWave - timer;
                        float seconds = value % 60;
                        float minutes = (value - seconds) / 60;
                        string text = string.Format("{0:00}:{1:00}", minutes, seconds);
                        UIController.instance.SetWaveTimerText(text);
                    }
                    else
                    {
                        if (currentWave < _waves.Count)
                        {
                            isSpavnedEnemy = true;
                            SetEnemyCountStartWawe();
                            UIController.instance.SetWaveTimerState(false,false);
                            StartCoroutine(SpawnEnemy());
                        }
                        else
                        {
                            isEndLevel = true;
                            UIController.instance.OpenPanelWin();
                        }
                    }
                }
            }
        }
    }

    void SetEnemyCountStartWawe()
    {
        //UIController.instance.SetEnemysCountInWave(_enemysCountInWave[currentWave]);
    }


    void ResetWave()
    {
        timer = 0f;
        isSpavnedEnemy = false;
        currentWave++;
    }

    IEnumerator SpawnEnemy()
    {
        _spawnPointsCurrentWave.Clear();
        foreach (Transform points in _spawnPointsWave[currentWave])
        {
            _spawnPointsCurrentWave.Add(points);
        }
        int count = 0;
        while (count < _waves[currentWave]._smallEnemyCount)
        {
            int randID = Random.Range(0, _spawnPointsCurrentWave.Count);
            int randEnemy = Random.Range(0, smallEnemys.Count);
            Instantiate(smallEnemys[randEnemy], GetRandomPosition(_spawnPointsCurrentWave[randID]), Quaternion.identity);
            AddEnemyCount();
            count++;
            yield return new WaitForSeconds(0.1f);
        }

        SpawnBigEnemys();
        ResetWave();
    }
    IEnumerator CheckEnemyCount(int countOst=0)
    {
        _spawnPointsCurrentWave.Clear();
        foreach (Transform points in _spawnPointsWave[currentWave])
        {
            _spawnPointsCurrentWave.Add(points);
        }
        int count = 0;
        if (countOst == 0)
        {
            int enCount = 20;
            if (_waves[currentWave]._smallEnemyCount < 20)
            {
                enCount = _waves[currentWave]._smallEnemyCount;
            }
            while (count < enCount)
            {
                int randID = Random.Range(0, _spawnPointsCurrentWave.Count);
                int randEnemy = Random.Range(0, smallEnemys.Count);
                Instantiate(smallEnemys[randEnemy], GetRandomPosition(_spawnPointsCurrentWave[randID]), Quaternion.identity);
                AddEnemyCount();
                count++;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            while (count < countOst)
            {
                int randID = Random.Range(0, _spawnPointsCurrentWave.Count);
                int randEnemy = Random.Range(0, smallEnemys.Count);
                Instantiate(smallEnemys[randEnemy], GetRandomPosition(_spawnPointsCurrentWave[randID]), Quaternion.identity);
                AddEnemyCount();
                count++;
                yield return new WaitForSeconds(0.1f);
            }
        }

       // StartCoroutine(WaitPerSpawn());
    }

    void SpawnBigEnemys()
    {
        if (isNeedKillAll)
        {
            if (enemysBigCountSpawned == 0)
            {
                for (int i = 0; i < _waves[currentWave]._bigEnemyCount; i++)
                {
                    int randID = Random.Range(0, _spawnPointsCurrentWave.Count);
                    int randEnemy = Random.Range(0, bigEnemys.Count);
                    Instantiate(bigEnemys[randEnemy], GetRandomPosition(_spawnPointsCurrentWave[randID]),
                        Quaternion.identity);
                    AddEnemyCount(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < _waves[currentWave]._bigEnemyCount; i++)
            {
                int randID = Random.Range(0, _spawnPointsCurrentWave.Count);
                int randEnemy = Random.Range(0, bigEnemys.Count);
                Instantiate(bigEnemys[randEnemy], GetRandomPosition(_spawnPointsCurrentWave[randID]),
                    Quaternion.identity);
                AddEnemyCount(true);
            }
        }
    }
    Vector3 GetRandomPosition(Transform point)
    {
        Vector3 randomPos = Random.insideUnitSphere * 5f + point.position;
        randomPos.y = 0.5f;
        return randomPos;
    }
    
    public void AddEnemyCount(bool isBig=false)
    {
        if (!isBig)
        {
            enemysCountSpawned++;
            allSpawnedEnemys++;
        }
        else
        {
            enemysBigCountSpawned++;
            allSpawnedEnemysBig++;
        }
    }
    
    public void RemoveEnemyCount(bool isBig=false)
    {
        if (!isBig)
        {
            enemysCountSpawned--;
            destroyEnemyCount++;
            
            //UIController.instance.SetEnemysCountInWave(_count);
        }
        else
        {
            enemysBigCountSpawned--;
            destroyEnemyBigCount++;
        }
        int _count = allSpawnedEnemys - destroyEnemyCount;
        if (_count == 0)
        {
            if (allSpawnedEnemys == _waves[currentWave]._smallEnemyCount)
            {
                if (destroyEnemyBigCount == 0)
                {
                    SpawnBigEnemys();
                }else if (destroyEnemyBigCount == _waves[currentWave]._bigEnemyCount)
                {
                    if (currentWave + 1 < _waves.Count)
                    {
                        currentWave++;
                        UIController.instance.SetWaveCountText(currentWave + 1, _waves.Count);
                        timer = 0;
                        UIController.instance.SetWaveTimerState(true);
                        isSpavnedEnemy = false;
                        destroyEnemyCount = allSpawnedEnemys = 
                        enemysBigCountSpawned=allSpawnedEnemysBig=0;
                    }
                    else
                    {
                        if (destroyEnemyCount == allSpawnedEnemys)
                        {
                            if (!isEndLevel)
                            {
                                isEndLevel = true;
                                UIController.instance.OpenPanelWin();
                            }
                        }
                    }
                }
            }
            else
            {
                int ost = _waves[currentWave]._smallEnemyCount - allSpawnedEnemys;
                StartCoroutine(CheckEnemyCount(ost));
            }
        }
    }
}
