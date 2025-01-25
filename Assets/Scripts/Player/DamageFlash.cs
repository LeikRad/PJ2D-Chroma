using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private Color flashColor = Color.white;
    private float flashTime = 0.25f;

    private SpriteRenderer spriteRenderer;
    private Material material;

    private Coroutine damageFlashCourotine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;  // Assign the material of the sprite renderer
    }

    public void CallDamageFlash()
    {
        if (gameObject.activeInHierarchy) 
        {
            damageFlashCourotine = StartCoroutine(DamageFlasher());
        }
    }

    private IEnumerator DamageFlasher()
    {
        SetFlashColor(); // set the color

        float currentFlashAmmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmmount = Mathf.Lerp(1f, 0f, (elapsedTime / flashTime));
            
            SetFlashAmmount(currentFlashAmmount);

            yield return null;
        }
        
    }

    private void SetFlashColor()
    {
            material.SetColor("_FlashColor", flashColor);
    }

    private void SetFlashAmmount(float amount)
    {
            material.SetFloat("_FlashAmount", amount);
    }
}
