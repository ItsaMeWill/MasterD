using System;

namespace InventorySystem
{
    class ConsoleControl
    {

        public Shop[] shopControl;
        public Hero heroControl;
        public string userChoice, resultOperation;
        public bool shopMenu, buyMenu, sellMenu, mainMenu, heroInventory;

        public ConsoleControl(Hero hero, Shop[] shopList)
        {
            this.heroControl = hero;
            this.shopControl = shopList;
        }

        // Function that initializes the program
        public void Init()
        {
            // This bool ensures the loop
            mainMenu = true;
            MainMenu();

        }

        // Begins the program at the main menu
        public void MainMenu()
        {
            // Just entered this menu, so reset the result operation message
            resultOperation = "";

            while(mainMenu == true)
            {
                Console.Clear();
                Console.WriteLine("Welcome hero, choose your destiny!");
                Console.WriteLine(resultOperation);
                Console.WriteLine("1 - Equipment Shop\n2 - Consumable Shop\n3 - Inventory\n0 - Begin adventure");

                userChoice = Console.ReadLine();

                // Validate the user input
                int validatedInput = ValidateChoice(userChoice);

                // -1 means the user typed anything but a number
                if (validatedInput == -1)
                {
                    resultOperation = "You must type a number!";
                }

                // User input is inside the range of possible options
                else if (validatedInput >= 0 && validatedInput <= 3)
                {
                    // Check the user input and go to the respective menu
                    // The bools controls the loop inside each one of them
                    switch (validatedInput)
                    {
                        // 0 means the end of the program
                        case 0:
                            mainMenu = false;
                            Console.Clear();
                            Console.WriteLine("Game still in development...");
                            break;
                        // 1 goes to the Equipment Shop menu
                        case 1:
                            shopMenu = true;
                            ShopMenu(shopControl[0]);
                            break;
                        // 2 goes to the Consumable Shop menu
                        case 2:
                            shopMenu = true;
                            ShopMenu(shopControl[1]);
                            break;
                        // 3 goes to the hero inventory menu
                        case 3:
                            heroInventory = true;
                            HeroInventory(validatedInput);
                            break;
                    }
                }
                // Number typed out of available range
                else
                {
                    resultOperation = "Please, pick a number between 0 and 3";
                }

            }
            



        }

        // Function to handle all possible operations inside the shop menu screen
        public void ShopMenu(Shop shop)
        {
            // Just entered this menu, so reset the result operation message
            resultOperation = "";   

            while (shopMenu == true) {

                Console.Clear();

                // Print to the console the shop list and possible options
                shop.Draw();

                // Print the last operation result (begins empty "" by default)
                Console.WriteLine(resultOperation);

                Console.WriteLine("Choose an option:\n1 - Buy items\n2 - Sell items\n0 - Exit shop");
                userChoice = Console.ReadLine();

                // Validate the user input
                int validatedInput = ValidateChoice(userChoice);

                // -1 means the user typed anything but a number
                if (validatedInput == -1)
                {
                    resultOperation = "You must type a number!";
                }

                // User input is inside the range of possible options
                else if (validatedInput >= 0 && validatedInput <= 2)
                {
                    // Check the user input and go to the respective menu
                    // The bools controls the loop inside each one of them
                    switch (validatedInput)
                    {
                        // 0 means exit the shop
                        case 0:
                            shopMenu = false;

                            // Reset the result operation variable
                            resultOperation = "";
                            break;
                        // 1 means the buy options are shown and handled
                        case 1:
                            buyMenu = true;
                            BuyMenu(shop, validatedInput);
                            break;

                        // 2 means the sell options are shown and handled
                        case 2:
                            sellMenu = true;
                            SellMenu(shop, validatedInput);
                            break;
                    }
                }
                // Number typed out of available range
                else
                {
                    resultOperation = "Please, pick a number between 0 and 2";
                }
            } 
        }

