﻿using System.Collections.Generic;
using System.Linq;
using SS3D.Systems.Storage.Containers;
using SS3D.Systems.Storage.Interfaces;
using SS3D.Systems.Storage.Items;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace SS3D.Systems.Storage.UI
{
    /// <summary>
    /// SingleItemContainerSlot allows displaying the content of a container that contain a single item in the UI.
    /// It handles updating the inventory when an item is dropped on it, and it changes the displayed sprite accordingly.
    /// As of now, it's only use is displaying the content of the containers on the hands of the player in the UI slots.
    /// </summary>
    public class SingleItemContainerSlot : InventoryDisplayElement, IPointerClickHandler, ISlotProvider
    {
        public ItemDisplay ItemDisplay;

        /// <summary>
        /// The container displayed by this slot.
        /// </summary>
        private AttachedContainer _container;

        public AttachedContainer Container
        {
            get => _container;
            set => UpdateContainer(value);
        }

        public void Start()
        {
            Assert.IsNotNull(ItemDisplay);
            if (Container != null)
            {
                UpdateContainer(Container);
            }
        }
        
        /// <summary>
        /// When dragging and dropping an item sprite over this slot, update the inventory
        /// and the displayed sprite inside the slot.
        /// Does nothing if the slot already has an item.
        /// </summary>
        public override void OnItemDisplayDrop(ItemDisplay display)
        {
            if (!_container.Container.Empty)
            {
                return;
            }

            display.ShouldDrop = true;
            ItemDisplay.Item = display.Item;
            Inventory.ClientTransferItem(ItemDisplay.Item, Vector2Int.zero, Container);
        }

        /// <summary>
        /// Change the displayed sprite inside the slot.
        /// </summary>
        private void UpdateDisplay()
        {
            ItemDisplay.Item = _container.Container.Items.FirstOrDefault();
        }

        /// <summary>
        /// UpdateContainer modify the container that this slot display, replacing the old one with newContainer.
        /// </summary>
        private void UpdateContainer(AttachedContainer newContainer)
        {
            if (_container == newContainer)
            {
                return;
            }
            
            if (_container != null)
            {
                _container.Container.OnContentsChanged -= ContainerContentsChanged;
            }
            
            newContainer.Container.OnContentsChanged += ContainerContentsChanged;
            _container = newContainer;
        }

        private void ContainerContentsChanged(Container _, IEnumerable<Item> items, IEnumerable<Item> newItems, ContainerChangeType type)
        {
            if (type != ContainerChangeType.Move)
            {
                UpdateDisplay();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Inventory.ClientInteractWithSingleSlot(_container);
            Inventory.ActivateHand(_container);
        }
		
		public GameObject GetCurrentGameObjectInSlot()
		{
			if (ItemDisplay.Item == null)
			{
				return null;
			}
			else
			{
				return ItemDisplay.Item.gameObject;
			}
		}
		
    }
}