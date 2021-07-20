using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// Perhaps we can use the transpiler to rewrite CraftItem, to loop amountToCraft vs stacksize, alternatively we just need to add a postfix that does that -1

/// <summary>
/// CraftItem is patched so when we craft with a multiplier, we spawn the additional items.
/// </summary>
[HarmonyPatch(typeof(CraftingMenu), "CraftItem")]
class CraftItem_ModifierKeys
{
    static void Postfix(CraftingMenu __instance)
    {
        var itemToCraft = __instance.selectedRecipeBox.ItemToCraft;
        var localPlayer = ComponentManager<Network_Player>.Value;

        var amountToCraft = CraftModifierKeys.amountToCraft - itemToCraft.settings_recipe.AmountToCraft; // the original CraftItem have already crafted one
        var stacksToCraft = Mathf.Floor(amountToCraft / itemToCraft.settings_Inventory.StackSize);
        for (int i = 0; i < stacksToCraft; i++)
        {
            localPlayer.Inventory.AddItem(itemToCraft.UniqueName, itemToCraft.settings_Inventory.StackSize);
        }

        var rest = amountToCraft % itemToCraft.settings_Inventory.StackSize;
        if (rest > 0)
        {
            localPlayer.Inventory.AddItem(itemToCraft.UniqueName, rest);
        }
    }
}