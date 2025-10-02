using UnityEngine;

public class PlayerItemInHand : MonoBehaviour
{
    [SerializeField] private SpriteRenderer itemSprite;

    private void Awake()
    {
        itemSprite.sprite = null;
    }

    public void ChangeItemSprite(Sprite newSprite)
    {
        itemSprite.sprite = newSprite;
    }
}
