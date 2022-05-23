using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTracker : MonoBehaviour
{
    public static CharacterTracker instace;

    public int currentHealth, maxHealth, currentCoins;


    private void Awake()
    {
        instace = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
