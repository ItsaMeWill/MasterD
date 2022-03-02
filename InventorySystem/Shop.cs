using System;
using System.Collections.Generic;

namespace InventorySystem
{

    // Class that will handle all shopping operations
    class Shop : ListUtils, IPanel
    {

        // Shop characteristics
        private List<Items> itemList;
        private int moneyAvailable;
        private string shopType;

        // Shop constructor, with polimorphism example
        public Shop(int money, string shopType)
        {
            this.moneyAvailable = money;
            this.shopType = shopType;
            this.itemList = new List<Items>();

            if (shopType == "Equipment")
            {
                itemList.Add(new Equipment(0, 0, 1, EquipType.HELMET, "Shadow Beanie", "Make you look sus. Dexterity + 1", 1, 1800, 1800, 1, Rarity.LEGENDARY));
                itemList.Add(new Equipment(0, 0, 1, EquipType.ARMOR, "Hoodie", "Stylish and helps protect your identity. Dexterity + 1", 2, 1200, 1200, 1, Rarity.MAGIC));
                itemList.Add(new Equipment(0, 0, 1, EquipType.PANTS, "Sneaky Pants", "For those who likes to move silencly. Dexterity + 1", 3, 1500, 1500, 1, Rarity.MAGIC));
                itemList.Add(new Equipment(0, 0, 1, EquipType.WEAPON, "Swiss Knife", "A thousand utilities. Dexterity + 1", 4, 2000, 2000, 1, Rarity.RARE));
                itemList.Add(new Equipment(0, 1, 0, EquipType.HELMET, "Horny Helm", "Makes your headbuts do extra damage. Strength + 1", 5, 1200, 1200, 1, Rarity.COMMON));
                itemList.Add(new Equipment(0, 1, 0, EquipType.ARMOR, "Dragon? Plate Mail", "Legendary armor made from dragon scales. Or it is from geckos? Strength + 1", 6, 2000, 2000, 1, Rarity.LEGENDARY));
                itemList.Add(new Equipment(0, 1, 0, EquipType.PANTS, "Sturdy Socks", "Made from heavy and resistant material. Just hope the enemy aims at your feet. Strength + 1", 7, 1500, 1500, 1, Rarity.COMMON));
                itemList.Add(new Equipment(0, 1, 0, EquipType.WEAPON, "Big Hamma", "Its big. Its heavy. It will hurt. Strength + 1", 8, 1800, 1800, 1, Rarity.MAGIC));
                itemList.Add(new Equipment(1, 0, 0, EquipType.HELMET, "Pointy Hat", "The unique, legendary, trademark of the wizards. Magic + 1", 9, 2000, 2000, 1, Rarity.LEGENDARY));
                itemList.Add(new Equipment(1, 0, 0, EquipType.ARMOR, "Confy Long Sleeve Coat", "Its long, its warm and confy. Perfection. Magic + 1", 10, 1800, 1800, 1, Rarity.MAGIC));
                itemList.Add(new Equipment(1, 0, 0, EquipType.PANTS, "Astral Pants", "A magic imbued, polka-dot boxer underwear. Magic + 1", 11, 1500, 1500, 1, Rarity.RARE));
                itemList.Add(new Equipment(1, 0, 0, EquipType.WEAPON, "Oak Staff", "Magic staff that increases the user concentration. Or an elderly people tool for balance. Magic + 1", 12, 1200, 1200, 1, Rarity.MAGIC));
                OrganizeList(itemList, true);

            }

            else if (shopType == "Consumable")
            {
                itemList.Add(new Items("Life Potion", "A life recovering potion. HP + 30", 1, 150, 150, 10, Rarity.COMMON));
                itemList.Add(new Items("Mana Potion", "A wizard must have. MP + 50", 2, 300, 300, 4, Rarity.COMMON));
                itemList.Add(new Items("Provisions", "A necessity for long travels. Allows resting in the wilderness", 3, 500, 500, 2, Rarity.COMMON));
                itemList.Add(new Items("Exploding Potion", "Try not to drink this. Causes fire damage", 4, 250, 250, 10, Rarity.COMMON));
                OrganizeList(itemList, true);

            }

        }

        // Print all items info into the console, including the table like design
        public void Draw()
        {
            Console.Clear();

            Console.WriteLine($"Welcome to the {shopType} shop!");

            if(itemList.Count == 0)
            {
                Console.WriteLine("All sold out...");
            } else
            {
                // Declare and initialize the table with its headers
                var itemTable = new ConsoleTable("", "Name", "Price", "Quantity", "Rarity", "Description");
                foreach (Items currItem in itemList)
                {
                    // Add a row with the respective item info
                    itemTable.AddRow(currItem.GetPosition(), currItem.GetName(), "$ " + currItem.GetPrice(), currItem.GetQuantity(), currItem.GetRarity(), currItem.GetDescription());
                }

                // Print the entire table
                itemTable.Write(Format.Alternative);
            }
        }

