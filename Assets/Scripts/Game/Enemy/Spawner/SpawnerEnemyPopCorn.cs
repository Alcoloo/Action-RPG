using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg;

public class SpawnerEnemyPopCorn : MonoBehaviour {
    
    public static List<SpawnerEnemyPopCorn> spawnerList = new List<SpawnerEnemyPopCorn>();
    public static event Action<string, Vector3, bool> onSpawn;
    protected int numberSpawned = 0;
    [SerializeField]
    protected GameObject enemy;
    [SerializeField]
    protected int maxEnemySpawn= 20;
	
    void Awake()
    {
        if (!spawnerList.Contains(this)) spawnerList.Add(this);
    }

	void Start () {
        
    }

    public void startWave()
    {
        StartCoroutine(SpawnEnemies());
    }
	
    protected IEnumerator SpawnEnemies()
    {
        while(numberSpawned <= maxEnemySpawn)
        {
            numberSpawned++;
            bool lIsLead = numberSpawned == 0 ? true : false;
            SpawnEnemy(lIsLead);
            yield return new WaitForSeconds(0.2f);
        }
        //test 
        EnemyManager.manager.testBehavior();
        numberSpawned = 0;
    }

    protected void SpawnEnemy(bool isLead)
    {
        if(enemy != null)
        {
            //GameObject lEnemy = PoolingManager.instance.getFromPool(enemy.name);
            //lEnemy.SetActive(true);
            //lEnemy.transform.position = transform.position;
            onSpawn(enemy.name, transform.position, isLead);
        }
    }
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy()
    {
        spawnerList = new List<SpawnerEnemyPopCorn>(); 
    }

}
