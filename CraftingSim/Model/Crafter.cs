using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CraftingSim.Model
{
    /// <summary>
    /// Implementation of ICrafter. 
    /// </summary>
    public class Crafter : ICrafter
    {
        private readonly Inventory inventory;
        private readonly List<IRecipe> recipeList;

        public Crafter(Inventory inventory)
        {
            this.inventory = inventory;
            recipeList = new List<IRecipe>();
        }

        /// <summary>
        /// returns a read only list of loaded recipes.
        /// </summary>
        public IEnumerable<IRecipe> RecipeList => recipeList;

        /// <summary>
        /// Loads recipes from the files.
        /// Must parse the name, success rate, required materials and
        /// necessary quantities.
        /// </summary>
        /// <param name="recipeFiles">Array of file paths</param>
        public void LoadRecipesFromFile(string[] recipeFiles)
        {
            foreach (string file in recipeFiles)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        string name = reader.ReadLine();
                        double successRate = double.Parse(reader.ReadLine());
                        Dictionary<IMaterial, int> requiredMaterials = new Dictionary<IMaterial, int>();

                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');
                            IMaterial material = new Material(parts[0]);
                            int quantity = int.Parse(parts[1]);
                            requiredMaterials.Add(material, quantity);
                        }

                        IRecipe recipe = new Recipe(name, successRate, requiredMaterials);
                        recipeList.Add(recipe);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading recipe from file: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Attempts to craft an item from a given recipe. Consumes inventory 
        /// materials and returns the result message.
        /// </summary>
        /// <param name="recipeName">Name of the recipe to craft</param>
        /// <returns>A message indicating success, failure, or error</returns>
        public string CraftItem(string recipeName)
        {
            IRecipe selected = null;

            for (int i = 0; i < recipeList.Count; i++)
            {
                if (recipeList[i].Name.Equals(recipeName,
                        StringComparison.OrdinalIgnoreCase))
                {
                    selected = recipeList[i];
                    break;
                }
            }

            if (selected == null)
                return "Recipe not found.";

            foreach (KeyValuePair<IMaterial, int> required in selected.RequiredMaterials)
            {
                IMaterial material = required.Key;
                int need = required.Value;
                int have = inventory.GetQuantity(material);

                if (have < need)
                {
                    if (have == 0)
                    {
                        return "Missing material: " + material.Name;
                    }
                    return "Not enough " + material.Name +
                           " (need " + need +
                           ", have " + have + ")";
                }
            }

            foreach (KeyValuePair<IMaterial, int> required in selected.RequiredMaterials)
                if (!inventory.RemoveMaterial(required.Key, required.Value))
                    return "Not enough materials";

            Random rng = new Random();
            if (rng.NextDouble() < selected.SuccessRate)
                return "Crafting '" + selected.Name + "' succeeded!";
            else
                return "Crafting '" + selected.Name + "' failed. Materials lost.";
        }
    }
}