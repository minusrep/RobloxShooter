using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Pathfinding;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    public AnimancerComponent _AnimancerComponent;

    [SerializeField] private AnimationClip _idle, _walkForward, _dead;
    private string lastAnimationState;


    public float enemyHealth = 100;
    public float enemyArmor = 0;
    public float walkSpeed;

    public int state;
    public Vector3 randomDestation;

    [HideInInspector] public IAstarAI ai;
    public bool isAtDestination;
    public Vector3 DestantionPoint;
    public GameObject enemy;
    private List<GameObject> enemys = new List<GameObject>();

    public AIWeaponController _WeaponController;

    private bool CanMove, isDead;
    [HideInInspector] public string AIName;
    [HideInInspector] public int enemyLevel;
    public TMP_Text _aiNameText;


    private Transform lineCastShootPos;

    [HideInInspector] public int playerKills;
    public SFXPlayer _soundFX;
    void Start()
    {
        ai = GetComponent<IAstarAI>();
        ai.maxSpeed = walkSpeed;
        SetAIAnimationState("idle");
        state = -1;
        enemyLevel = Random.Range(0, 50);
        _WeaponController.GetWeapon(enemyLevel);
        lineCastShootPos = _WeaponController._weaponPose;
    }


    public void SetName(string name)
    {
        AIName = name;
        _aiNameText.text = name + "\nlvl: " + enemyLevel.ToString();
    }

    void OnTargetReached()
    {
        SetAIAnimationState("idle");
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (!CanMove)
            {
                if (UIController.instance.isConectionSuccess)
                {
                    CanMove = true;
                }
            }

            if (CanMove)
            {
                if (ai != null)
                {
                    if (ai.reachedEndOfPath)
                    {
                        if (!isAtDestination) OnTargetReached();
                        isAtDestination = true;
                    }
                    else isAtDestination = false;

                    if (!isAtDestination)
                    {
                        /*if (_state == EnemyState.idle)
                        {
                            _AnimancerComponent.Play(walk, 0.25f);
                            _state = EnemyState.walk;
                        }*/
                    }
                }

                if (state == -1)
                {
                    StartCoroutine(FindPoint());
                    state = 0;
                }

                if (state == 1)
                {
                    if (Vector3.Distance(DestantionPoint, transform.position) <= 3f)
                    {
                        state = -1;
                    }
                }

                if (state != 2)
                {
                    Collider[] hitColliders =
                        Physics.OverlapSphere(transform.position, 30f, _WeaponController.enemyLayer);
                    if (hitColliders.Length > 0)
                    {
                        if (enemy == null)
                        {
                            for (int i = 0; i < hitColliders.Length; i++)
                            {
                                if (hitColliders[i].GetComponent<EnemyController>().enemyHealth > 0)
                                {
                                    enemy = hitColliders[i].gameObject;
                                    state = 2;
                                    
                                    break;
                                }
                            }
                        }
                    }
                }

                if (state == 2)
                {
                    if (enemy != null)
                    {
                        RaycastHit hit;
                        if (Vector3.Distance(enemy.transform.position, transform.position) >= 10f)
                        {
                           SetDestantionToPoint(enemy.transform.position);
                           transform.LookAt(enemy.transform);
                           if (Physics.Raycast(lineCastShootPos.position, transform.TransformDirection(Vector3.forward), out hit, 100f, _WeaponController.enemyLayer))
                           {
                               Debug.DrawRay(lineCastShootPos.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                               Debug.Log("Did Hit");
                               transform.LookAt(enemy.transform);
                               _WeaponController.StartShoot(enemy);
                           }else
                           {
                               Debug.DrawRay(lineCastShootPos.position, transform.TransformDirection(Vector3.forward) * 100, Color.red);
                               Debug.Log("Did not Hit");
                           }
                           
                        }
                        else
                        {
                            if (Physics.Raycast(lineCastShootPos.position, transform.TransformDirection(Vector3.forward), out hit, 100f, _WeaponController.enemyLayer))
                            {
                                ai.destination = transform.position;
                                ai.canMove = false;
                                SetAIAnimationState("idle");
                                transform.LookAt(enemy.transform);
                                _WeaponController.StartShoot(enemy);
                            }
                            else
                            {
                                transform.LookAt(enemy.transform);
                                SetDestantionToPoint(enemy.transform.position);
                                Debug.DrawRay(lineCastShootPos.position, transform.TransformDirection(Vector3.forward) * 100, Color.red);
                                Debug.Log("Did not Hit");
                            }
                        }
                        if (enemy.GetComponent<EnemyController>().enemyHealth <= 0)
                        {
                            enemy = null;
                            _WeaponController.StopShoot();
                            state = -1;
                        }
                    }
                }
            }
        }
    }

    IEnumerator FindPoint()
    {
        DestantionPoint = PathForAI.instance.GetPointToMove();
        float tWait = Random.Range(0.1f, 1f);
        yield return new WaitForSeconds(tWait);
        if (!isDead)
        {
            SetDestantionToPoint(DestantionPoint);
            float rTime = Random.Range(1f, 3f);
            StartCoroutine(ChangePos(rTime));
            if (state != 2)
                state = 1;
        }
    }
    
    IEnumerator ChangePos(float time)
    {
        yield return new WaitForSeconds(time);
        if (enemy == null)
        {
            int rId = Random.Range(0, 100);
            if (rId < 50)
            {
                state = -1;
            }
        }
    }

    public void SetAIAnimationState(string state)
    {
        if (lastAnimationState != state)
        {
            if (state == "idle")
            {
                _AnimancerComponent.Play(_idle);
                _soundFX.PlayWalk(false);
            }

            if (state == "walkF")
            {
                _AnimancerComponent.Play(_walkForward);
                _soundFX.PlayWalk(true);
            }

            if (state == "dead")
            {
                _soundFX.PlayWalk(false);
                GetComponent<CharacterController>().enabled = false;
                //_AnimancerComponent.Animator.applyRootMotion = true;
                LevelController.instance.SetMinusPlayersCount();
                _WeaponController.StopShoot();
                _WeaponController.SetUnActiveWeapon();
                _AnimancerComponent.Play(_dead);
            }

            lastAnimationState = state;
        }
    }

    public void SetDamage(float damage)
    {
        if (!isDead)
        {
            if (enemyHealth > 0)
            {
                enemyHealth -= damage;
            }

            if (enemyHealth == 0)
            {
                isDead = true;
                SetAIAnimationState("dead");
                ai.canMove = false;
            }
            _soundFX.PlayDamage();
        }
    }

    public void SetDestantionToPoint(Vector3 pointToWay)
    {
        ai.destination = pointToWay;
        ai.canMove = true;
        SetAIAnimationState("walkF");
    }
}
        