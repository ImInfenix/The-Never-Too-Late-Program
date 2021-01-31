using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltExit : ConveyorBeltPart
{
    [SerializeField]
    private bool isFinisher;

    public void SetIsFinisher(bool isFinisher)
    {
        this.isFinisher = isFinisher;
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.CompareTag(CarryableObject.commonTag))
            if (isFinisher)
                LevelManager.Instance.ConfirmEnd(other.GetComponent<CarryableObject>());
            else
                LevelManager.Instance.ResendToPool(other.GetComponent<CarryableObject>());
    }
}
