using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float range;
    public float accuracy;
    public float fireRate;
    public float reloadTime;
    public int damage;

    public int ammoCapacity;    // 한 탄창의 최대 탄약
    public int currentAmmo;     // 현재 탄창에 있는 탄약
    public int maxAmmo;         // 최대 보유 가능 탄약
    public int carryAmmo;       // 현재 보유 중인 탄약

    public float retroActionForce;
    public float retroActionFineSightForce;
    public Vector3 fineSightOriginPos;

    public Animator animator;
    public ParticleSystem muzzleFlash;
    public AudioClip fireSound;
}
