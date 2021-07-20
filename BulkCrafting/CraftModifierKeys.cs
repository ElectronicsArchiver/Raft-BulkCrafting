using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftModifierKeys : MonoBehaviour
{


    public SelectedRecipeBox SelectedRecipeBox { get; set; }
    private Text craftButtonText;
    private string originalCraftButtonText;
    private int multiplier = 10;
    private Dictionary<string, int> originalNewCost = new Dictionary<string, int>();
    private Item_Base originalItem;
    private bool leftShiftModifier = false;

    public static int amountToCraft = 1;

    private void Start()
    {
        Debug.Log("CraftModifierKeys started");
        craftButtonText = SelectedRecipeBox.craftButton.GetComponentInChildren<Text>();
        originalCraftButtonText = craftButtonText.text;
    }

    private void Update()
    {
        if (SelectedRecipeBox.ItemToCraft == null)
        {
            // TODO: Reset things.
            return;
        }

        // We got a new selected recipe, or the first one.
        if (originalItem != SelectedRecipeBox.ItemToCraft)
        {
            Debug.Log(BulkCrafting.ModNamePrefix + " A new recipe was selected, restoring original cost.");

            if (originalItem != null)
            {
                RestoreOriginalCostMultiple();
            }

            if (SelectedRecipeBox.ItemToCraft != null)
            {
                originalItem = SelectedRecipeBox.ItemToCraft;
                CacheOriginalCostMultiple();
            }
        }

        // TODO: we need to clone the original cost to be able to restore the original costmultiple.
        if (Input.GetKeyDown(KeyCode.LeftShift) && !leftShiftModifier)
        {
            leftShiftModifier = true;

            //Debug.Log("Left shift pressed");
            craftButtonText.text = originalCraftButtonText + " x " + multiplier;

            // TODO: this causes 10 hammers to be added to the slot, this is not what we want, it should add a hammer 10 times. depending on stack size
            //Traverse.Create(SelectedRecipeBox.ItemToCraft.settings_recipe).Field("amountToCraft").SetValue(SelectedRecipeBox.ItemToCraft.settings_recipe.AmountToCraft * multiplier);

            if (SelectedRecipeBox.ItemToCraft != null)
            {
                ModifyCostMultiple(SelectedRecipeBox.ItemToCraft.settings_recipe.NewCost, multiplier);
                amountToCraft = SelectedRecipeBox.ItemToCraft.settings_recipe.AmountToCraft * multiplier;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            leftShiftModifier = false;

            craftButtonText.text = originalCraftButtonText;
            //Traverse.Create(SelectedRecipeBox.ItemToCraft.settings_recipe).Field("amountToCraft").SetValue(SelectedRecipeBox.ItemToCraft.settings_recipe.AmountToCraft / multiplier);

            amountToCraft = SelectedRecipeBox.ItemToCraft.settings_recipe.AmountToCraft;

            RestoreOriginalCostMultiple();
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            //Debug.Log("Left alt pressed");
        }
    }

    private void ModifyCostMultiple(CostMultiple[] newCost, int multiplier)
    {
        foreach (var costMultiple in newCost)
        {
            costMultiple.amount *= multiplier;
        }
    }

    private void RestoreOriginalCostMultiple()
    {
        foreach (var costMultiple in SelectedRecipeBox.ItemToCraft.settings_recipe.NewCost)
        {
            foreach (var item in costMultiple.items)
            {
                if (originalNewCost.TryGetValue(item.UniqueName, out var amount))
                {
                    costMultiple.amount = amount;
                }
            }
        }
    }

    private void CacheOriginalCostMultiple()
    {
        originalNewCost.Clear();

        foreach (var costMultiple in SelectedRecipeBox.ItemToCraft.settings_recipe.NewCost)
        {
            foreach (var item in costMultiple.items)
            {
                originalNewCost.Add(item.UniqueName, costMultiple.amount);
            }
        }
    }
}