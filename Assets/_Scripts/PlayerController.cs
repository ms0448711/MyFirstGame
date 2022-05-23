using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    private Vector2 moveInput;

    public Rigidbody2D theRB;

    public Transform gunArm;

    //private Camera theCam;

    public Animator anim;
    /*
    public GameObject bulletToFire;
    public Transform firePoint;

    public float timeBetweenShots;
    private float shotCounter;
    */
    public SpriteRenderer bodySR;

    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = .5f, dashCooldown = 1f, dashInvinciblity = .5f;
    private float dashCounter, dashCoolCounter;
    public bool IsDashing { get => dashCounter > 0; }

    [HideInInspector]
    public bool canMove = true;

    public List<Gun> availableGuns = new List<Gun>();
    [HideInInspector]
    public int currentGun;
    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //theCam = Camera.main;
        

        activeMoveSpeed = moveSpeed;

        UIController.instace.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instace.gunText.text = availableGuns[currentGun].weaponName;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {
            #region Player Control
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            //transform.position += new Vector3(moveInput.x * Time.deltaTime * moveSpeed, moveInput.y * Time.deltaTime * moveSpeed, 0);

            theRB.velocity = moveInput * activeMoveSpeed;
            #endregion

            #region Rotate Gun Arm
            //Get mouse postion
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);

            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                gunArm.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
                gunArm.localScale = Vector3.one;
            }

            //rotate gun arm
            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(0, 0, angle);
            #endregion

            /*
            #region Shoot Bullet
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                shotCounter = timeBetweenShots;

                AudioManager.instance.PlaySFX(AudioManager.AudioCode.Shoot1);
            }

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
            #endregion
            */

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (availableGuns.Count > 0)
                {
                    currentGun++;
                    currentGun %= availableGuns.Count;
                    SwitchGun();
                }
                else
                {
                    Debug.LogError("Player has no guns!");
                }
            }

            #region Dash
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    anim.SetTrigger("dash");

                    PlayerHealthContrller.instance.MakeInvincible(dashInvinciblity);

                    AudioManager.instance.PlaySFX(AudioManager.AudioCode.PlayerDash);
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;//翻滾完後，進入翻滾冷卻時間
                }
            }

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }

            #endregion

            #region Moving Animation
            if (moveInput != Vector2.zero)
                anim.SetBool("isMoving", true);
            else
                anim.SetBool("isMoving", false);
            #endregion
        }
        else
        {
            theRB.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }
    }

    public void SwitchGun()
    {
        foreach(Gun theGun in availableGuns)
        {
            theGun.gameObject.SetActive(false);
        }
        availableGuns[currentGun].gameObject.SetActive(true);

        UIController.instace.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instace.gunText.text = availableGuns[currentGun].weaponName;
    }
}
