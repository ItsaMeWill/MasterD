/* Here i present my simple console based, inventory system!
 * Features a simple interaction between the player and 2 shops, 
 * where is possible to buy items from the shop, sell items back 
 * to the shop (but shops buy only specific items,
 * like, an equipment shop only buys equipments!). And remember:
 * shops have a limited ammount of money! Check before selling stuff.
 * And manage your inventory!
 * Equip and unequip those equipments and see your attributes change!
 * Equipments are divided into 4 categories: Helmet, Armor, Pants and Weapons
 * And are limited to only 1 equipped by category (would not make sense using 2 hats, would it?)
 * Equiping the same item will remove it (if it is equipped) and equipping an item of the same type,
 * while having another of that type equipped, will automatically remove the equipped one and equip the new.
 * Attributes changes are reflected on this proccess.
 * 
 * More techy stuff:
 * The ConsoleControl class is responsible for handling all messages printed into the console.
 * The classes Hero and Shop, represents themselfs, and belongs to the ConsoleControl. This way,
 * i can keep things organize into a single, big class.
 * I managed to create some logic into the Shop class, that help maintain its item list organized. Since the player
 * can buy and sell, items are removed from the shop list and added, as these operations happens. This way, i can
 * mantain the list original state, as the items position are reorganized as items are removed / added.
 * The Items class, have every aspect of an item, but is restrict to consumables, only. Theres another type of
 * item, which are represented by the Equipment class which that inherits Items aspects.
 * IPanel is the interface used to help Hero and Items classes to manage the buying and selling, also printing all
 * info into the console (i had to make small changes into it)
 * ListUtils, as the name suggest, is used to reduce code duplication, since it has functions that aids both the Hero and
 * Shop classes better handle some logic involving its items list.
 */
namespace InventorySystem
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleControl InventorySystem = new ConsoleControl(new Hero(5000), new Shop[2] {new Shop(10000, "Equipment"), new Shop(10000, "Consumable")});

            InventorySystem.Init();

        }
    }
}
