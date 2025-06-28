using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject goCrosshairHUD;
    [SerializeField] private GunController gunController;

    private float gunAccuracy;          // 크로스헤어에 따른 정확도

    // Update is called once per frame
    void Update()
    {
        
    }
    public void WalkingAnimation(bool flag)
    {
        WeaponManager.currentWeaponAnimator.SetBool("Walk", flag);
        animator.SetBool("Walking", flag);
    }
    public void RunningAnimation(bool flag)
    {
        WeaponManager.currentWeaponAnimator.SetBool("Run", flag);
        animator.SetBool("Running", flag);
    }
    public void JumpingAnimation(bool flag)
    {
        animator.SetBool("Running", flag);
    }
    public void CrouchingAnimation(bool flag)
    {
        animator.SetBool("Crouching", flag);
    }
    public void FineSightAnimation(bool flag)
    {
        animator.SetBool("FineSight", flag);
    }
    public void FireAnimation()
    {
        if (animator.GetBool("Walking"))
        {
            animator.SetTrigger("WalkFire");
        }
        else if (animator.GetBool("Crouching"))
        {
            animator.SetTrigger("CrouchFire");
        }
        else
        {
            animator.SetTrigger("IdleFire");
        }
    }

    public float GetAccuracy()
    {
        if (animator.GetBool("Walking"))
        {
            gunAccuracy = 0.04f;
        }
        else if (animator.GetBool("Crouching"))
        {
            gunAccuracy = 0.01f;
        }
        else if (gunController.GetFineSightMode())
        {
            gunAccuracy = 0.001f;
        }
        else
        {
            gunAccuracy = 0.02f;
        }

        return gunAccuracy;
    }
}
