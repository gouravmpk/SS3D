﻿using SS3D.Interactions;
using SS3D.Interactions.Interfaces;
using SS3D.Systems.Combat.Interactions;
using SS3D.Systems.Crafting;
using SS3D.Systems.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS3D.Systems.Crafting
{
    public class Slice : MonoBehaviour, IInteractionSourceExtension
    {
        public void GetSourceInteractions(IInteractionTarget[] targets, List<InteractionEntry> interactions)
        {
            SliceInteraction interaction = new SliceInteraction(2f, GetComponent<InteractionSource>().transform);

            foreach (IInteractionTarget target in targets)
            {
                interactions.Add(new InteractionEntry(target, interaction));
            }
        }
    }
}


