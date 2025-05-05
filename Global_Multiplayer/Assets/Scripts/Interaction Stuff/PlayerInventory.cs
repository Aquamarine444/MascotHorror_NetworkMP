using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    [SerializeField] public bool hasDial;
    [SerializeField] public bool hasFuse;
    [SerializeField] public bool fuseFilled;
    [SerializeField] public bool dialFilled;

    [SerializeField] private GameObject dialSprite;
    [SerializeField] private GameObject fuseSprite;


    public void UpdateInventory()
    {
        dialSprite.SetActive(hasDial);
        fuseSprite.SetActive(hasFuse);
    }

}
