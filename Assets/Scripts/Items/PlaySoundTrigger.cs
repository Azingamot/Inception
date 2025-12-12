using UnityEngine;

public class PlaySoundTrigger : MonoBehaviour
{
    public void PlaySound()
    {
        AudioSystem.PlaySound(InventoryController.Instance.GetSelectedItem().SoundType);
    }
}
