using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] int destroyTime;
    [SerializeField] SphereCollider collider;
    [SerializeField] GameObject go_rock;
    [SerializeField] GameObject go_debris;
    [SerializeField] GameObject go_effectPrefab;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip effectSound1;
    [SerializeField] AudioClip effectSound2;

    public void Mining()
    {
        audioSource.clip = effectSound1;
        audioSource.Play();
        var clone = Instantiate(go_effectPrefab, collider.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);

        hp--;
        if (hp <= 0)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        audioSource.clip = effectSound2;
        audioSource.Play();

        collider.enabled = false;
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
