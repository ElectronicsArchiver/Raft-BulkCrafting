using HarmonyLib;
using System.Reflection;
using UnityEngine;

public class BulkCrafting : Mod
{
    public static string ModNamePrefix = "<color=#d16e17>[Bulk Crafting]</color>";

    private const string harmonyId = "com.thmsn.bulk-crafting";
    
    Harmony harmony;

    public void Start()
    {
        harmony = new Harmony(harmonyId);
        harmony.PatchAll(Assembly.GetExecutingAssembly());

        Debug.Log(ModNamePrefix + " has been loaded!");
        // could we patch this on load? even if we load in the menu?
        AddCraftModifierKeysToSelectedRecipeBox();
    }

    public void OnModUnload()
    {
        Debug.Log(ModNamePrefix+ " has been unloaded!");
        // TODO: reset selected recipe
    }

    public override void WorldEvent_WorldLoaded()
    {
        AddCraftModifierKeysToSelectedRecipeBox();

        base.WorldEvent_WorldLoaded();
    }

    private static void AddCraftModifierKeysToSelectedRecipeBox()
    {
        var recipeBox = ComponentManager<CraftingMenu>.Value.selectedRecipeBox;
       
        var craftModifierKeys = recipeBox.GetComponent<CraftModifierKeys>() ?? recipeBox.gameObject.AddComponent<CraftModifierKeys>();
        craftModifierKeys.SelectedRecipeBox = recipeBox;
    }
}
