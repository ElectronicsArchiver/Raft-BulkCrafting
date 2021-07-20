using UnityEngine;

public class BulkCrafting : Mod
{
    public static string ModNamePrefix = "<color=#d16e17>[Bulk Crafting]</color>";

    private const string harmonyId = "com.thmsn.bulk-crafting";

    public void Start()
    {
        Debug.Log(ModNamePrefix + " has been loaded!");
    }

    public void OnModUnload()
    {
        Debug.Log(ModNamePrefix+ " has been unloaded!");
    }

    public override void WorldEvent_WorldLoaded()
    {
        var recipeBox = ComponentManager<CraftingMenu>.Value.selectedRecipeBox;
        var craftModifierKeys = recipeBox.gameObject.AddComponent<CraftModifierKeys>();
        craftModifierKeys.SelectedRecipeBox = recipeBox;

        base.WorldEvent_WorldLoaded();
    }
}
