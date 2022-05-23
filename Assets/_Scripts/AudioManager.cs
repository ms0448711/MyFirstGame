using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource levelMusic, gameOverMusic, winMusic;

    public AudioSource[] sfx;

    public enum AudioCode
    {
        BoxBreaking,
        EnemyDeath,
        EnemyHurt,
        Explosion,
        Impact,
        PickupCoin,
        PickupGun,
        PickupHealth,
        PlayerDash,
        PlayerDeath,
        playerDie,
        PlayerHurt,
        Shoot1,
        Shoot2,
        Shoot3,
        Shoot4,
        Shoot5,
        Shoot6,
        ShopBuy,
        ShopNotEnough,
        WarpOut
    }

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGameOver()
    {
        levelMusic.Stop();

        gameOverMusic.Play();
    }

    public void PlayLevelWin()
    {
        levelMusic.Stop();

        winMusic.Play();
    }

    public void PlaySFX(AudioCode sfxToPlay)
    {
        sfx[((int)sfxToPlay)].Stop();
        sfx[((int)sfxToPlay)].Play();
    }
}
