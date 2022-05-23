using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float moveSpeed;

    public Transform target;

    public Camera mainCamera, bigMapCamera;

    private bool bigMapActive=false;
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
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetBigMapActive(!bigMapActive);
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetBigMapActive(bool isActive)
    {
        if (!LevelManager.instance.isPaused)
        {
            bigMapActive = isActive;

            bigMapCamera.enabled = isActive;
            mainCamera.enabled = !isActive;

            PlayerController.instance.canMove = !isActive;
            Time.timeScale = isActive ? 0f : 1f;

            UIController.instace.mapDisplay.SetActive(!isActive);
            UIController.instace.bigMapText.SetActive(isActive);
            
        }
    }
}
