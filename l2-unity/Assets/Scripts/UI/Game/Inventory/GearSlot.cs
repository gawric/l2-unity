using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GearSlot : InventorySlot
{
    public GearSlot(int position, VisualElement slotElement, InventoryGearTab tab) : base(position, slotElement, tab) {
    }

    public GearSlot(int position, AbstractItem item, VisualElement slotElement, InventoryGearTab tab) : base(position, item, slotElement, tab) {
    }
    
    protected override void HandleRightClick() {

    }
}
