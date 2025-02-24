using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Animancer;
using DG.Tweening;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    
    public enum EnemyType
    {
        small,
        big
    }
    
    public enum EnemyAttakType
    {
        near,
        further
    }

    public EnemyType _EnemyType;
    public EnemyAttakType _AttakType;
    public AnimancerComponent _AnimancerComponent;

    [SerializeField] private AnimationClip _Idle, _WalkForward, _Dead;
    [SerializeField] private EnemyDamageUI _damageUI;
    [SerializeField] private ClipTransition _Attack;


    public float enemyHealth=100;
    public float walkSpeed;
    public float distanceMag;
    public float enemyAttak;
    public int state;
    public Vector3 randomDestation;
    
    
    [HideInInspector] public IAstarAI ai;
    public bool isAtDestination;
    public Vector3 DestantionPoint;
    private Vector3 rPointToplayer;
    public LayerMask AiPaleyrlayer;
    public GameObject curentAttak;

    private AnimancerState _animationState;
    private string lastAnimationState;
    private Transform _characterVisual;

    public Transform shootPoint;
    public GameObject shotPrefab;
    public float shootForce;
    void Start()
    {
        ai = GetComponent<IAstarAI>();
        ai.maxSpeed = walkSpeed;

        
        SetAIAnimationState("walkF");
        state = 0;

        _characterVisual = _AnimancerComponent.transform;
    }
    
    void Update()
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

        if (enemyHealth > 0)
        {
            if (state != 2)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 30f, AiPaleyrlayer);
                if (hitColliders.Length > 0)
                {
                    if (curentAttak == null)
                    {
                        float distance = 100;
                        int id = -1;
                        for (int i = 0; i < hitColliders.Length; i++)
                        {
                            if (hitColliders[i].GetComponent<AIController>() != null)
                            {
                                if (hitColliders[i].GetComponent<AIController>().enemyHealth > 0)
                                {
                                    if (Vector3.Distance(hitColliders[i].transform.position, transform.position) <=
                                        distance)
                                    {
                                        distance = Vector3.Distance(hitColliders[i].transform.position,
                                            transform.position);
                                        id = i;
                                    }
                                }
                            }
                            else
                            {
                                if (PlayerController.instance._health > 0)
                                {
                                    if (Vector3.Distance(hitColliders[i].transform.position, transform.position) <=
                                        distance)
                                    {
                                        float yOffset = hitColliders[i].transform.position.y - transform.position.y;
                                        if (_AttakType == EnemyAttakType.near)
                                        {
                                            if (yOffset < 5f)
                                            {
                                                distance = Vector3.Distance(hitColliders[i].transform.position,
                                                    transform.position);
                                                id = i;
                                            }
                                        }
                                        else
                                        {
                                            if (yOffset < 15f)
                                            {
                                                distance = Vector3.Distance(hitColliders[i].transform.position,
                                                    transform.position);
                                                id = i;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (id != -1)
                        {
                            curentAttak = hitColliders[id].gameObject;
                            state = 2;
                        }
                    }
                }
            }

            if (state == 0)
            {
                float RandTime = Random.Range(2f, 4f);

                StartCoroutine(WaitIdle(RandTime));
                state = -1;
            }

            if (state == 1)
            {
                if (isAtDestination)
                {
                    state = 0;
                }
            }

            if (state == 2)
            {
                if (_AttakType == EnemyAttakType.near)
                {
                    Vector3 distance = curentAttak.transform.position - transform.position;
                    float yOffset = curentAttak.transform.position.y - transform.position.y;
                    if (yOffset > 1f && yOffset < 5f)
                    {
                        Vector3 newPos = new Vector3(curentAttak.transform.position.x, transform.position.y,
                            curentAttak.transform.position.z);
                        distance = newPos - transform.position;
                        if (distance.magnitude < distanceMag + 4)
                        {
                            state = 3;
                            ai.canMove = false;
                        }
                        else
                        {
                            if (rPointToplayer == Vector3.zero)
                            {
                                rPointToplayer = GetrandomPointInPlayer(curentAttak.transform);
                            }

                            SetDestantionToPoint(rPointToplayer);
                        }
                    }
                    else
                    {
                        if (yOffset < 5f)
                        {

                            if (distance.magnitude < distanceMag)
                            {
                                state = 3;
                                ai.canMove = false;
                            }
                            else
                            {
                                SetDestantionToPoint(curentAttak.transform.position);
                            }
                        }
                        else
                        {
                            state = 0;
                            curentAttak = null;
                        }
                    }
                }
                else
                {
                    Vector3 distance = curentAttak.transform.position - transform.position;
                    if (distance.magnitude < distanceMag)
                    {
                        state = 3;
                        ai.canMove = false;
                    }
                    else
                    {
                        SetDestantionToPoint(curentAttak.transform.position);
                    }
                }
            }

            if (state == 3)
            {
                
                SetAIAnimationState("attak");
                
                Vector3 distance = curentAttak.transform.position - transform.position;
                if (distance.magnitude > distanceMag)
                {
                    state = 2;
                }

                if (curentAttak != null)
                {
                    var lookPos = distance;
                    lookPos.y = 0f;
                    var rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = rotation;
                    if (curentAttak.GetComponent<AIController>() != null)
                    {
                        if (curentAttak.GetComponent<AIController>().enemyHealth == 0)
                        {
                            state = 0;
                            curentAttak = null;
                        }
                    }
                    else
                    {
                        if (PlayerController.instance._health <= 0)
                        {
                            state = 0;
                            curentAttak = null;
                        }
                    }
                }
            }
        }
        else
        {
            if(state!=-2)
            {
                ai.canMove = false;
                SetAIAnimationState("dead");
                state = -2;
            }
        }
        
    }

    public void Jump()
    {
        if (_EnemyType == EnemyType.small)
        {
            if (curentAttak != null)
            {
                float yOffset = curentAttak.transform.position.y;
                _characterVisual.DOLocalMoveY(yOffset, 0.2f).OnComplete(() =>
                {
                    _characterVisual.DOLocalMoveY(0, 0.3f);
                });
            }
        }
    }

    public void AttakEnemy()
    {
        if (curentAttak != null)
        {
            if (_AttakType == EnemyAttakType.near)
            {
                if (curentAttak.tag == "Player")
                {
                    PlayerController.instance.SetDamage(enemyAttak);
                }
                else
                {
                    curentAttak.GetComponent<AIController>().SetDamage(enemyAttak);
                }
            }
            else
            {
                GameObject bullet = Instantiate(shotPrefab, shootPoint.position, shootPoint.rotation);
                bullet.GetComponent<Granade>().damage = enemyAttak;
                Vector3 dirWithoutSpread = curentAttak.transform.position - shootPoint.position;
                bullet.transform.forward = dirWithoutSpread.normalized;
                bullet.GetComponent<Rigidbody>()
                    .AddForce(dirWithoutSpread.normalized * shootForce, ForceMode.Impulse);
            }
        }
    }

    IEnumerator WaitIdle(float randTime)
    {
        yield return new WaitForSeconds(randTime);
        if (state == -1 && state!=2)
        {
            state = 1;
            SetDestantionToPoint(GetRandomPoint());
        }
    }
    
    

    void OnTargetReached()
    {
        if(lastAnimationState=="walkF")
            SetAIAnimationState("idle");
        if(state==1)
            state = 0;
    }
    public void SetDamage(float damage, string playerName)
    {
        if(enemyHealth>0)
        {
            enemyHealth -= damage;
            _damageUI.SpawnDamageUI("-" + damage.ToString());
            if (enemyHealth <= 0)
            {
                if (playerName == "PlayerController")
                {
                    PlayerController.instance.playerKills++;
                }
                else
                {
                    LevelController.instance.SetKillsAI(playerName);
                }
            }
        }
    }
    public void SetDestantionToPoint(Vector3 pointToWay)
    {
        ai.destination = pointToWay;
        ai.canMove = true;

        SetAIAnimationState("walkF");
        ai.maxSpeed = walkSpeed;
        
    }
    
    Vector3 GetRandomPoint()
    {
        float randX = Random.Range(-55f, 100f);
        float randZ = Random.Range(-80f, 75f);
        Vector3  vector = new Vector3(0,0,0);
        vector.x = randX;
        vector.z = randZ;
        DestantionPoint = vector;
        return vector;
    }

    Vector3 GetrandomPointInPlayer(Transform player)
    {
        var vector3 =player.position+ Random.insideUnitSphere.normalized * 3;
        Vector3 newPos = new Vector3(vector3.x, 0, vector3.z);
        return newPos;
    }
    
    public void SetAIAnimationState(string state)
    {
        if (lastAnimationState != state)
        {
            if (state == "idle")
            {
                _AnimancerComponent.Play(_Idle);
            }
            if (state == "walkF")
            {
                _AnimancerComponent.Play(_WalkForward);
            }
            if (state == "dead")
            {
                GetComponent<CharacterController>().enabled = false;
                //_AnimancerComponent.Animator.applyRootMotion = true;
                if (_EnemyType == EnemyType.small)
                {
                    LevelController.instance.RemoveEnemyCount();
                }
                else
                {
                    LevelController.instance.RemoveEnemyCount(true);
                }
                
                _AnimancerComponent.Play(_Dead);
                StartCoroutine(DalayDead());
            }
            if (state == "attak")
            {
                _AnimancerComponent.Play(_Attack);
            }
            lastAnimationState = state;
        }
    }

    IEnumerator DalayDead()
    {
        yield return new WaitForSeconds(2f);
        transform.DOMoveY(-5f, 2f).OnComplete(DestroyThis);
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
