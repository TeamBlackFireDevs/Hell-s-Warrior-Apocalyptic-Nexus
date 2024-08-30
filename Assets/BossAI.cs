using System.Collections;
using System.Collections.Generic;
using HQFPSWeapons;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{

    public float lookRadius;
    public float attackDistance;
    public float stopDistance;
    public float health;
    public float damage;
    public int hellPointsOnDeath;
    public GameObject hellPointsText;
    public enum AttackType {Laser, Slam};
    AttackType currentAttackType;
    public float minAttackTime;
    public float maxAttackTime;
    public float minGruntSoundTime;
    public float maxGruntSoundTime;
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;
    Transform target;

    NavMeshAgent agent;
    Animator anim;

    bool alive, attacking, grunting, chasing, doingAttack;
    public GameObject deathFX;
    public List<AudioClip> deathSounds = new List<AudioClip>();

    [Range(0f,1f)]
    public float gruntingVolume;
    [Range(0f,1f)]
    public float attackingVolume;
    AudioSource audioSource;

    public Transform bodyCenter;

    public GameObject audioSpawnObj;

    public string enemyID;

    [Header("BURNING")]

    public GameObject burnFX;
    public float initialBurnDamage;
    public float burnDamage;
    public int burnCount;
    public float burnInterval;
    public float burnAgainInterval;
    [Header("Warning")]
    public GameObject warningObj;
    [Header("Attacks")]

    public GameObject laserEyes;
    public float laserTime;
    public AudioClip laserSound;
    public ParticleSystem groundSlamEffect;
    public ParticleSystem slamPlasmaExplosion;
    public float splashRange;
    public LayerMask targetLayers;
    public AudioClip slamSound;
    public AudioClip slamDebrisSound;
    [Header("UI")]
    public Slider healthbar;


    int currentBurnCounts;
    float burnTimer;
    float burnAgainTimer;
    bool burning, burningAgain;

    float distance;





    void Start()
    {
        healthbar.maxValue = health;
        healthbar.value = health;
        alive = true;
        agent = GetComponent<NavMeshAgent>();
        target = GameManager.Instance.CurrentPlayer.transform;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(!alive)return;
        distance = Vector3.Distance(target.position,transform.position);

        if(!grunting && !attacking)
        {
            grunting = true;
            StartCoroutine(GruntSoundLoop());
        }

        laserEyes.transform.LookAt(target);

        if(burning)
        {
            burnTimer += Time.deltaTime;
            if(burnTimer >= burnInterval)
            {
                burnTimer = 0f;
                currentBurnCounts --;
                TakeBurnDamage(burnDamage);
                if(currentBurnCounts <= 0)
                {
                    StopBurning();
                }
            }
            if(burningAgain)
            {
                burnAgainTimer += Time.deltaTime;
                if(burnAgainTimer > burnAgainInterval)
                {
                    burnAgainTimer = 0f;
                    burningAgain = false;
                    TakeBurnDamage(initialBurnDamage);
                }
            }
        }

        if(distance <= lookRadius)
        {
            FaceTarget();
            //grunting = false;
            //StopCoroutine(GruntSoundLoop());

            if(distance <= attackDistance)
            {
                Debug.Log("ATTACK!");
                //Attack the target
                //Face the target
                if(!attacking) 
                {
                    attacking = true;
                    grunting = false;
                    StopCoroutine(GruntSoundLoop());
                    StartCoroutine(AttackLoop());
                }

                if(distance <= stopDistance)
                {
                    anim.SetBool("chasing",false);
                    anim.SetBool("patrolling",false);
                    chasing = false;
                    agent.isStopped = true;
                }else
                {
                    if(doingAttack) return;
                    anim.SetBool("chasing",true);
                    anim.SetBool("patrolling",false);
                    agent.isStopped = false;
                    agent.speed = runSpeed;
                    agent.SetDestination(target.position);
                    // if(!chasing)
                    // {
                    //     chasing = true;
                    //     PlayAudio(angrySound,1f);
                    // }
                }

                
            }else
            {
                if(doingAttack) return;
                attacking = false;
                anim.SetBool("attacking",false);
                StopCoroutine(AttackLoop());

                anim.SetBool("chasing",true);
                anim.SetBool("patrolling",false);
                agent.isStopped = false;
                agent.speed = runSpeed;
                agent.SetDestination(target.position);
            }
    
        }else
        {
            if(doingAttack) return;
            agent.isStopped = false;
            anim.SetBool("chasing",false);
            anim.SetBool("attacking",false);
            anim.SetBool("patrolling",true);
            agent.SetDestination(target.position);
            agent.speed = walkSpeed;


            //transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }
    }

    IEnumerator GruntSoundLoop()
    {
        while(alive && !attacking && grunting)
        {
            yield return new WaitForSeconds(Random.Range(minGruntSoundTime,maxGruntSoundTime));
            if(alive && !attacking && grunting)
            {
                int randGrunt = Random.Range(0,11);
                if(randGrunt < 5)
                {
                    AudioClip clip = SoundManager.instance.GrabASound(enemyID,"Grunt");
                    if(clip != null)
                    {
                        PlayAudio(clip,gruntingVolume);
                    }
                }
            }
        }
        yield break;
    }
    
    IEnumerator AttackLoop()
    {
        while(alive && attacking)
        {
            if(doingAttack)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }
            yield return new WaitForSeconds(Random.Range(minAttackTime,maxAttackTime));
            currentAttackType = (AttackType)Random.Range(0, 2);

            anim.SetBool("chasing",false);
            anim.SetBool("patrolling",false);
            chasing = false;
            agent.isStopped = true;

            warningObj.SetActive(true);
            yield return new WaitForSeconds(1f);
            warningObj.SetActive(false);
            if(alive && attacking)
            {
                doingAttack = true;
                anim.SetInteger("attackType", (int)currentAttackType);
                anim.SetBool("attacking",true);
    
                Debug.Log("Zombie Attacks!");
            }
        }
        yield break;
    }

    public void ActivateLasers()
    {
        StartCoroutine(LaserShooting());
    }

    IEnumerator LaserShooting()
    {
        PlayAudio(laserSound, 1f, true);
        anim.speed = 0f;
        laserEyes.SetActive(true);
        yield return new WaitForSeconds(laserTime);
        laserEyes.SetActive(false);
        anim.speed = 1f;
        audioSource.loop = false;
        audioSource.Stop();
    }

    public void GroundSlam()
    {
        groundSlamEffect.Play();
        slamPlasmaExplosion.Play();
        PlayAudio(slamSound, 1f);
        PlayAudio(slamDebrisSound, 1f);
        Collider[] collisions = Physics.OverlapSphere(transform.position, splashRange, targetLayers);
        foreach(Collider col in collisions)
        {
            Debug.Log("Groundslammed " + col.transform.name);
            if(col.transform.root.CompareTag("Player"))
            {
                GameManager.Instance.CurrentPlayer.ChangeHealth.Try(new HealthEventData(-damage));
            }
            break;
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(new Vector3(direction.x,0f,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,rot,Time.deltaTime * turnSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,lookRadius);
    }

    public void DamagePlayer()
    {
        GameManager.Instance.CurrentPlayer.ChangeHealth.Try(new HealthEventData(-damage));
    }

    public void AttackAnimFinished()
    {
        doingAttack = false;
        anim.SetBool("attacking",false);
    }

    public void TakeDamage(float dmg, Collider col)
    {
        if(!alive)return;
        if(col.transform.CompareTag("EnemyHead"))
        {
            health -= dmg * 3;
        }else
        {
            health -= dmg;
        }
        healthbar.value = health;
        AudioClip clip = SoundManager.instance.GrabAHurtSound();
        if(clip != null)
        {
            PlayAudio(clip,1f);
        }
        if(health <= 0f)
        {
            Death();
        }
    }

    void TakeBurnDamage(float dmg)
    {
        if(!alive)return;
        health -= dmg;
        healthbar.value = health;
        // AudioClip clip = SoundManager.instance.GrabAHurtSound();
        // if(clip != null)
        // {
        //     PlayAudio(clip,1f);
        // }
        if(health <= 0f)
        {
            Death();
        }
    }

    public void Burn()
    {
        if(!alive)return;
        if(burning && !burningAgain)
        {
            burningAgain = true;
            burnAgainTimer = 0f;
            currentBurnCounts = burnCount;
        }
        if(!burning)
        {
            TakeBurnDamage(initialBurnDamage);
            burning = true;
            burnTimer = 0f;
            currentBurnCounts = burnCount;
        }
        // AudioClip clip = SoundManager.instance.GrabAHurtSound();
        // if(clip != null)
        // {
        //     PlayAudio(clip,1f);
        // }
        burnFX.SetActive(true);
    }

    void Death()
    {
        alive = false;
        anim.SetTrigger("Death");
        hellPointsText.SetActive(true);

        for(int i = 0; i < deathSounds.Count; i++)
        {
            SpawnAndPlayAudio(deathSounds[i],1f);
        }
        ScoreManager.Instance.AddPoints(hellPointsOnDeath);

        foreach(Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
        Instantiate(deathFX,bodyCenter.position,Quaternion.identity);
        agent.isStopped = true;
        Destroy(gameObject,7f);
    }
    
    void StopBurning()
    {
        burning = false;
        burnFX.SetActive(false);
    }

    void PlayAudio(AudioClip clip, float volume, bool loop = false)
    {
        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.volume = volume * GlobalVolumeManager.Instance.GetSoundVol();
        audioSource.Play();
    }

    void SpawnAndPlayAudio(AudioClip clip, float volume)
    {
        GameObject audioObj = Instantiate(audioSpawnObj,bodyCenter);
        AudioSource audioSource = audioObj.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume * GlobalVolumeManager.Instance.GetSoundVol();
        audioSource.Play();
    }


    

}
