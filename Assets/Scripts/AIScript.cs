﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class AIScript : MonoBehaviour
{
    
    Animator m_Animator;
    //patrolling randomly between waypoints
    public Transform[] moveSpots;
    private int randomSpot;

    public bool playerIsInLOS = false;
    public float fieldOfViewAngle = 160f;
    public float losRadius = 45f;

    private bool aiMemorizesPlayer = false;
    public float memoryStartime = 10f;
    private float increasingMemoryTime;

    Vector3 noisePosition;
    private bool aiHeardPlayer = false;
    public float noiseTravelDistance = 50f;
    public float spinSpeed = 3f;
    private bool canSpin = false;
    private float isSpinningTime;
    public float spinTime =3f;

    NavMeshAgent nav;

    //AI Strafe
    public float distToPlayer = 5.0f;

    private float randomStrafeStartTime;
    private float waitStrafeTime;
    public float t_minStrafe;
    public float t_maxStrafe;

    public Transform strafeRight;
    public Transform strafeLeft;
    private int randomStrafeDir;

    //when to chase
    public float chaseRadius = 20f;

    public float facePlayerFactor = 20f;

    //wait time
    private float waitTime;
    public float startWaitTime = 1f;

    public Transform player;

    public float range = 50.0f;
    public float bulletImpulse = 20.0f;

    private bool onRange = false;

    public Rigidbody projectile1;
    public Rigidbody projectile2;

    public Transform firePoint1;
    public Transform firePoint2;

    public int Health = 100;
    public GameObject explotion;

    public GameObject llave;

    //public AudioSource audioRobot;
    //public AudioSource patrol;

    
    private void Awake()
    {
        float rand = Random.Range(3.0f, 4.0f);
        InvokeRepeating("Shoot", 4, rand);
        nav = GetComponent<NavMeshAgent>();
        nav.enabled = true;
    }
    
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);
    }

    void Shoot()
    {

        if (onRange)
        {

            Rigidbody bullet1 = (Rigidbody)Instantiate(projectile1, firePoint1.position + transform.forward, firePoint2.rotation);
            bullet1.AddForce(transform.forward * bulletImpulse, ForceMode.Impulse);
            Rigidbody bullet2 = (Rigidbody)Instantiate(projectile2, firePoint2.position + transform.forward, firePoint2.rotation);
            bullet2.AddForce(transform.forward * bulletImpulse, ForceMode.Impulse);
            Destroy(bullet1.gameObject, 5);
            Destroy(bullet2.gameObject, 5);
        }

    }
    void PlayAudio()
    {
        if(onRange)
        {
            //patrol.Stop();
            //audioRobot.Play();
        }
    }
    void Update()
    {   
        if(Health <=0)
        {
            Instantiate(llave.gameObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

            onRange = Vector3.Distance(transform.position, player.position) < range;

        //float distance = Vector3.Distance(CharacterControllerMovement.playerPos, transform.position);
        float distance = Vector3.Distance(MechController.playerPos, transform.position);

        //if (distance > chaseRadius)
        {
            Patrol();
        }
        //else if (distance <= chaseRadius)

        
            ChasePlayer();

            FacePlayer();

            //PlayAudio();
        if(distance <= losRadius)
        {
            CheckLOS();
        }

        if (nav.isActiveAndEnabled)
        {
            if(playerIsInLOS == false && aiMemorizesPlayer == false && aiHeardPlayer == false)
            {
                Patrol();
                NoiseCheck();

                StopCoroutine(AiMemory());
            }

            if (playerIsInLOS == false && aiMemorizesPlayer == false && aiHeardPlayer == false)
            {
                Patrol();
                
            }

            else if(aiHeardPlayer == true && playerIsInLOS == false && aiMemorizesPlayer == false)
            {
                canSpin = true;
                //GoToNoisePosition();
            }

            else if(playerIsInLOS == true)
            {
                
                aiMemorizesPlayer = true;

                FacePlayer();

                ChasePlayer();

                
            }

            else if(aiMemorizesPlayer == true && playerIsInLOS == false)
            {
                ChasePlayer();
                StartCoroutine(AiMemory());
            }
        }
    }
  
    void NoiseCheck()
    {
        //float distance = Vector3.Distance(CharacterControllerMovement.playerPos, transform.position);
        float distance = Vector3.Distance(MechController.playerPos, transform.position);

        if (distance <= noiseTravelDistance)

        {
            if (Input.GetButton("Fire1"))
            {
                noisePosition = CharacterControllerMovement.playerPos;

                aiHeardPlayer = true;
            }

            else
            {
                aiHeardPlayer = false;
                canSpin = false;
            }
        }
    }

    void CheckLOS()
    {
        
        //Vector3 direction = CharacterControllerMovement.playerPos - transform.position;
        Vector3 direction = MechController.playerPos - transform.position;

        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction.normalized, out hit, losRadius))
            {
                if (hit.collider.tag == "Player")
                {
                    playerIsInLOS = true;

                    aiMemorizesPlayer = true;
                   
                }

                else
                {
                    playerIsInLOS = false;
                }
            }
        }
    }

        void Patrol()
        {
            nav.SetDestination(moveSpots[randomSpot].position);
        
        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 2.0f)
            {
            //patrol.Play();
            //audioRobot.Stop();
            if (waitTime <= 0)
                {
                    randomSpot = Random.Range(0, moveSpots.Length);

                    waitTime = startWaitTime;
                }
                else { waitTime -= Time.deltaTime; }
            }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "MisilJugador")
        {
            Health -= 10;
            Instantiate(explotion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(explotion.gameObject, 5);
        }
    }
  
    void ChasePlayer()
    {

        //float distance = Vector3.Distance(CharacterControllerMovement.playerPos, transform.position);
        float distance = Vector3.Distance(MechController.playerPos, transform.position);

        if (distance <= chaseRadius && distance > distToPlayer)
        {
            //patrol.Stop();
            //audioRobot.Play();
            //nav.SetDestination(CharacterControllerMovement.playerPos);
            nav.SetDestination(MechController.playerPos);
        }

        else if (nav.isActiveAndEnabled && distance <= distToPlayer)
        {

            
            randomStrafeDir = Random.Range(0, 2);
            randomStrafeStartTime = Random.Range(t_minStrafe, t_maxStrafe);

            if (waitStrafeTime <= 0)
            {

                if (randomStrafeDir == 0)
                {
                    nav.SetDestination(strafeLeft.position);

                }

                else
                if (randomStrafeDir == 1)
                {
                    nav.SetDestination(strafeRight.position);

                }
                waitStrafeTime = randomStrafeStartTime;
            }
            else
            {
                waitStrafeTime -= Time.deltaTime;
            }
        }
    }

        void FacePlayer()
        {
            
            //Vector3 direction = (CharacterControllerMovement.playerPos - transform.position).normalized;
            Vector3 direction = (MechController.playerPos - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * facePlayerFactor);
        }

    IEnumerator AiMemory()
    {
        increasingMemoryTime = 0;
        
        while (increasingMemoryTime < memoryStartime)
        {
            increasingMemoryTime += Time.deltaTime;
            aiMemorizesPlayer = true;
            yield return null;
        }

        aiHeardPlayer = false;
        aiMemorizesPlayer = false;
    }

 }





