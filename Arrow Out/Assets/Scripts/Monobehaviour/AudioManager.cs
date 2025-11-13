using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Private
    [Header("======  Audio Source  ======")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    // Public
    [Header("======  Audio Clip  ======")]
    public AudioClip background;
    public AudioClip buttonClick;
    public AudioClip arrowClick;
    public AudioClip wroungArrowClick;
    public AudioClip levelPass;
    public AudioClip levelFail;
    public AudioClip congratulation;

    private void Awake()
    {
        // Check if there's already an instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    private void Start()
    {
        // Background Music
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip); // To play onshot sound with different sounds
    }

    public void ButtonClick()
    {
        SFXSource.PlayOneShot(buttonClick); // Button Click sound
    }
}
