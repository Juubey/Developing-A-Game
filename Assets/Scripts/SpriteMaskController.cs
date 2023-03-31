using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SoulSpliter
{
    [RequireComponent(typeof(Collider2D))]
    public class SpriteMaskController : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer playerSpriteRenderer;

        [SerializeField]
        private SpriteMask spriteMask;

        private Collider2D spriteMaskCollider;

        //List of objects that we are colliding with
        private List<SpriteRenderer> otherRenderers = new List<SpriteRenderer>();

        public bool checking = false;

        private void Awake()
        {
            spriteMaskCollider = GetComponent<Collider2D>();
            spriteMaskCollider.isTrigger = true;
        }

        private void Update()
        {
            if (checking)
            {
                foreach (SpriteRenderer renderer in otherRenderers)
                    //check if object is on the same layer and infront of the player sprite
                    if(
                        playerSpriteRenderer.sortingLayerName == renderer.sortingLayerName && playerSpriteRenderer.sortingOrder <= renderer.sortingOrder
                        //check the y sorting order
                        && playerSpriteRenderer.transform.position.y > renderer.transform.position.y)
                    {
                        //if true enable the sprite mask
                        spriteMask.enabled = true;
                        playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                        return;
                    }
                    else
                    {
                        //else disable the spritemask
                        spriteMask.enabled = false;
                        playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
                    }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.isTrigger == false)
                return;
            SpriteRenderer spriteRenderer = collider.GetComponent<SpriteRenderer>();
            if(spriteRenderer != null)
            {
                otherRenderers.Add(spriteRenderer);
                checking = true;
            }
        }


        private void OnTriggerExit(Collider2D collider)
        {
            if (collider.isTrigger == false)
                return;
            SpriteRenderer spriteRenderer = collider.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                otherRenderers.Remove(spriteRenderer);
                if(otherRenderers.Count <= 0)
                {
                    checking = false;
                    spriteMask.enabled = false;
                    playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
                }
            }
        }
    }
}
