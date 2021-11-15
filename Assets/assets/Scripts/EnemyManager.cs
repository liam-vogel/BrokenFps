using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    
    [SerializeField] private GameObject boar_Prefab, cannibal_Prefab;

    public Transform [] cannabil_SpawnPoints, boar_SpawnPoints;
    public static EnemyManager instance;

    [SerializeField] private int cannibal_Enemy_Count, boar_Enemy_Count;

    private int initial_Cannibal_Count, initial_boar_Count;

    public float wait_Before_Spawn_Enemies_Time = 10f;

    void Awake()
    {
        makeInstance();
    }
    void makeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        initial_Cannibal_Count = cannibal_Enemy_Count;
        initial_boar_Count = boar_Enemy_Count;

        SpawnEnemies();

        StartCoroutine("CheckToSpawnEnemies");
    }


    void SpawnEnemies()
    {
        SpawnBoars();
        SpawnCannibals();
         StartCoroutine("CheckToSpawnEnemies");
    }
    void SpawnCannibals()
    {
        int index = 0;
        for(int i = 0; i < cannibal_Enemy_Count; i++)
        {
            if(index >= cannabil_SpawnPoints.Length){
                index = 0;
            }
            
            Instantiate(cannibal_Prefab,cannabil_SpawnPoints[index].position, Quaternion.identity);
            index++;
        }    

        cannibal_Enemy_Count = 0;
    
    }

    void SpawnBoars()
    {

        int index = 0;
        for(int i = 0; i < boar_Enemy_Count; i++)
        {
            if(index >= boar_SpawnPoints.Length){
                index = 0;
            }
            
            Instantiate(boar_Prefab,boar_SpawnPoints[index].position, Quaternion.identity);
            index++;
        }    

        boar_Enemy_Count = 0;
    }

    IEnumerator CheckToSpawnEnemies()
    {
      yield return new WaitForSeconds(wait_Before_Spawn_Enemies_Time);
        SpawnCannibals();
        SpawnBoars();
   
    }
    public void EnemyDied(bool cannibal)
    {
        cannibal_Enemy_Count++;
        if(cannibal_Enemy_Count > initial_Cannibal_Count){
            cannibal_Enemy_Count = initial_Cannibal_Count;
        }
        else
        {
            boar_Enemy_Count++;
             if(boar_Enemy_Count > initial_boar_Count){
            boar_Enemy_Count = initial_boar_Count;}
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void StopSpawning()
    {
        StopCoroutine("CheckToSpawnEnemies");
    }

}
