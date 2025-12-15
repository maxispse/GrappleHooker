using UnityEngine;
using TMPro;

public class CoinUITextUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private void Start()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.SetCoinText(coinText);
        }
    }
}
