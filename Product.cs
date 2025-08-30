using System;

namespace ProductManagements
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public int StockQuantity { get; set; }
        
        public decimal Price { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Product()
        {
        }

        public Product(string name, string description, int stockQuantity, decimal price)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? string.Empty;
            StockQuantity = stockQuantity;
            Price = price;
            CreatedDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Price:C} ({StockQuantity} db)";
        }

        public override bool Equals(object obj)
        {
            if (obj is Product other)
            {
                return Id == other.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}