using System.Collections.Generic;
using System.Linq;

namespace InventorySystem
{
    // This class was created to avoid code duplication.
    // Since both the Shop and Hero classes was using these exact same functions
    // i decided to encapsulated these in this class and make use of the heritage paradigm
    public class ListUtils
    {

        // Implement sorting interface function: set the position of all items
        // The Hero inventory does not use a fixed item position in the list, so, thats what the 
        // parameter isFromShop is used for. To specify who called this function
        // True = from the Shop class
        // False = from the Hero class
        public List<Items> OrganizeList(List<Items> itemList, bool isFromShop)
        {
            // Iterate the shop item list.
            // Count - 1 because of the difference between the count return and the index (0 based)
            for (int i = 0; i <= itemList.Count - 1; i++)
            {
                Equipment isEquipment = itemList[i] as Equipment;

                // SetPosition receives a value depending on the whichClass parameter
                itemList[i].SetPosition(isFromShop ? itemList[i].GetFixedPosition() : i + 1);

            }

            // And then is sorted by the position
            List<Items> sortedList = itemList.OrderBy(currItem => currItem.GetPosition()).ToList();

            return sortedList;

        }

        // Function to check the existence of a given item on a list
        public int CheckDuplicateItem(Items itemToCheck, List<Items> itemList)
        {
            int index = itemList.FindIndex(item => item.GetName() == itemToCheck.GetName());
            return index;
        }

    }
}
