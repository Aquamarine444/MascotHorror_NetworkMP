using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    [SerializeField] public bool hasDial;
    [SerializeField] public bool hasFuse;

    [SerializeField] private GameObject dialSprite;
    [SerializeField] private GameObject fuseSprite;


    public void UpdateInventory()
    {
        if (hasDial)
        {
            dialSprite.SetActive(true);
        }

        if (hasFuse)
        {
            fuseSprite.SetActive(true);
        }
    }
    
}
