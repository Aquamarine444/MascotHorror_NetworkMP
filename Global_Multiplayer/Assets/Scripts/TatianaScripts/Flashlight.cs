using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject Light;

    public int Counter;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Counter += 1;

            if (Counter%2 == 1)
            {
                Light.SetActive(true);
            }

            if (Counter%2 == 2)
            {
                Light.SetActive(false);
            }
        }
    }
}
