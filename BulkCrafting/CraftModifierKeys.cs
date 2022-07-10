using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: there seems o be an issue with the recipe being stuck after you press quick craft making it increment the cost repeatedly.
public class CraftModifierKeys : MonoBehaviour
{


    public SelectedRecipeBox SelectedRecipeBox { get; set; }
    private Text craftButtonText;
    private string originalCraftButtonText;
    private int multiplier = 10;
    private Dictionary<CostMultiple, int> originalNewCost = new Dictionary<CostMultiple, int>();
    private Item_Base originalItem;
    private bool leftShiftModifier = false;

    public static int amountToCraft = 1;

    private void Start()
    {
        Debug.Log(BulkCrafting.ModNamePrefix + "CraftModifierKeys started");
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
            Debug.Log(BulkCrafting.ModNamePrefix + " A new recipe was selected");

            //if (originalItem != null)
            //{
            //    RestoreOriginalCostMultiple();
            //}

            //if (SelectedRecipeBox.ItemToCraft != null)
            //{
            //    originalItem = SelectedRecipeBox.ItemToCraft;
            //    CacheOriginalCostMultiple();
            //}
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
                RestoreOriginalCostMultiple();
                CacheOriginalCostMultiple();
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
        //Debug.Log(BulkCrafting.ModNamePrefix + " modifying cost");
        foreach (var costMultiple in newCost)
        {
            costMultiple.amount *= multiplier;
        }
    }

    private void RestoreOriginalCostMultiple()
    {
        //Debug.Log(BulkCrafting.ModNamePrefix + " restoring original cost.");
        foreach (var pair in originalNewCost)
        {
            if (pair.Key != null)
            {
                pair.Key.amount = pair.Value;
            }
        }

        originalNewCost.Clear();
    }

    private void CacheOriginalCostMultiple()
    {
        foreach (var costMultiple in SelectedRecipeBox.ItemToCraft.settings_recipe.NewCost)
        {
            if (!originalNewCost.ContainsKey(costMultiple))
            {
                originalNewCost.Add(costMultiple, costMultiple.amount);
            }
        }
    }
}