        // Check if the shop can buy the selected item and decreases its money. Also, increase the item value by 10% (back to its original price)
        // Add the item to the shop list or increment its quantity if the item already exists
        public bool HandleInput_Buy(Items selectedItem)
        {

            if (CanBuyItem(selectedItem))
            {
                // Check if the shop can buy the item being selled
                if (moneyAvailable >= selectedItem.GetPrice())
                {
                    // Reduce the shop money
                    moneyAvailable -= selectedItem.GetPrice();

                    // Check for duplicated items
                    int index = CheckDuplicateItem(selectedItem, this.itemList);

                    // -1 means item not found
                    if (index == -1)
                    {
                        // Check if the selected item is an equipment or the base class, Items
                        Equipment isEquipment = selectedItem as Equipment;

                        // Not null means it is an equipment, also chech is it is an equipment shop
                        // to create a new equipment to add to the shop item list
                        if (isEquipment != null)
                        {
                            Equipment itemBought = new Equipment(
                            isEquipment.GetMagic(),
                            isEquipment.GetStrength(),
                            isEquipment.GetDexterity(),
                            isEquipment.GetEquipType(),
                            isEquipment.GetName(),
                            isEquipment.GetDescription(),
                            isEquipment.GetFixedPosition(),
                            isEquipment.GetOriginalPrice(),
                            isEquipment.GetOriginalPrice(),
                            1,
                            isEquipment.GetRarity()
                            );
                            itemList.Add(itemBought);
                        }
                        // Its not an equipment but its the consumable shop, so, buy the item
                        else
                        {
                            Items itemBought = new Items(
                                selectedItem.GetName(),
                                selectedItem.GetDescription(),
                                selectedItem.GetFixedPosition(),
                                selectedItem.GetOriginalPrice(),
                                selectedItem.GetOriginalPrice(),
                                1,
                                selectedItem.GetRarity());
                            itemList.Add(itemBought);
                        }
                        // Item added, organize the list and reflect the changes on the shop item list
                        itemList = OrganizeList(itemList, true);
                    }
                    else
                    {
                        // Item already exists in the shop, so increment its quantity
                        itemList[index].IncrementQuantity();
                    }
                }
                // Purchase successful
                return true;
            }
            else
            {
                // Purchase failed
                return false;
            }
        }

        // Sell the selected item to the hero
        // Also, decrease the item quantity if its more than 1 or remove the entry from the shop list otherwise
        public bool HandleInput_Sell(Items itemToSell)
        {
            // Is there more than a single item?
            if (itemList.Find(item => item == itemToSell).GetQuantity() > 1)
            {
                // Sell successful, decrease item quantity and increase the shop money
                itemList.Find(item => item == itemToSell).DecrementQuantity();
                moneyAvailable += itemToSell.GetPrice();
                return true;
            }

            // Is there just a single item?
            else if (itemList.Find(item => item == itemToSell).GetQuantity() == 1)
            {
                // Sell successful, remove the item from the shop list
                itemList.Remove(itemToSell);

                // Item removed, organize the list and reflect the changes on the shop item list
                itemList = OrganizeList(itemList, false);

                // Increase the shop money
                moneyAvailable += itemToSell.GetPrice();
                return true;
            }

            // Sell failed
            return false;
        }

        // Function to check if this shop can buy the selected item
        // Equipment shop can only buy equipments
        // Consumable shop can only buy consumables
        public bool CanBuyItem(Items itemToCheck)
        {
            bool canBuy;

            Equipment isEquipment = itemToCheck as Equipment;

            if (isEquipment != null)
            {
                if(shopType == "Equipment")
                {
                    canBuy = true;
                } else
                {
                    canBuy = false;
                }
            } else
            {
                canBuy = shopType == "Consumable";
            }

            return canBuy;
        }

        // Getters
        // Get the shop type
        public string GetShopType()
        {
            return this.shopType;
        }

        // Get item list count
        public int GetListCount()
        {
            return this.itemList.Count;
        }

        // Get an specific item based on its position property
        public Items GetItem(int position)
        {
            return this.itemList.Find(item => item.GetPosition() == position);
        }

        // Get the shop item list
        public List<Items> GetItemList()
        {
            return this.itemList;
        }

        // Get the shop available money
        public int GetMoneyAvailable()
        {
            return this.moneyAvailable;
        }

    }
}
