using UnityEngine;

public class WeakAnimal : Animal
{
    public void Run(Vector3 targetPos)
    {
        direction = Quaternion.LookRotation(transform.position - targetPos).eulerAngles;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        animator.SetBool("Running", isRunning);
    }

    public override void Damaged(int damage, Vector3 targetPos)
    {
        base.Damaged(damage, targetPos);
        if (!isDead)
        {
            Run(targetPos);
        }
    }
}
