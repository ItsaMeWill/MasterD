using System;
using System.Collections.Generic;

namespace InventorySystem
{
    public class Hero : ListUtils, IPanel
    {
        // Hero characteristics
        private int money, magicPoints, strengthPoints, dexterityPoints;
        private List<Items> inventory;

        // Hero constructor
        public Hero(int money)
        {
            this.money = money;
            this.magicPoints = 0;
            this.strengthPoints = 0;
            this.dexterityPoints = 0;
            this.inventory = new List<Items>();
        }

        // Check if the hero can buy the selected item and decreases its money. Also, decrease the item value by 10% (shopkeepers are greedy)
        // Add the item to the inventory or increment its quantity if the item already exists
        public bool HandleInput_Buy(Items selectedItem)
        {
            if(money >= selectedItem.GetPrice())
            {
                // Reduce the hero money
                money -= selectedItem.GetPrice();

                // Check for duplicated items
                int index = CheckDuplicateItem(selectedItem, this.inventory);

                // -1 means item not found
                if(index == -1)
                {
                    float calculatePrice = selectedItem.GetPrice();
                    
                    // Item price is reduced by 10%
                    calculatePrice -= (calculatePrice / 100 * 10);

                    // Check if the selected item is an equipment or the base class, Items
                    Equipment isEquipment = selectedItem as Equipment;
                    
                    // Not null means it is an equipment, so create a new equipment to add to the hero inventory
                    if(isEquipment != null)
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
                        (int)calculatePrice,
                        1,
                        isEquipment.GetRarity()
                        );
                        inventory.Add(itemBought);
                    } 
                    // Being null means its not an equipment, so create a new item to add to the hero inventory
                    else
                    {
                        Items itemBought = new Items(
                        selectedItem.GetName(),
                        selectedItem.GetDescription(),
                        selectedItem.GetFixedPosition(),
                        selectedItem.GetOriginalPrice(),
                        (int)calculatePrice,
                        1,
                        selectedItem.GetRarity()
                        );
                        inventory.Add(itemBought);
                    }

                    // Item added, organize the list if the list is not empty and reflect the changes on the hero inventory
                    inventory = OrganizeList(inventory, false);

                }
                
                else
                {
                    // Item already exists in the inventory, so increment its quantity
                    inventory[index].IncrementQuantity();
                }

