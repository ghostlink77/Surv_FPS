using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class GunController : MonoBehaviour
{
    [SerializeField] private Gun currentGun;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject hitEffectPrefab;

    private float currentFireRate;
    private bool isReloading = false;
    [HideInInspector] public bool isFineSightMode = false;

    private AudioSource audioSource;
    private RaycastHit hitInfo;
    private Vector3 originPos;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originPos = Vector3.zero;
    }
    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
    }

    // 연사속도 계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReloading)
        {
            Fire();
        }
    }
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReloading)
        {
            FineSight();
        }
    }
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentGun.currentAmmo < currentGun.ammoCapacity)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }


    private void Fire()
    {
        if (!isReloading)
        {
            if (currentGun.currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
        
    }
    private void Shoot()
    {
        currentGun.currentAmmo--;
        currentFireRate = currentGun.fireRate;
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
        Hit();

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());

        Debug.Log("Fired");
    }
    private void Hit()
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo, currentGun.range))
        {
            var clone = Instantiate(hitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.animator.SetBool("FineSightMode", isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }
    public void CancelFineSight()
    {
        if (isFineSightMode)
        {
            FineSight();
        }
    }

    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryAmmo > 0)
        {
            isReloading = true;
            currentGun.animator.SetTrigger("Reload");

            currentGun.carryAmmo += currentGun.currentAmmo;
            currentGun.currentAmmo = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryAmmo >= currentGun.ammoCapacity)
            {
                currentGun.currentAmmo = currentGun.ammoCapacity;
                currentGun.carryAmmo -= currentGun.ammoCapacity;
            }
            else
            {
                currentGun.currentAmmo = currentGun.carryAmmo;
                currentGun.carryAmmo = 0;
            }
            isReloading = false;
        }
        else
        {
            Debug.Log("소유한 탄약이 없습니다.");
        }
    }

    // 정조준 활성화/비활성화
    IEnumerator FineSightActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }
    // 레트로 액션(반동)
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.5f);
                yield return null;
            }
            while (currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.5f);
                yield return null;
            }
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    private void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
