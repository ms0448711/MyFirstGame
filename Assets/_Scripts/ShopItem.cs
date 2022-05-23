using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public GameObject buyMessage;

    private bool inBuyZone;

    public bool isHealthRestore, isHealthUpgrade, isWeapon;

    public int itemCost;

    public int healthUpgradeAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inBuyZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (LevelManager.instance.currentCoins >= itemCost)
                {
                    LevelManager.instance.SpendCoin(itemCost);

                    if (isHealthRestore)
                    {
                        PlayerHealthContrller.instance.HealPlayer(PlayerHealthContrller.instance.maxHealth);
                    }
                    if(isHealthUpgrade)
                    {
                        PlayerHealthContrller.instance.IncreaseMaxHealth(healthUpgradeAmount);
                    }

                    gameObject.SetActive(true);
                    inBuyZone = false;

                    AudioManager.instance.PlaySFX(AudioManager.AudioCode.ShopBuy);
                }
                else
                {
                    AudioManager.instance.PlaySFX(AudioManager.AudioCode.ShopNotEnough);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buyMessage.SetActive(true);

            inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buyMessage.SetActive(false);

            inBuyZone = false;
        }
    }

    
}
