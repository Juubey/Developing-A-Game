using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactFlash : MonoBehaviour
{
    public void Flash(SpriteRenderer spriteRend, float duration, Color flashColor) {
        StartCoroutine(DoFlash(spriteRend, duration, flashColor));
    }

    private IEnumerator DoFlash(SpriteRenderer spriteRend, float duration, Color flashColor) {
        Color originalColor = spriteRend.color;
        spriteRend.color = flashColor;
        yield return new WaitForSeconds(duration);
        //Check to ensure the sprite's game object hasn't been destroyed
        if(spriteRend != null) {
            spriteRend.color = originalColor;
        }
    }
}
