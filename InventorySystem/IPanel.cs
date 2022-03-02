
namespace InventorySystem
{
    interface IPanel
    {
        /// <summary>
        /// Manage the buying of an item
        /// Return a bool to indicate the success or not of the operation
        /// </summary>
        public bool HandleInput_Buy(Items selectedItem);

        /// <summary>
        /// Manage the selling of an item
        /// Return a bool to indicate the success or not of the operation
        /// </summary>
        public bool HandleInput_Sell(Items selectedItem);

        /// <summary>
        /// Função utilizada para imprimir os paineis na consola
        /// </summary>
        public void Draw();
    }

}