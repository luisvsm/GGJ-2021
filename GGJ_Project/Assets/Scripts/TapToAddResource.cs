using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToAddResource : MonoBehaviour
{
    public int amountToAdd;
    public GameDataMonoSingleton.RESOURCE_TYPE resourceType;
    // Start is called before the first frame update

    void OnMouseDown()
    {
        if (resourceType == GameDataMonoSingleton.RESOURCE_TYPE.water)
        {
            PlayerInventoryMonoSingleton.Instance.CollectWater(amountToAdd);
            AudioController.Play("SFX_WaterCan_GetWater");
        }
        else if (resourceType == GameDataMonoSingleton.RESOURCE_TYPE.poo)
        {
            PlayerInventoryMonoSingleton.Instance.CollectPoo(amountToAdd);
            AudioController.Play("SFX_Poo_GetPoo");
        }
    }
}
