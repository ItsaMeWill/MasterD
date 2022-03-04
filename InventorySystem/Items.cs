
namespace InventorySystem
{
    // Item structure
    public class Items
    {
        // Item characteristics
        private string name, description;
        private int fixedPosition, position, originalPrice, price, quantityAvailable;
        private Rarity itemRarity;
        private ItemType itemType;

        // Item constructor
        public Items(string name, string description, int fixedPosition, int originalPrice, int price, int quantity, Rarity rarity, ItemType itemType)
        {
            this.name = name;
            this.description = description;
            
            // Initializing position always as 0, the list where this item will be is responsible for setting their positions
            this.position = 0;
            this.fixedPosition = fixedPosition;
            this.originalPrice = originalPrice;
            this.price = price;
            this.quantityAvailable = quantity;
            this.itemRarity = rarity;
            this.itemType = itemType;
        }

        // Getters
        public string GetName()
        {
            return name;
        }

        public string GetDescription()
        {
            return description;
        }

        public int GetOriginalPrice()
        {
            return originalPrice;
        }

        public int GetPrice()
        {
            return price;
        }

        public int GetQuantity()
        {
            return quantityAvailable;
        }

        public Rarity GetRarity()
        {
            return itemRarity;
        }

        public ItemType GetItemType()
        {
            return this.itemType;
        }

        public int GetPosition()
        {
            return position;
        }

        public int GetFixedPosition()
        {
            return fixedPosition;
        }

        // Setters
        public void SetPosition(int position)
        {
            this.position = position;
        }

        public void SetFixedPosition(int fixedPosition)
        {
            this.fixedPosition = fixedPosition;
        }

        public void SetQuantity(int quantity)
        {
            this.quantityAvailable = quantity;
        }

        public void SetPrice(int price)
        {
            this.price = price;
        }

        public void IncrementQuantity()
        {
            this.quantityAvailable += 1;
        }

        public void DecrementQuantity()
        {
            this.quantityAvailable -= 1;
        }

    }

    public enum Rarity
    {
        COMMON,
        MAGIC,
        RARE,
        LEGENDARY
    }

    public enum ItemType
    {
        CONSUMABLE,
        HELMET,
        ARMOR,
        PANTS,
        WEAPON
    }

}
