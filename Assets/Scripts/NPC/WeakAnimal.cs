using UnityEditor.Rendering;
using UnityEngine;

public class WeakAnimal : Animal
{
    public void Run(Vector3 targetPos)
    {
        Vector3 temp = transform.position - targetPos;
        destination = new Vector3(temp.x, 0f, temp.z).normalized;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        nav.speed = runSpeed;
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
