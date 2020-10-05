using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_musicPlayer;
    [SerializeField]
    private AudioSource m_soundPlayer;
    // public AudioClip Background;
    private static AudioManager m_instance;
    private const string m_prefabPath = "AudioManager";
    public static AudioManager Instance{
        get{
            if (m_instance is null){
                m_instance = FindObjectOfType<AudioManager>();
                if (m_instance is null){
                    var obj = Resources.Load(m_prefabPath);
                    var singleton = (GameObject)Instantiate(obj);
                    m_instance = singleton.GetComponent<AudioManager>();
                    singleton.name = $"AudioManagerInstance";
                }
            }
            return m_instance;
        }
    }

    private void Awake() {
        DontDestroyOnLoad(this);
    }

    public void PlayMusic(){
        if (m_musicPlayer.isPlaying){
            return;
        }
        m_musicPlayer.Play();
    }

    public void PlaySound(AudioClip clip){
        m_soundPlayer.PlayOneShot(clip);
    }

    public void SetSoundVolume(float vol)
    {
        m_soundPlayer.volume = vol;
    }

    public void StopSound()
    {
        m_soundPlayer.Stop();
    }

}
