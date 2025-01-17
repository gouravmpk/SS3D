﻿using SS3D.Data.Enums;
using SS3D.Systems.Inventory.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SS3D.Systems.Crafting
{
    /// <summary>
    /// Crafting recipes allow to replace a bunch of item by another, using a specific interaction.
    /// </summary>
    [CreateAssetMenu(fileName = "Recipe", menuName = "SS3D/Crafting/Recipe")]
    public class CraftingRecipe : ScriptableObject, ISerializationCallbackReceiver
    {

        private Dictionary<ItemId, int> _elements = new();

        [SerializeField]
        private float _executionTime;

        [SerializeField]
        private string _interactionName;

        [SerializeField]
        private ItemId _target;

        [SerializeField]
        private List<ItemId> _result;


        /// <summary>
        /// The items and their respective numbers necessary for the recipe.
        /// </summary>
        public Dictionary<ItemId, int> Elements => _elements;

        /// <summary>
        /// Time it takes in second for the crafting to finish.
        /// </summary>
        public float ExecutionTime => _executionTime;

        /// <summary>
        /// Name of the necessary interaction to perform the recipe.
        /// </summary>
        public string InteractionName => _interactionName;

        /// <summary>
        /// The target of the recipe, which is the item on which the player must click to get the crafting interactions.
        /// </summary>
        public ItemId Target => _target;

        /// <summary>
        /// The result of the crafting.
        /// </summary>
        public List<ItemId> Result => _result;

        public int ElementsNumber => _elements.Sum(x => x.Value);

#if UNITY_EDITOR
        /// <summary>
        /// Necessary to be able to edit in editor recipe elements.
        /// Not straightforward since they are in a Hashset.
        /// </summary>
        [SerializeField]
        private List<RecipeElement> _recipeElements;
#endif

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            // just transfer things from the list to the dictionnary.

            int enumCount = Enum.GetNames(typeof(ItemId)).Length;
            _elements = new Dictionary<ItemId, int>();
            foreach (RecipeElement item in _recipeElements)
            {
                int i = 0;
                
                while (!_elements.TryAdd((ItemId) (((int)item.ItemId + i) % enumCount), item.Count))
                {
                    i++;
                }
            }
            _recipeElements = null;
#endif
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            // just transfer things from the dictionnary to the list.
            if (_elements == null) return;
            _recipeElements = new List<RecipeElement>();
            foreach (ItemId id in _elements.Keys)
            {
                _recipeElements.Add(new RecipeElement(id, _elements[id]));
            }
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// A recipe element is simply describing an item and a number of it.
        /// </summary>
        [System.Serializable]
        private struct RecipeElement
        {
            /// <summary>
            /// Number of items.
            /// </summary>
            [SerializeField]
            private int _count;

            /// <summary>
            /// Id of the item.
            /// </summary>
            [SerializeField]
            private ItemId _itemId;

            public int Count => _count;

            public ItemId ItemId => _itemId;

            public RecipeElement(ItemId id, int count)
            {
                _count = count;
                _itemId = id;
            }
        }
#endif
    }

}

