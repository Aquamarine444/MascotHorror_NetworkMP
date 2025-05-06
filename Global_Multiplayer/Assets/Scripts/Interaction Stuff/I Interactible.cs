using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    // Rotate and look at
    bool Inspect { get; }

    // View from a distance
    bool Examine { get; }

    // Toggle buttons or open doors
    bool Trigger { get; }

    // Placing items
    bool Place { get; }

    GameObject ExamineCam { get; }

    public bool Interact(Interactors interact);
}
