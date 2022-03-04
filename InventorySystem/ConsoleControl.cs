using System;

namespace InventorySystem
{
    class ConsoleControl
    {
        // Array of shops
        public Shop[] shopControl;
        public Hero heroControl;

        // Strings that control the user inputs and the result (success or failure)
        public string userChoice, resultOperation;

        // Bools that controls the loops of the various screens
        // The bool multipleBuy / multiplesSell is a way i managed to, inside a shop menu loop, give the user the choice of buying / selling multiple items
        public bool shopMenu, buyMenu, sellMenu, mainMenu, heroInventory, multipleBuy, multipleSell;

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

            while (mainMenu == true)
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

            // This variable is used when inside the multiple items buy loop
            int ammountToBuy;

            // Stay in the buy menu until the user make a purchase or go back
            while (buyMenu)
            {
                // GoTo label used to support the multiple item buy
                MultipleBuyGoTo:
                
                // Print to the console the shop list and possible options
                shop.Draw();

                // Print the last operation result (begins empty "" by default)
                Console.WriteLine(resultOperation);

                Console.WriteLine($"Money available: {heroControl.GetMoney()}");
                
                // This bool controls the multiple item buy menu
                if (multipleBuy)
                {
                    Console.WriteLine("Choose the ammount to buy (or 0 to go back)");
                    userChoice = Console.ReadLine();
                    
                    // Now we use the ammountToBuy variable, preserving the value in the validatedInput variable (the item that was choosen)
                    ammountToBuy = ValidateChoice(userChoice);

                    // -1 means the user typed anything but a number 
                    if (ammountToBuy == -1)
                    {
                        resultOperation = "You must type a number!";

                        // Using the goto statement to go back to the beginning of the buyMenu loop
                        goto MultipleBuyGoTo;
                    }

                    // User input is inside the range of possible options (using the selected item quantity as the maximum number possible) 
                    else if (ammountToBuy >= 1 && ammountToBuy <= shop.GetItem(validatedInput).GetQuantity())
                    {
                        // Check if the hero has enough money to buy the ammount specified of the selected item
                        if (heroControl.GetMoney() >= shop.GetItem(validatedInput).GetPrice() * ammountToBuy)
                        {
                            // Iterate the ammount of times necessary to compute all the buys (hero side) and all the sells (shop side)
                            for(int i = 1; i <= ammountToBuy; i++)
                            {
                                heroControl.HandleInput_Buy(shop.GetItem(validatedInput));
                                shop.HandleInput_Sell(shop.GetItem(validatedInput));
                            }
                            // Set the bool back to false, set the result message and goto MultipleBuyGoTo
                            multipleBuy = false;
                            resultOperation = $"Successfully bought {shop.GetItem(validatedInput).GetName()} x {ammountToBuy}";
                            goto MultipleBuyGoTo;
                        }
                        // Hero dont have enough money to buy the quantity specified
                        else
                        {
                            resultOperation = "Sorry, you dont have enough money for that Hero!";
                            goto MultipleBuyGoTo;
                        }
                    }
                    // User choosed 0, so go back to the shop buy menu 
                    else if (ammountToBuy == 0)
                    {
                        resultOperation = "";
                        multipleBuy = false;
                        goto MultipleBuyGoTo;
                    }
                    // Number typed out of available range
                    else
                    {
                        resultOperation = $"Please, pick a number between 1 and {shop.GetItem(validatedInput).GetQuantity()}";
                        goto MultipleBuyGoTo;
                    }

                }
                // The normal buy menu flow
                else 
                Console.WriteLine(shop.GetListCount() == 0 ? "No item available to sell! (0 to go back)" : "Choose an item to buy from the list! (or 0 to go back)");
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
                    // Here, if the selected item has more than 1 quantity available, then we ignore the normal flow
                    // and proceed to the multiple item buy flow
                    if (shop.GetItem(validatedInput).GetQuantity() > 1 && multipleBuy == false)
                    {
                        // Set the bool that controls the multiple item buy flow and call the function again
                        // With this setup and with the use of recursion, we ensure that the flow will go where i want
                        multipleBuy = true;
                        BuyMenu(shop, validatedInput);
                    }

                    // Check if the hero has enough money to buy the item
                    // The HandleInput_Buy function of the Hero class, handles all operations
                    // on its side. Here, is just used to see if the purchase is possible
                    else if (heroControl.HandleInput_Buy(shop.GetItem(validatedInput)) == false)
                    {
                        resultOperation = "Sorry, you dont have enough money for that Hero!";
                    }

                    // Purchase was successfull, so handles the sell in the shop side
                    // The HandleInput_Sell function of the Shop class, handles all operations
                    // on its side.
                    else {
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
                    resultOperation = shop.GetListCount() == 0 ? "No item available to buy! (0 to go back)" : $"Please, pick a number between 1 and {shop.GetListCount()}";
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

            // This variable is used when inside the multiple items sell loop
            int ammountToSell;

            while (sellMenu){

                // GoTo label used to support the multiple item sell
                MultipleSellGoTo:

                // Print the hero inventory to the console
                heroControl.Draw();
                Console.WriteLine($"Shop money available: {shop.GetMoneyAvailable()}");

                // Print the last operation result (begins empty "" by default)
                Console.WriteLine(resultOperation);

                // This bool controls the multiple item sell menu
                if (multipleSell)
                {

                    Console.WriteLine("Choose the ammount to sell (or 0 to go back)");
                    userChoice = Console.ReadLine();

                    // Now we use the ammountToSell variable, preserving the value in the validatedInput variable (the item that was choosen)
                    ammountToSell = ValidateChoice(userChoice);

                    // -1 means the user typed anything but a number 
                    if (ammountToSell == -1)
                    {
                        resultOperation = "You must type a number!";

                        // Using the goto statement to go back to the beginning of the sellMenu loop
                        goto MultipleSellGoTo;
                    }

                    // User input is inside the range of possible options (using the selected item quantity as the maximum number possible) 
                    else if (ammountToSell >= 1 && ammountToSell <= heroControl.GetItem(validatedInput).GetQuantity())
                    {
                        // Check if the shop has enough money to buy the ammount specified of the selected item
                        if (shop.GetMoneyAvailable() >= heroControl.GetItem(validatedInput).GetPrice() * ammountToSell)
                        {
                            // String used to hold the name of the items being sold
                            // Was using resultOperation = $"Successfully sold {heroControl.GetItem(validatedInput)} x {ammountToSell}"; below
                            // But since if there is not more of the item in the hero inventory, an error would occurr (null object)
                            // This string solves that problem
                            string itemName = heroControl.GetItem(validatedInput).GetName();

                            // Iterate the ammount of times necessary to compute all the buys (shop side) and all the sells (hero side)
                            for (int i = 1; i <= ammountToSell; i++)
                            {
                                shop.HandleInput_Buy(heroControl.GetItem(validatedInput));
                                heroControl.HandleInput_Sell(heroControl.GetItem(validatedInput));
                            }
                            // Set the bool back to false, set the result message and goto MultipleSellGoTo
                            multipleSell = false;
                            resultOperation = $"Successfully sold {itemName} x {ammountToSell}";
                            goto MultipleSellGoTo;
                        }
                        // Shop dont have enough money to buy the quantity specified
                        else
                        {
                            resultOperation = "Sorry Hero, i can't afford that!";
                            goto MultipleSellGoTo;
                        }
                    }
                    // User choosed 0, so go back to the hero sell menu 
                    else if (ammountToSell == 0)
                    {
                        resultOperation = "";
                        multipleSell = false;
                        goto MultipleSellGoTo;
                    }
                    // Number typed out of available range
                    else
                    {
                        resultOperation = $"Please, pick a number between 1 and {heroControl.GetItem(validatedInput).GetQuantity()}";
                        goto MultipleSellGoTo;
                    }

                }
                // The normal sell menu flow
                else
                // Custom message according to hero inventory
                Console.WriteLine(heroControl.GetListCount() == 0 ? "No items available to sell (0 to go back)" : "Choose an item to sell from the list! (or 0 to go back)");
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
                    if (isEquipment != null && isEquipment.GetEquipped() == true)
                    {
                        resultOperation = "It's not possible to sell an equipped item!";
                    }

                    // Check if the shop can buy the item (Equipment shop only buy equipments and vice versa)
                    else if (shop.CanBuyItem(heroControl.GetItem(validatedInput)) == false)
                    {
                        resultOperation = (shop.GetShopType() == "Equipment" ? "I only deal with equipments, Hero!" : "I only deal with consumables, Hero!");
                    }

                    // Here, if the selected item has more than 1 quantity available, then we ignore the normal flow
                    // and proceed to the multiple item sell flow
                    else if (heroControl.GetItem(validatedInput).GetQuantity() > 1 && multipleSell == false)
                    {
                        // Set the bool that controls the multiple item sell flow and call the function again
                        // With this setup and with the use of recursion, we ensure that the flow will go where i want
                        multipleSell = true;
                        SellMenu(shop, validatedInput);
                    }

                    // Check if the shop has enough money to buy the item
                    else if(shop.GetMoneyAvailable() < heroControl.GetItem(validatedInput).GetPrice())
                    {
                        resultOperation = "Sorry Hero, i can't afford that!";
                    }

                    // Sell was successfull, so handles the sell in the hero side
                    // The HandleInput_Sell function of the Hero class, handles all operations
                    // on its side.
                    else
                        {
                            resultOperation = $"Successfully sold {heroControl.GetItem(validatedInput).GetName()}";
                            shop.HandleInput_Buy(heroControl.GetItem(validatedInput));
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
                    resultOperation = heroControl.GetListCount() == 0 ? "No items available to sell! (0 to go back)" : $"Please, pick a number between 1 and {heroControl.GetListCount()}";
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
                    resultOperation = heroControl.GetListCount() == 0 ? "No items to equip! (0 to go back)" : $"Please, pick a number between 1 and {heroControl.GetListCount()}";
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
