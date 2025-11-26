using TMPro;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [Header("Settings")]
    public int currentCoins = 0;
    public TMP_Text coinText;
    GameObject coin;

    void Start()
    {
        
    }
    public void CoinPickup(int coin = 1)
    {
        currentCoins++;
        currentCoins = Mathf.Min(currentCoins, 999);
    }
    public void CoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "" + currentCoins;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinPickup(1);
            CoinUI();
            Destroy(coin);
        }
    }
}