        // Function that handles all buying operations
        // All this was initially inside the ShopMenu function
        // But i decided to extract all into this one
        // Trying to make things tiddy
        private void BuyMenu(Shop shop, int validatedInput)
        {
            // Just entered this menu, so reset the result operation message
            resultOperation = "";

            // Stay in the buy menu until the user make a purchase or go back
            while (buyMenu == true)
            {
                // Print to the console the shop list and possible options
                shop.Draw();

                // Print the last operation result (begins empty "" by default)
                Console.WriteLine(resultOperation);

                Console.WriteLine($"Money available: {heroControl.GetMoney()}");
                Console.WriteLine("Choose an item to buy from the list! (or 0 to go back)");
                userChoice = Console.ReadLine();

                // Validate the user input
                validatedInput = ValidateChoice(userChoice);

                // -1 means the user typed anything but a number
                if (validatedInput == -1)
                {
                    resultOperation = "You must type a number!";
                }
                // User input is inside the range of possible options (using the shop list count as the maximum number possible) 
                else if (validatedInput >= 1 && validatedInput <= shop.GetListCount())
                {
                    // Check if the hero has enough money to buy the item
                    // The HandleInput_Buy function of the Hero class, handles all operations
                    // on its side. Here, is just used to see if the purchase is possible
                    if (heroControl.HandleInput_Buy(shop.GetItem(validatedInput)) == false)
                    {
                        resultOperation = "Sorry, you dont have enough money for that Hero!";
                    }

                    // Purchase was successfull, so handles the sell in the shop side
                    // The HandleInput_Sell function of the Shop class, handles all operations
                    // on its side.
                    else
                    {
                        resultOperation = $"Successfully bought {shop.GetItem(validatedInput).GetName()}";
                        shop.HandleInput_Sell(shop.GetItem(validatedInput));

                        // Purchased an item, so exit the buy menu
                        buyMenu = false;
                    }
                }

                // User choosed 0, so go back to the shop main menu
                else if (validatedInput == 0)
                {
                    buyMenu = false;

                    // Make sure the result operation message is empty, just in case there
                    // was an error before chosing 0 (otherwise, the message from
                    // the result operation would still appear, making no sense)
                    resultOperation = "";
                }

                // Number typed out of available range
                else
                {
                    resultOperation = $"Please, pick a number between 0 and {shop.GetListCount()}";
                }
            }
        }

        // Function that handles all selling operations
        // All this was initially inside the ShopMenu function
        // But i decided to extract all into this one
        // Trying to make things tiddy
        private void SellMenu(Shop shop, int validatedInput)
        {
            // Just entered this menu, so reset the result operation message
            resultOperation = "";
            
            while (sellMenu == true){

                // Print the hero inventory to the console
                heroControl.Draw();
                Console.WriteLine($"Shop money available: {shop.GetMoneyAvailable()}");

                // Print the last operation result (begins empty "" by default)
                Console.WriteLine(resultOperation);

                // Custom message according to hero inventory
                Console.WriteLine(heroControl.GetListCount() == 0 ? "No items to sell (0 to go back)" : "Choose an item to sell from the list! (or 0 to go back)");

                userChoice = Console.ReadLine();

                // Validate the user input
                validatedInput = ValidateChoice(userChoice);

                // -1 means the user typed anything but a number
                if (validatedInput == -1)
                {
                    resultOperation = "You must type a number!";
                }
                // User input is inside the range of possible options (using the hero inventory count as the maximum number possible) 
                else if (validatedInput >= 1 && validatedInput <= heroControl.GetListCount())
                {
                    // A situation that can happen, but i solved here:
                    // It's not possible to sell an equipped item
                    // So, let's guarantee this
                    Equipment isEquipment = heroControl.GetItem(validatedInput) as Equipment;
                    if(isEquipment != null && isEquipment.GetEquipped() == true)
                    {
                        resultOperation = "It's not possible to sell an equipped item!";
                    }

                    // Check if the shop purchase was sucessfull, the first problem being
                    // trying to sell equipments to consumable shop and vice versa
                    // The HandleInput_Buy function of the Shop class, handles all operations
                    // on its side. Here, is just used to see if the purchase is possible
                    else if (shop.HandleInput_Buy(heroControl.GetItem(validatedInput)) == false)
                    {
                        // Chech if the first problem mentioned above happened
                        if (shop.CanBuyItem(heroControl.GetItem(validatedInput)) == false)
                        {
                            resultOperation = (shop.GetShopType() == "Equipment" ? "I only deal with equipments, Hero!" : "I only deal with consumables, Hero!");
                        }
                        // The purchase failed because of lack of money
                        else
                        {
                            resultOperation = "Sorry Hero, i can't afford that!";
                        }
                    }

                    // Sell was successfull, so handles the sell in the hero side
                    // The HandleInput_Sell function of the Hero class, handles all operations
                    // on its side.
                    else
                    {
                        resultOperation = $"Successfully sold {heroControl.GetItem(validatedInput).GetName()}";
                        heroControl.HandleInput_Sell(heroControl.GetItem(validatedInput));

                        // Sold an item, so exit the buy menu
                        sellMenu = false;
                    }

                }

                // User choosed 0, so go back to the shop main menu
                else if (validatedInput == 0)
                {
                    sellMenu = false;

                    // Make sure the result operation message is empty, just in case there
                    // was an error before chosing 0 (otherwise, the message from
                    // the result operation would still appear, making no sense)
                    resultOperation = "";
                }

                // Number typed out of available range
                else
                {
                    resultOperation = $"Please, pick a number between 0 and {heroControl.GetListCount()}";
                }
            }
        }

