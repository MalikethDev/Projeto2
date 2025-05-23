using System;
using System.Collections.Generic;

namespace CraftingSim.Model
{
    /// <summary>
    /// Represents a Recipe with required materials and quantities to 
    /// craft an item.
    /// Recipes are ordered alphabetically by name. 
    /// </summary>
    public class Recipe : IRecipe
    {
        private readonly string name;
        private readonly double successRate;
        private readonly Dictionary<IMaterial, int> requiredMaterials;

        public Recipe(string name, double successRate, Dictionary<IMaterial, int> requiredMaterials)
        {
            this.name = name;
            this.successRate = successRate;
            this.requiredMaterials = new Dictionary<IMaterial, int>(requiredMaterials);
        }

        public string Name => name;
        public double SuccessRate => successRate;
        public IReadOnlyDictionary<IMaterial, int> RequiredMaterials => requiredMaterials;

        public int CompareTo(IRecipe other)
        {
            if (other == null) return 1;
            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }
    }
}