using UnityEngine;

// Sound 클래스 : 이름과 오디오 클립을 저장
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    public string[] playSoundName;          // 플레이 중인 사운드 이름

    [SerializeField] public Sound[] effectSounds;
    [SerializeField] public Sound[] bgmSounds;

    #region singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion singleton

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    // 이름을 통해 사운드 재생
    public void PlaySE(string name)
    {
        foreach (var sound in effectSounds)
        {
            if (name == sound.name)
            {
                for (int i = 0; i< audioSourceEffects.Length; i++)
                {
                    if (!audioSourceEffects[i].isPlaying)
                    {
                        playSoundName[i] = name;
                        audioSourceEffects[i].clip = sound.clip;
                        audioSourceEffects[i].Play();
                        return;
                    }
                }
                Debug.Log("All audio sources are busy playing other sounds.");
                return;
            }
        }
        Debug.LogWarning("Sound not found: " + name);
    }

    public void StopAllSE()
    {
        foreach (var audioSource in audioSourceEffects)
        {
            audioSource.Stop();
        }
    }

    public void StopSE(string name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == name)
            {
                audioSourceEffects[i].Stop();
                break;
            }
        }

    }
}
