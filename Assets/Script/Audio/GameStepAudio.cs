using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStepAudio : MonoBehaviour
{
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource bgm;

    private void Start()
    {
        bgm.Play();

        foreach (var source in sfx)
        {
            if (source.clip.loadState != AudioDataLoadState.Loaded)
            {
                source.clip.LoadAudioData();
            }
        }
    }

    public void PlaySFX(int _sfxIndex)
    {
        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _sfxIndex) => sfx[_sfxIndex].Stop();
}