        private void HeroInventory(int validatedInput)
        {
            // Just entered this menu, so reset the result operation message
            resultOperation = "";

            // Bool to check if there is equipable items currently in the hero inventory
            bool hasEquipableItems = false;

            // Iterate the hero inventory
            foreach (Items currItem in heroControl.GetInventory())
            {
                Equipment isEquipment = currItem as Equipment;
                // And if there is at least one equipment
                if (isEquipment != null)
                {
                    // Assign that bool as true and continue
                    hasEquipableItems = true;
                    break;
                }
            }

            while (heroInventory == true)
            {
                // Print the hero inventory to the console
                heroControl.Draw();

                // Print the last operation result (begins empty "" by default)
                Console.WriteLine(resultOperation);

                // Custom message according to the hasEquipableItems bool
                Console.WriteLine(heroControl.GetListCount() != 0 && hasEquipableItems == true ? "Choose an item to equip / unequip (or 0 to go back)" : "No items to equip (0 to go back)");
                
                userChoice = Console.ReadLine();

                // Validate the user input
                validatedInput = ValidateChoice(userChoice);

                // -1 means the user typed anything but a number
                if (validatedInput == -1)
                {
                    resultOperation = "You must type a number!";
                }
                // User input is inside the range of possible options (using the hero inventory count as the maximum number possible) 
                else if (validatedInput >= 1 && validatedInput <= heroControl.GetListCount())
                {
                    Equipment isEquipment = heroControl.GetItem(validatedInput) as Equipment;
                    // The function ManageEquipment equip or unequip the selected item and calculate the hero attributes accordingly
                    resultOperation = heroControl.ManageEquipment(heroControl.GetItem(validatedInput));
                }
                // // User choosed 0, so go back to the main menu
                else if (validatedInput == 0)
                {
                    heroInventory = false;

                    // Make sure the result operation message is empty, just in case there
                    // was an error before chosing 0 (otherwise, the message from
                    // the result operation would still appear, making no sense)
                    resultOperation = "";
                }
                // Number typed out of available range
                else
                {
                    resultOperation = $"Please, pick a number between 0 and {heroControl.GetListCount()}";
                }

            }

        }

        public int ValidateChoice(string choice)
        {
            bool success = int.TryParse(choice, out var value);

            if (success)
            {
                return value;
            } else
            {
                return -1;
            }
        }

        
    }
}
