using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField]
    private PlayerAnimationController _animator;
    private string lastAnimationState;

    private float startHealth, startArmor;
    public float _health, _armor;
    
    
    [SerializeField]
    private AIMUIController _aimImage;

    public bool isAIMMode;
    public LayerMask aimLayerMask;

    private bool isAIMCamera;
    public Camera _camera;

    [SerializeField]
    private CMF.CameraMouseInput mouseInput;

    [SerializeField] 
    private CMF.CameraDistanceRaycaster cameraRaycaster;

    [SerializeField] 
    private CMF.CharacterKeyboardInput jumpInput;

    public bool isDead, isShootButton;

    [HideInInspector] public int playerKills;

    private Vector3 startPos;

    public Transform grenadeSpawnPoint;
    public GameObject grenadePrefab;
    public SFXPlayer _soundFX;

    private float timerShoot;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetPlayerAnimationState("idle");
        startArmor = 100;
        startHealth = 100;
        int currentArmor = PlayerData.instance.shieldCount;
        if (currentArmor > 0)
        {
            _armor = 100f;
            PlayerData.instance.shieldCount-=1;
        }

        SetHealthPercent();
        startPos = transform.position;
    }
    

    public void SetHealthPercent()
    {
        float percentHealth = _health / startHealth;
        float percentArmor = _armor / startArmor;
        
        UIController.instance.SetHealth(percentHealth);
        UIController.instance.SetArmor(percentArmor);
    }

    public void SetButtonShoot(bool isShoot)
    {
        if (!isAIMMode)
        {
            timerShoot = 0;
            isShootButton = true;
        }
        else
        {
            isShootButton = isShoot;
        }
    }

    void Update()
    {
        if (!isDead)
        {
            if (jumpInput._joystikMove.Horizontal != 0 || jumpInput._joystikMove.Vertical != 0)
            {
                _soundFX.PlayWalk(true);
            }
            else
            {
                _soundFX.PlayWalk(false);
            }
            if (isAIMMode)
            {
                Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                if (isShootButton)
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "Enemy")
                        {
                            _aimImage.SetAIMColor(1);
                            WeaponController.instance.StartShoot(hit.transform.gameObject);
                        }
                        else
                        {
                            _aimImage.SetAIMColor(0);
                            WeaponController.instance.StartShoot(null);
                        }
                    }
                }
                else
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "Enemy")
                        {
                            _aimImage.SetAIMColor(1);
                            WeaponController.instance.StartShoot(hit.transform.gameObject);
                        }
                        else
                        {
                            _aimImage.SetAIMColor(0);
                            WeaponController.instance.StopShoot();
                        }
                    }
                    else
                    {
                        _aimImage.SetAIMColor(0);
                        WeaponController.instance.StopShoot();
                    }
                }
            }
            else
            {
                Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                if (isShootButton)
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "Enemy")
                        {
                            _aimImage.SetAIMColor(1);
                            WeaponController.instance.StartShoot(hit.transform.gameObject);
                        }
                        else
                        {
                            _aimImage.SetAIMColor(0);
                            WeaponController.instance.StartShoot(null);
                        }
                    }

                    if (timerShoot < 0.1f)
                    {
                        timerShoot += 1f * Time.deltaTime;
                    }
                    else
                    {
                        timerShoot = 0;
                        isShootButton = false;
                    }
                }
                else
                {
                    _aimImage.SetAIMColor(0);
                    WeaponController.instance.StopShoot();
                }
            }
        }
    }

    public void SetPlayerAnimationState(string state)
    {
        if (lastAnimationState != state)
        {
            if (state == "idle")
            {
                _animator.PlayIdle();
            }
            if (state == "walkF")
            {
                _animator.PlayWalkF();
            }
            if (state == "walkB")
            {
                _animator.PlayWalkB();
            }
            if (state == "dead")
            {
                //_animator._animancer.Animator.applyRootMotion = true;
                _animator.PlayDead();
            }

            lastAnimationState = state;
        }
    }

    public void SetAimCamera()
    {
        if(isAIMCamera)
        {
            //_camera.DOFieldOfView(60, 0.5f);
            cameraRaycaster.isFPS = true;
            //mouseInput.mouseInputMultiplier = 0.0001f;
            isAIMCamera = false;
        }
        else
        {
            //_camera.DOFieldOfView(15, 0.5f);
            cameraRaycaster.isFPS = false;
            //mouseInput.mouseInputMultiplier = 0.001f;
            isAIMCamera = true;
        }
    }

    public void Jump()
    {
        jumpInput.isJump = true;
        _soundFX.PlayJump();
        StartCoroutine(WaitAfterJump());
    }

    IEnumerator WaitAfterJump()
    {
        yield return new WaitForSeconds(0.5f);
        jumpInput.isJump = false;
    }

    public void SetDamage(float damage)
    {
        if (!isDead)
        {
            if (_armor > 0)
            {
                if (_armor < damage)
                {
                    float ost = damage - _armor;
                    _armor = 0f;
                    _health -= ost;
                }
                else
                {
                    _armor -= damage;
                }
            }
            else if (_health > 0)
            {
                if (_health < damage)
                {
                    SetDead();
                }
                else
                {
                    _health -= damage;
                    if (_health <= 0)
                    {
                        SetDead();
                    }
                }
            }
            else
            {
                SetDead();
            }
            UIController.instance.SetVisualDamage();
            _soundFX.PlayDamage();
            SetHealthPercent();
        }
    }

    public void SetHealing(float healingValue)
    {
        _health += healingValue;
        SetHealthPercent();
    }

    void SetDead()
    {
        _health = 0f;
        isDead = true;
        
        SetPlayerAnimationState("dead");
        WeaponController.instance.SetDead();
        StartCoroutine(WaitPerDead());
    }

    public void PlayerRevive()
    {
        WeaponController.instance.SetActiveWeapon();
        SetPlayerAnimationState("idle");
        transform.position = startPos;
        _health = 50f;
        isDead = false;
    }

    IEnumerator WaitPerDead()
    {
        yield return new WaitForSeconds(1f);
        UIController.instance.OpenPanelDead();
    }

    public void SetConsumable(string itemName)
    {
        if (itemName == "firstaid")
        {
            _health += 25f;
            SetHealthPercent();
        }
        
        if (itemName == "grenade")
        {
            GameObject grenade = Instantiate(grenadePrefab, grenadeSpawnPoint.position, grenadeSpawnPoint.rotation);
            grenade.GetComponent<Rigidbody>().AddForce(grenadeSpawnPoint.forward * 1000);
        }
    }
}