                // Purchase successful
                return true;
            } else
            {
                // Purchase failed
                return false;
            }
        }

        // Sell the selected item back to the shop
        // Also, decrease the item quantity if its more than 1 or remove the entry from the inventory list otherwise
        public bool HandleInput_Sell(Items itemToSell)
        {
            // Is there more than a single item?
            if(inventory.Find(item => item == itemToSell).GetQuantity() > 1)
            {
                // Sell successful, decrease item quantity and increase the heros money
                inventory.Find(item => item == itemToSell).DecrementQuantity();
                money += itemToSell.GetPrice();
                return true;
            }

            // Is there just a single item?
            else if(inventory.Find(item => item == itemToSell).GetQuantity() == 1)
            {
                // Sell successful, remove the item from the hero inventory
                inventory.Remove(itemToSell);

                // Item removed, organize the list if the list is not empty and reflect the changes on the hero inventory
                if(inventory.Count != 0)
                {
                    inventory = OrganizeList(inventory, false);
                }

                // Increase the hero money
                money += itemToSell.GetPrice();
                return true;
            }

            // Sell failed
            return false;
        }

        // Print the hero attributes and inventory to the console and give some initial instructions
        public void Draw()
        {
            Console.Clear();

            // Print the attributes
            Console.WriteLine("ATTRIBUTES");
            var attributesTable = new ConsoleTable("Strength", "Dexterity", "Magic");
            attributesTable.AddRow(strengthPoints, dexterityPoints, magicPoints);
            attributesTable.Write(Format.Alternative);

            Console.WriteLine("INVENTORY");

            if (inventory.Count == 0)
            {
                Console.WriteLine("Empty...\n");
            }
            else 
            {
                // Iterate and print the inventory
                var inventoryTable = new ConsoleTable("", "Name", "Value", "Quantity", "Rarity", "Description", "Equipped");
                foreach (Items currItem in inventory)
                {
                    // Changes made here: initially, the class Items had the property that defined if the item is equipped or not
                    // But since only equipments can be equipped / unequipped, it made sense to put this property on the Equipment class
                    // This change reflected the way the Hero inventory is printed, but i think it makes more sense this way
                    // Check to see if the current item in the iteration is an equipment, mainly for the equipped property
                    Equipment isEquipment = currItem as Equipment;
                    if(isEquipment != null)
                    {
                        inventoryTable.AddRow(isEquipment.GetPosition(), isEquipment.GetName(), isEquipment.GetPrice(), isEquipment.GetQuantity(), isEquipment.GetRarity(), isEquipment.GetDescription(), isEquipment.GetEquipped() ? "X" : "");
                    } else
                    {
                        inventoryTable.AddRow(currItem.GetPosition(), currItem.GetName(), currItem.GetPrice(), currItem.GetQuantity(), currItem.GetRarity(), currItem.GetDescription(), "");
                    }
                    
                }
                inventoryTable.Write(Format.Alternative);
            }

            Console.WriteLine($"Money available: {money}");

        }

        // Function to check if the selected item is equipable, if it is already equipped / unequipped and calculate the hero attributes
        public string ManageEquipment(Items itemToEquip)
        {
            // Initialize the variable that will receive the result of this operation
            string message;

            // Check if the selected item is an equipment and therefore, can be equipped
            Equipment isEquipment = itemToEquip as Equipment;

            // Not null means it is an equipment
            if (isEquipment != null)
            {
                CheckEquippedType(isEquipment);

                // Calculate the effects of equipping / unequipping this equipment, has on the hero attributes
                // Assign this calculation to the variable newAttributes (an array that represents Magic, Strength and Dexterity, respectively)
                // The CalculateAttribute function will set this equipment "equipped" property by sending the oposite of the actual equipment
                // equipped property
                int[] newAttributes = isEquipment.CalculateAttribute(magicPoints, strengthPoints, dexterityPoints, !isEquipment.GetEquipped());

                // Hero attributes receives the new value
                magicPoints = newAttributes[0];
                strengthPoints = newAttributes[1];
                dexterityPoints = newAttributes[2];

                // Item was equipped or unequipped
                message = itemToEquip.GetName() + (isEquipment.GetEquipped() ? " equipped" : " unequipped");
            }

            // The item is not an equipment, thus, not equipable
            else
            {
                message = "Thats not an equipable item!";
            }

            return message;
        }

        // This function will unequip any equipment of the same type of the equipment that is sent as parameter
        // By checking the hero inventory, if any equipment of the same type that is also equipped is found,
        // It will be unequipped and the hero attributes will be recalculated through the call of the CalculateAttribute function
        // using the item found
        private void CheckEquippedType(Equipment itemToCheck)
        {
            // Iterate the hero inventory
            foreach (Items currItem in inventory)
            {
                // Try to down cast the current item as equipment
                Equipment isEquipment = currItem as Equipment;
                
                // Not null means it is an equipment
                if(isEquipment != null)
                {   
                    // Check which kind of equipment we are searching by matching their properties equipType
                    if (isEquipment.GetEquipType() == itemToCheck.GetEquipType())
                    {
                        // We check to see if this item of the same type is equipped
                        // And it must be different from the itemToCheck (to avoid entering
                        // this flow, if we are trying to equip / unequip the same item,
                        // n this case, the function CalculateAttributes will run twice
                        // equipping and unequipping the same item)
                        if (isEquipment.GetEquipped() == true && isEquipment != itemToCheck)
                        {
                            // Calculate the effects of equipping / unequipping this equipment, has on the hero attributes
                            // Assign this calculation to the variable newAttributes (an array that represents Magic, Strength and Dexterity, respectively)
                            // The CalculateAttribute function will set this equipment "equipped" property by sending the oposite of the actual equipment
                            // equipped property
                            int[] newAttributes = isEquipment.CalculateAttribute(magicPoints, strengthPoints, dexterityPoints, !isEquipment.GetEquipped());

                            // Hero attributes receives the new value
                            magicPoints = newAttributes[0];
                            strengthPoints = newAttributes[1];
                            dexterityPoints = newAttributes[2];
                        }
                    }

                
                }
            } 
        }

        // Getters
        public int GetMoney()
        {
            return money;
        }

        public int GetMagic()
        {
            return magicPoints;
        }

        public int GetStrength()
        {
            return strengthPoints;
        }

        public int GetDexterity()
        {
            return dexterityPoints;
        }

        // Get inventory list count
        public int GetListCount()
        {
            return this.inventory.Count;
        }

        // Get an specific item based on its position property
        public Items GetItem(int position)
        {
            return this.inventory.Find(item => item.GetPosition() == position);
        }

        // Get the hero inventory
        public List<Items> GetInventory()
        {
            return inventory;
        }

        // Setters
        /* Interface is handling the heros money now
        public void SetMoney(int price)
        {
            this.money = price;
        } */

        public void SetMagic(int magic)
        {
            this.magicPoints = magic;
        }

        public void SetStrength(int strength)
        {
            this.strengthPoints = strength;
        }

        public void SetDexterity(int dexterity)
        {
            this.dexterityPoints = dexterity;
        }
    }
}
