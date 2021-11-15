using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HealthScript : MonoBehaviour
{
    private EnemyAttack enemy_Anim;
    private NavMeshAgent navAgent;
    private EnemyController enemy_Controller;

    public float health = 100f;
    public bool is_player, is_Boar, is_Cannibal;

    private bool is_Dead;

    private EnemySounds enemyAudio;

    private PlayerStats player_Stats;
    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }


    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
    }
    void Awake()
    {
        if (is_Cannibal || is_Boar)
        {
            enemy_Anim = GetComponent<EnemyAttack>();
            enemy_Controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();
            enemyAudio = GetComponentInChildren<EnemySounds>();

        }

        if (is_player)
        {
            player_Stats = GetComponent<PlayerStats>();
            
        }

    }
    public void ApplyDamage(float damage)
    {
        if (is_Dead)
            return;

        health -= damage;

        if (is_player)
        {
            player_Stats.Display_HealthStats(health);
        }

        if (is_Cannibal || is_Boar)
        {
            if (enemy_Controller.Enemy_State == EnemyState.PATROL)
            {
                enemy_Controller.chase_Distance = 50f;
            }
        }
        if(health <= 0f)
        {
            PlayerDied();
            is_Dead = true;
        }

        void PlayerDied()
        {
            if (is_Cannibal)
            {
                GetComponent<Animator>().enabled = false;
                GetComponent<BoxCollider>().isTrigger = false;
               // GetComponent<Rigidbody>().AddTorque(-transform.forward * 50f);

                enemy_Controller.enabled = false;
                navAgent.enabled = false;
                enemy_Anim.enabled = false;
                StartCoroutine(DeadSound());

                EnemyManager.instance.EnemyDied(true);
            }

            if (is_Boar)
            {

                enemy_Anim.Dead();
                navAgent.velocity = Vector3.zero;
                navAgent.isStopped = true;
                enemy_Controller.enabled = false;

                StartCoroutine(DeadSound());
                EnemyManager.instance.EnemyDied(false);
            }
            if (is_player)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);

                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].GetComponent<EnemyController>().enabled = false;
                }
                    EnemyManager.instance.StopSpawning();
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<PlayerAttack>().enabled = false;
                GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
            
            }

           


            if (tag == Tags.PLAYER_TAG)
            {
                Invoke(nameof(RestartGame), 1f);


            }
            else
            {

                
                 Invoke(nameof(TurnOffGameObject), 1.5f);
            }
        
          
            
            
            
            
            
            
        }
        
    
    
    }
    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemyAudio.Play_DeadSound();
    }
}
