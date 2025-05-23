using System;
using System.Collections.Generic;

namespace CraftingSim.Model
{
    /// <summary>
    /// Represents a Material used to craft an item
    /// Materials have an Id and a Name
    /// Materials are the same if either the Id or the Name are equal
    /// </summary>
    public class Material : IMaterial
    {
        private static int idCounter = 0;
        private readonly int id;
        private readonly string name;

        public Material(string name)
        {
            this.name = name;
            id = idCounter++;
        }

        public int Id => id;
        public string Name => name;

        public bool Equals(IMaterial other)
        {
            if (other == null) return false;
            return Id == other.Id || Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}