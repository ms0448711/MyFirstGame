using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    /// <summary>
    /// How many seconds to wait for the next level to load.
    /// </summary>
    public float waitToLoad = 4f;

    public string nextLevel;

    public bool isPaused;

    public int currentCoins;

    public Transform startPoint;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.transform.position = startPoint.position;
        PlayerController.instance.canMove = true;

        currentCoins = CharacterTracker.instace.currentCoins;

        Time.timeScale = 1f;

        UpdateCoin();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public IEnumerator LevelEnd()
    {
        AudioManager.instance.PlayLevelWin();

        PlayerController.instance.canMove = false;

        UIController.instace.StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        CharacterTracker.instace.maxHealth = PlayerHealthContrller.instance.maxHealth;
        CharacterTracker.instace.currentHealth = PlayerHealthContrller.instance.currentHealth;
        CharacterTracker.instace.currentCoins = currentCoins;

        SceneManager.LoadScene(nextLevel);
    }

    public void PauseUnpause()
    {
        if (!isPaused)
        {
            UIController.instace.pauseMenu.SetActive(true);

            isPaused = true;

            Time.timeScale = 0f;
        }
        else
        {
            UIController.instace.pauseMenu.SetActive(false);

            isPaused = false;

            Time.timeScale = 1f;
        }
    }

    public void GetCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoin();
    }

    public void SpendCoin(int amount)
    {
        currentCoins -= amount;

        if (currentCoins < 0)
        {
            Debug.LogWarning("Currency below zero!");
            currentCoins = 0;
        }
        UpdateCoin();
    }

    public void UpdateCoin()
    {
        UIController.instace.coinText.text = currentCoins.ToString();
    }
}
