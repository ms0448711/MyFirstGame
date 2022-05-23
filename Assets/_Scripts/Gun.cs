using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletToFire;
    public Transform firePoint;

    public float timeBetweenShots;
    private float shotCounter;

    public string weaponName;
    public Sprite gunUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.canMove && !LevelManager.instance.isPaused)
        {
            #region Shoot Bullet
            if (shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                {
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                    shotCounter = timeBetweenShots;

                    AudioManager.instance.PlaySFX(AudioManager.AudioCode.Shoot1);
                }



                /*
                if (Input.GetMouseButton(0))
                {
                    shotCounter -= Time.deltaTime;
                    if (shotCounter <= 0)
                    {
                        Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                        shotCounter = timeBetweenShots;

                        AudioManager.instance.PlaySFX(AudioManager.AudioCode.Shoot1);
                    }
                }
                */
            }
            #endregion
        }

    }
}
