using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Player Reference")]
    public PlayerHealth playerHealth;

    [Header("Face UI Sprites")]
    public Sprite neutralFace;
    public Sprite damagedFace;
    public Sprite seriouslyDamagedFace;
    public Sprite deadFace;
    public Sprite hitFace;
    public float hitDuration = 0.2f;
    public Image faceImage;
    

    
    void Start()
    {
        UpdateFace();
    }

    void Update()
    {
        UpdateFace();
    }

    void UpdateFace()
    {
        if (faceImage == null || playerHealth == null) return;

        switch (playerHealth.currentLives)
        {
            case 4:
                faceImage.sprite = neutralFace;
                break;
            case 3:
                faceImage.sprite = neutralFace;
                break;
            case 2:
                faceImage.sprite = damagedFace;
                break;
            case 1:
                faceImage.sprite = seriouslyDamagedFace;
                break;
            case 0:
                faceImage.sprite = deadFace;
                break;
        }
    }
    public void FlashHit()
    {
        StartCoroutine(HitFlash());
    }

    public IEnumerator HitFlash()
    {
        faceImage.sprite = hitFace;
        yield return new WaitForSeconds(hitDuration);
        UpdateFace(); // return to current lives face
    }
}
