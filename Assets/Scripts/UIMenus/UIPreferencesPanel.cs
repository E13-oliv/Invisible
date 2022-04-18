using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPreferencesPanel : MonoBehaviour
{
    // managers
    private GameManager GM;
    private AudioManager AM;

    [Header("Preferences")]
    [SerializeField]
    private Text musicVolumeUI;
    [SerializeField]
    private Button musicVolumeUp;
    [SerializeField]
    private Button musicVolumeDown;
    private int musicVolume;
    [SerializeField]
    private Text sfxVolumeUI;
    [SerializeField]
    private Button sfxVolumeUp;
    [SerializeField]
    private Button sfxVolumeDown;
    private int sfxVolume;

    private static int minVolume = 0;
    private static int maxVolume = 5;

    private void Start()
    {
        // find GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        // find AudioManager
        GameObject audioManagerGameObject = GameObject.Find("AudioManager");
        AM = audioManagerGameObject.GetComponent<AudioManager>();

        // display volume in preferences panel
        musicVolume = GM.GetMusicVolume();
        sfxVolume = GM.GetSfxVolume();

        musicVolumeUI.text = musicVolume.ToString();
        sfxVolumeUI.text = sfxVolume.ToString();
    }

    public void ChangeMusicVolume(int volumeChange)
    {
        musicVolume = musicVolume + volumeChange;

        musicVolume = Mathf.Clamp(musicVolume, minVolume, maxVolume);

        musicVolumeUI.text = musicVolume.ToString();
        GM.SetMusicVolume(musicVolume);
        AM.SetVolume("MusicVol", musicVolume);
    }
    public void ChangeSfxVolume(int volumeChange)
    {
        sfxVolume = sfxVolume + volumeChange;

        sfxVolume = Mathf.Clamp(sfxVolume, minVolume, maxVolume);

        sfxVolumeUI.text = sfxVolume.ToString();
        GM.SetSfxVolume(sfxVolume);
        AM.SetVolume("SfxVol", sfxVolume);
    }
}
