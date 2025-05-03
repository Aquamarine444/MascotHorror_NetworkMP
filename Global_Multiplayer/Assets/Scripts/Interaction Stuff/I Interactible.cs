using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{

    bool Inspect { get; }
    bool Examine { get; }
    bool Trigger { get; }


    GameObject ExamineCam { get; }

    public bool Interact(Interactors interact);
}
