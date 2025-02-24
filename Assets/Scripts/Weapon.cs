using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isAI;
    public AIWeaponController _AIWeaponController;
    public enum WeaponType
    {
        melee,
        pistol,
        shutgun,
        automaticRifle,
        rifle,
        grenadeLauncher
    }

    public WeaponType _WeaponType;
    public bool isDoobleShootMelee;
    public float weaponFireRate, reloadSpeed;
    public int weaponDamage;
    public int capacity, magazines;
    public float shootForce;
    private int allAmmo;
    private int countShooted;
    private bool isReload;
    private float timer;
    public GameObject bulletPrefab, visualAmmo;
    public Transform spawnPoint;
    public ParticleSystem flash;
    public SFXPlayer _soundFX;
    
    private AnimancerComponent anim;
    public AnimationClip idle, shoot, reload;
    public AudioClip shootClip;
    private string _AnimationState;
    
    public bool isCanShoot;
    private Camera mainCamera;
    [HideInInspector] public GameObject enemy;
    public bool isShoot, isAttakEnd;

    private SFXSaw _sfxSaw;
    private bool isSaw;

    [HideInInspector] public Transform lTarget, rTarget;
    [HideInInspector] public ArsenalUISlot _PlayerUISlot;
    private void Awake()
    {
        timer = weaponFireRate;
        
        anim = GetComponent<AnimancerComponent>();
        

        lTarget = transform.Find("targetRigHandL");
        rTarget = transform.Find("targetRigHandR");
        Init();
    }

    void Init()
    {
        if (!isAI)
            mainCamera = Camera.main;
        if (allAmmo == 0)
        {
            allAmmo = magazines * capacity;
        }

        isCanShoot = true;
        isReload = false;
        PlayAnimation("idle");
        if (gameObject.name == "Chainsaw")
        {
            _sfxSaw = GetComponentInChildren<SFXSaw>();
            _sfxSaw.PlaySawIdle();
            isSaw = true;
        }
        else
        {
            isSaw = false;
        }

        if (_PlayerUISlot != null)
        {
            CheckAmmoText();
        }

    }

    public void CheckAmmoText()
    {
        if (allAmmo == 0)
        {
            allAmmo = magazines * capacity;
        }

        string ammoText = (capacity - countShooted).ToString() + "/" + allAmmo.ToString();
        _PlayerUISlot.SetAmmoCountText(ammoText);

    }

    public void PlayAnimation(string newState)
    {
        
            if (newState != _AnimationState)
            {
                if (newState == "idle")
                {
                    if (_WeaponType == WeaponType.melee)
                    {
                        anim.Play(idle, 0.25f);
                    }
                    else
                    {
                        anim.Play(idle);
                    }
                }

                if (newState == "shoot")
                {
                   
                    if (_WeaponType == WeaponType.melee && !isDoobleShootMelee)
                    {
                        var state = anim.Play(shoot,0.25f);
                        state.Events.Add(0.5f,PlaySFXShoot);
                        state.Events.OnEnd = () => { isAttakEnd = true; };
                    }else if(_WeaponType == WeaponType.melee)
                    {
                        var state = anim.Play(shoot,0.25f);
                        if (!isSaw)
                        {
                            state.Events.Add(0.25f, SetDoubleDamage);
                            state.Events.Add(0.65f, SetDoubleDamage);
                        }

                        state.Events.OnEnd = () => { isAttakEnd = true; };
                    }
                    else if (_WeaponType != WeaponType.grenadeLauncher)
                    {
                        
                        var state = anim.Play(shoot);
                        state.Speed = 2f;
                        state.Events.OnEnd = () => { PlayAnimation("idle"); };
                    }
                    else
                    {
                        var state = anim.Play(shoot);
                        state.Events.OnEnd = () => { PlayAnimation("idle"); };
                    }
                    
                }

                if (newState == "reload")
                {
                    anim.Play(reload);
                }

                _AnimationState = newState;
            }
        
    }

    public void Shoot()
    {
        if (isCanShoot)
        {
            if (_WeaponType == WeaponType.melee)
            {
                if (isSaw)
                {
                    SetDamageEnemyMelee();
                }
                if (_AnimationState == "idle")
                {
                    PlayVFXShoot();
                }
            }
            else
            {
                PlaySFXShoot();
                PlayVFXShoot();
                CheckAmmo();
                if (bulletPrefab != null)
                {
                    if (!isAI)
                    {
                        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                        RaycastHit hit;

                        Vector3 targetPoint;
                        if (Physics.Raycast(ray, out hit))
                            targetPoint = hit.point;
                        else
                            targetPoint = ray.GetPoint(75);
                        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
                        bullet.GetComponent<Bullet>().damage = weaponDamage;
                        Vector3 dirWithoutSpread = targetPoint - spawnPoint.position;
                        bullet.transform.forward = dirWithoutSpread.normalized;

                        bullet.GetComponent<Bullet>().parentName = "PlayerController";
                        bullet.GetComponent<Rigidbody>()
                            .AddForce(dirWithoutSpread.normalized * shootForce, ForceMode.Impulse);
                    }
                    else
                    {
                        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
                        bullet.GetComponent<Bullet>().damage = weaponDamage;
                        Vector3 dirWithoutSpread = enemy.transform.position - spawnPoint.position;
                        bullet.transform.forward = dirWithoutSpread.normalized;
                        bullet.GetComponent<Bullet>().parentName = _AIWeaponController.gameObject.name;
                        bullet.GetComponent<Rigidbody>()
                            .AddForce(dirWithoutSpread.normalized * shootForce, ForceMode.Impulse);
                    }

                    
                    if (visualAmmo != null)
                    {
                        visualAmmo.SetActive(false);
                    }
                }
                else
                {
                    SetDamageEnemyMelee();
                }
            }
        }
        else
        {
            PlaySFXEmpty();
            if (isAI)
            {
                _AIWeaponController.ChangeWeaponNoAmmo();
            }
        }
    }

    
    void SetDamageEnemyMelee()
    {
        if (enemy != null)
        {
            if (!isAI)
            {
                enemy.GetComponent<EnemyController>().SetDamage(weaponDamage, "PlayerController");
            }
            else
            {
                enemy.GetComponent<EnemyController>().SetDamage(weaponDamage, _AIWeaponController.gameObject.name);
            }
        }
    }

    void SetDoubleDamage()
    {
        SetDamageEnemyMelee();
        PlaySFXShoot();
    }
    void CheckAmmo()
    {
        countShooted++;
        if (countShooted == capacity)
        {
            isCanShoot = false;
            if (allAmmo > 0)
            {
                StartCoroutine(WaitAnimShooterPerReload());
            }
        }
        if(!isAI)
            CheckAmmoText();
    }
    IEnumerator WaitAnimShooterPerReload()
    {
        yield return new WaitWhile(() => _AnimationState != "idle");
        isReload = true;
        timer = 0f;
        PlayVSFXReload();
    }

    void PlayVSFXReload()
    {
        _soundFX.PlayChangeAmmo();
        PlayAnimation("reload");
    }

    void PlayVFXShoot()
    {
        PlayAnimation("shoot");
        if (flash != null)
        {
            if (flash.isPlaying)
            {
                flash.Stop();
                flash.Play(true);
            }
            else
            {
                flash.Play(true);
            }
        }
    }
    void PlaySFXShoot()
    {
        if (shootClip != null)
        {
            _soundFX.PlayCustomClip(shootClip);
        }

        if (_sfxSaw != null)
        {
            _sfxSaw.PlaySawAttak();
        }
    }

    void PlaySFXEmpty()
    {
        _soundFX.PlayNoAmmoShoot();
    }

    void Update()
    {
        if (!isReload)
        {
            if (!isAI)
            {
                if (isShoot)
                {
                    if(isAttakEnd) isAttakEnd = false;
                    if (timer < weaponFireRate)
                    {
                        timer += 1f * Time.deltaTime;
                    }
                    else
                    {
                        Shoot();
                        timer = 0f;
                    }
                }
                else
                {
                    if (timer < weaponFireRate)
                    {
                        timer += 1f * Time.deltaTime;
                    }
                    if (_WeaponType == WeaponType.melee)
                    {
                        if (isAttakEnd)
                        {
                            isAttakEnd = false;
                            PlayAnimation("idle");
                            if (_sfxSaw != null)
                            {
                                _sfxSaw.PlaySawIdle();
                            }
                        }
                    }
                }
            }
            else
            {
                if (_AIWeaponController.isShoot)
                {
                    if(isAttakEnd) isAttakEnd = false;
                    if (timer < weaponFireRate)
                    {
                        timer += 1f * Time.deltaTime;
                    }
                    else
                    {
                        Shoot();
                        timer = 0f;
                    }
                }else
                {
                    if (timer < weaponFireRate)
                    {
                        timer += 1f * Time.deltaTime;
                    }
                    if (_WeaponType == WeaponType.melee)
                    {
                        if (isAttakEnd)
                        {
                            isAttakEnd = false;
                            PlayAnimation("idle");
                            if (_sfxSaw != null)
                            {
                                _sfxSaw.PlaySawIdle();
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (timer < reloadSpeed)
            {
                timer+=1f*Time.deltaTime;
            }
            else
            {
                if (allAmmo > 0)
                {
                    if (allAmmo >= capacity)
                    {
                        if (countShooted == capacity)
                        {
                            allAmmo -= capacity;
                            countShooted = 0;
                        }
                        else
                        {
                            allAmmo -= countShooted;
                            countShooted = 0;
                        }
                    }
                    else
                    {
                        countShooted -= allAmmo;
                        allAmmo = 0;
                    }

                    timer = 0f;
                    isCanShoot = true;
                    isReload = false;
                    if(!isAI)
                        CheckAmmoText();
                }
            }
        }
    }

    public void Reload()
    {
        if (allAmmo > 0)
        {
            isCanShoot = false;
            isReload = true;
            timer = 0f;
            PlayVSFXReload();
        }
    }

    public bool isAmmoEmpty()
    {
        if (allAmmo == 0 && capacity == countShooted)
        {
            return true;
        }

        return false;
    }
    public void SetShootInController(bool _isShoot)
    {
        isShoot = _isShoot;
    }

    public void AddAmmoCount()
    {
        if (_WeaponType != WeaponType.melee)
        {
            Debug.Log(allAmmo+": +"+((magazines * capacity) / 5));
            allAmmo += (magazines * capacity) / 5;
            CheckAmmoText();
        }
    }
}
