using System;

// Resources used by the Console Tables package
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/* Welcome to my little experiment! I tried to simulate an interaction between the player and the shopkeeper.
   The interaction is simple: it is shown through the console an item list and the user chooses which item to buy.*/

// Had to change the namespace to make the Console Tables package work
namespace PrintItemListExercise

{
    class Program
    {

        static void Main(string[] args)
        {
            // Choice is the number selected by the user - resultTransaction will hold the message resulted from the user choice
            string choice, resultTransaction;

            resultTransaction = "";

            // Declare and initialize the hero structure
            Hero hero = new Hero(5000, 0, 0, 0, false);

            // Declare the item list array
            Items[] itemList = new Items[15];

            // Populate the array
            itemList = PopulateItemList(itemList);

            // Loop to keep the shopping going!
            while (hero.GetIsReady() == false)
            {
                Console.WriteLine("AMAZINGLY AWESOME ITEM LIST!\n");
                PrintItemList(itemList);

                Console.WriteLine($"Money available: $ {hero.GetMoney()}");
                Console.WriteLine($"{resultTransaction}");
                Console.WriteLine("Pick the item number you want to buy! (Or 0 if you are ready to leave the shop)");
                choice = Console.ReadLine();

                // Guarantee the user typed a number
                bool success = int.TryParse(choice, out var value);

                if (success)
                {
                    
                    // Confirm the number typed is between the items array or zero
                    if(value >= 0 && value <= 16)
                    {
                        // Zero means exit the shop and end the program
                        if (value == 0)
                        {
                            hero.SetIsReady(true);
                            break;
                        }
                        else {

                            // Iterate the items array
                            for (int i = 0; i < itemList.Length; i++)
                            {
                                // Get the item corresponding the typed number and checks if it is available and if the hero has enough money to buy it
                                if (value == itemList[i].GetPosition() && itemList[i].GetQuantity() > 0 && hero.GetMoney() >= itemList[i].GetPrice())
                                {
                                    // Reduce the hero money by the item price
                                    hero.SetMoney(hero.GetMoney() - itemList[i].GetPrice());

                                    // Reduce the item quantity available
                                    itemList[i].SetQuantity(itemList[i].GetQuantity() - 1);

                                    // Set the transaction message
                                    resultTransaction = $"{itemList[i].GetName()} bought successfully!";
                                    
                                    /* A little experiment, i didnt knew that we could use switch like this
                                       Increase the hero corresponding status */ 
                                    switch (value)
                                    {
                                        case 5: case 6: case 7: case 8:
                                            hero.SetDexterity(hero.GetDexterity() + 1);
                                            break;
                                        case 9: case 10: case 11: case 12:
                                            hero.SetStrength(hero.GetStrength() + 1);
                                            break;
                                        case 13: case 14: case 15: case 16:
                                            hero.SetMagic(hero.GetMagic() + 1);
                                            break;
                                    }
                                   
                                }
                                
                                // No more item of the selected type available
                                else if (value == itemList[i].GetPosition() && itemList[i].GetQuantity() == 0)
                                {
                                    // Set the transaction message
                                    resultTransaction = $"Sorry, no more {itemList[i].GetName()} available!";
                                }
                                
                                // Not enough money, Hero! 
                                else if (value == itemList[i].GetPosition() && hero.GetMoney() < itemList[i].GetPrice())
                                {
                                    // Set the transaction message
                                    resultTransaction = "Sorry Hero, you don't have enough money for that!";
                                }
                            }
                        }

                    } 
                    
                    // Number typed out of available range
                    else {

                        // Set the transaction message
                        resultTransaction = "Please, pick a number between 1 and 16!";
                    }

                } 
                
                // User is trolling and didnt type what the program asks for!
                else
                {
                    // Set the transaction message
                    resultTransaction = "You must type a number!";
                }
                
                // Clear the console to keep things organized
                Console.Clear();
            }

            // Clear the console before the farewell message
            Console.Clear();
            Console.WriteLine(FinalMessage(hero.GetDexterity(), hero.GetStrength(), hero.GetMagic()));

        }

        // Receives an array of items and return them populated
        public static Items[] PopulateItemList(Items[] itemList)
        {
            itemList = new Items[]
            {
                
                // Items constructor: Position, Name, Description, Price, Quantity available, Rarity
                new Items (1, "Life Potion", "A life recovering potion. HP + 30", 150, 10, Rarity.COMMON),
                new Items (2, "Mana Potion", "A wizard must have. MP + 50", 300, 4, Rarity.COMMON),
                new Items (3, "Provisions", "A necessity for long travels. Allows resting in the wilderness", 500, 2, Rarity.COMMON),
                new Items (4, "Exploding Potion", "Try not to drink this. Causes fire damage", 10, 250, Rarity.COMMON),
                new Items (5, "Shadow Beanie", "Make you look sus. Dexterity + 1", 1800, 1, Rarity.LEGENDARY),
                new Items (6, "Hoodie", "Stylish and helps protect your identity. Dexterity + 1", 1200, 1, Rarity.MAGIC),
                new Items (7, "Sneaky Pants", "For those who likes to move silencly. Dexterity + 1", 1500, 1, Rarity.MAGIC),
                new Items (8, "Swiss Knife", "A thousand utilities. Dexterity + 1", 2000, 1, Rarity.RARE),
                new Items (9, "Horny Helm", "Makes your headbuts do extra damage. Strength + 1", 1200, 1, Rarity.COMMON),
                new Items (10, "Dragon? Plate Mail", "Legendary armor made from dragon scales. Or it is from geckos?", 2000, 1, Rarity.LEGENDARY),
                new Items (11, "Sturdy Socks", "Made from heavy and resistant material. Just hope the enemy aims at your feet. Strength + 1", 1500, 1, Rarity.COMMON),
                new Items (12, "Big Hamma", "Its big. Its heavy. It will hurt. Strength + 1", 1800, 1, Rarity.MAGIC),
                new Items (13, "Pointy Hat", "The unique, legendary, trademark of the wizards. Magic + 1", 2000, 1, Rarity.LEGENDARY),
                new Items (14, "Confy Long Sleeve Coat", "Its long, its warm and confy. Perfection. Magic + 1", 1800, 1, Rarity.MAGIC),
                new Items (15, "Astral Pants", "A magic imbued, polka-dot boxer underwear. Magic + 1", 1500, 1, Rarity.RARE),
                new Items (16, "Oak Staff", "Magic staff that increases the user concentration. Or an elderly people tool for balance. Magic + 1", 1200, 1, Rarity.MAGIC),
            };

            return itemList;

        }

        // Print all items info into the console, including the table like design
        public static void PrintItemList(Items[] items)
        {

            // Declare and initialize the table with its headers
            var itemTable = new ConsoleTable("", "Name", "Price", "Quantity", "Rarity", "Description");

            for(int i = 0; i < items.Length; i++)
            {
                
                // Add a row with the respective item info
                itemTable.AddRow(items[i].GetPosition(), items[i].GetName(), "$ " + items[i].GetPrice(), items[i].GetQuantity(), items[i].GetRarity(), items[i].GetDescription());
            }

            // Print the entire table
            itemTable.Write(Format.Alternative);
        }

        // Build the farewell message based on the hero statuses
        public static string FinalMessage(int dexterity, int strength, int magic)
        {

            // Initial message
            string message = "May the gods bless you on your adventure, Hero!";

            // The way of the Rogue
            if(dexterity > strength && dexterity > magic)
            {
                message = "I see you are very sneaky, Hero! I'll maybe call an emergency meeting, cause thats kinda sus...\nI hereby declare you a Rogue!\n" + message;

            } 
            
            // The way of the Warrior
            else if(strength > dexterity && strength > magic)
            {
                message = "Now, thats some big muscles, Hero! Just hope nobody asks you how much is 2 plus 2...\nI hereby declare you a Warrior!\n" + message;

            } 
            
            // The way of the Wizard
            else if(magic > dexterity && magic > strength) 
            {
                message = "I didnt knew you were such a nerd, Hero! Im pretty sure you're not popular around the princesses...\nI hereby declare you a Wizard!\n" + message;

            } 
            
            // The way of the Generalist
            else if((dexterity == strength && strength == magic) && dexterity != 0)
            {
                message = "Interesting path you choose, Hero! Jack of all trades, master of none...\nI hereby declare you useless!...ahem... a Generalist!\n" + message;
            }

            return message;

        }

    }

    // Item rarity
    public enum Rarity
    {
        COMMON,
        MAGIC,
        RARE,
        LEGENDARY
    }

    // Item structure
    public struct Items
    {
        // Item characteristics
        private string name, description;
        private int position, price, quantityAvailable;
        private Rarity itemRarity;
        
        // Item constructor
        public Items(int position, string name, string description, int price, int quantity, Rarity rarity) {
            this.position = position;
            this.name = name;
            this.description = description;
            this.price = price;
            this.quantityAvailable = quantity;
            this.itemRarity = rarity;
        }

        // Getters
        public string GetName () 
        {
            return name;
        }

        public string GetDescription()
        {
            return description;
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

        public int GetPosition()
        {
            return position;
        }

        // Setters
        public void SetQuantity(int quantity)
        {
            this.quantityAvailable = quantity;
        }

    }

    // Hero structure
    public struct Hero
    {
        // Hero characteristics
        private int money, magicPoints, strengthPoints, dexterityPoints;
        private bool isReady;

        // Hero constructor
        public Hero(int money, int magic, int strength, int dexterity, bool ready)
        {
            this.money = money;
            this.magicPoints = magic;
            this.strengthPoints = strength;
            this.dexterityPoints = dexterity;
            this.isReady = ready;
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
        public bool GetIsReady()
        {
            return isReady;
        }

        // Setters
        public void SetMoney(int price)
        {
            this.money = price;
        }

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

        public void SetIsReady(bool ready)
        {
            this.isReady = ready;
        }

    }

       /* MY SOLUTION TO THE EXERCISE CHALLENGE: FORMATING THE ITEM LIST INTO A NICE TABLE
       FROM HERE ON, ITS JUST A COPY PASTE FROM THE CONSOLE TABLES SOURCE CODE (https://github.com/khalidabuhakmeh/ConsoleTables) 
       SO YOU GUYS DONT NEED TO INSTALL THE PACKAGE, ITS ALL HERE
       JUST DID LITTLE TWEAKS TO MAKE IT WORK */
    public class ConsoleTable
    {
        public IList<object> Columns { get; set; }
        public IList<object[]> Rows { get; protected set; }

        public ConsoleTableOptions Options { get; protected set; }
        public Type[] ColumnTypes { get; private set; }

        public static HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),
            typeof(uint), typeof(float)
        };

        public ConsoleTable(params string[] columns)
            : this(new ConsoleTableOptions { Columns = new List<string>(columns) })
        {
        }

        public ConsoleTable(ConsoleTableOptions options)
        {
            Options = options ?? throw new ArgumentNullException("options");
            Rows = new List<object[]>();
            Columns = new List<object>(options.Columns);
        }

        public ConsoleTable AddColumn(IEnumerable<string> names)
        {
            foreach (var name in names)
                Columns.Add(name);
            return this;
        }

        public ConsoleTable AddRow(params object[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (!Columns.Any())
                throw new Exception("Please set the columns first");

            if (Columns.Count != values.Length)
                throw new Exception(
                    $"The number columns in the row ({Columns.Count}) does not match the values ({values.Length})");

            Rows.Add(values);
            return this;
        }

        public ConsoleTable Configure(Action<ConsoleTableOptions> action)
        {
            action(Options);
            return this;
        }

        public static ConsoleTable From<T>(IEnumerable<T> values)
        {
            var table = new ConsoleTable
            {
                ColumnTypes = GetColumnsType<T>().ToArray()
            };

            var columns = GetColumns<T>();

            table.AddColumn(columns);

            foreach (
                var propertyValues
                in values.Select(value => columns.Select(column => GetColumnValue<T>(value, column)))
            ) table.AddRow(propertyValues.ToArray());

            return table;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            // find the longest column by searching each row
            var columnLengths = ColumnLengths();

            // set right alinment if is a number
            var columnAlignment = Enumerable.Range(0, Columns.Count)
                .Select(GetNumberAlignment)
                .ToList();

            // create the string format with padding
            var format = Enumerable.Range(0, Columns.Count)
                .Select(i => " | {" + i + "," + columnAlignment[i] + columnLengths[i] + "}")
                .Aggregate((s, a) => s + a) + " |";

            // find the longest formatted line
            var maxRowLength = Math.Max(0, Rows.Any() ? Rows.Max(row => string.Format(format, row).Length) : 0);
            var columnHeaders = string.Format(format, Columns.ToArray());

            // longest line is greater of formatted columnHeader and longest row
            var longestLine = Math.Max(maxRowLength, columnHeaders.Length);

            // add each row
            var results = Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            var divider = " " + string.Join("", Enumerable.Repeat("-", longestLine - 1)) + " ";

            builder.AppendLine(divider);
            builder.AppendLine(columnHeaders);

            foreach (var row in results)
            {
                builder.AppendLine(divider);
                builder.AppendLine(row);
            }

            builder.AppendLine(divider);

            if (Options.EnableCount)
            {
                builder.AppendLine("");
                builder.AppendFormat(" Count: {0}", Rows.Count);
            }

            return builder.ToString();
        }

        public string ToMarkDownString()
        {
            return ToMarkDownString('|');
        }

        private string ToMarkDownString(char delimiter)
        {
            var builder = new StringBuilder();

            // find the longest column by searching each row
            var columnLengths = ColumnLengths();

            // create the string format with padding
            var format = Format(columnLengths, delimiter);

            // find the longest formatted line
            var columnHeaders = string.Format(format, Columns.ToArray());

            // add each row
            var results = Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            var divider = Regex.Replace(columnHeaders, @"[^|]", "-");

            builder.AppendLine(columnHeaders);
            builder.AppendLine(divider);
            results.ForEach(row => builder.AppendLine(row));

            return builder.ToString();
        }

        public string ToMinimalString()
        {
            return ToMarkDownString(char.MinValue);
        }

        public string ToStringAlternative()
        {
            var builder = new StringBuilder();

            // find the longest column by searching each row
            var columnLengths = ColumnLengths();

            // create the string format with padding
            var format = Format(columnLengths);

            // find the longest formatted line
            var columnHeaders = string.Format(format, Columns.ToArray());

            // add each row
            var results = Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            var divider = Regex.Replace(columnHeaders, @"[^|]", "-");
            var dividerPlus = divider.Replace("|", "+");

            builder.AppendLine(dividerPlus);
            builder.AppendLine(columnHeaders);

            foreach (var row in results)
            {
                builder.AppendLine(dividerPlus);
                builder.AppendLine(row);
            }
            builder.AppendLine(dividerPlus);

            return builder.ToString();
        }

        private string Format(List<int> columnLengths, char delimiter = '|')
        {
            // set right alinment if is a number
            var columnAlignment = Enumerable.Range(0, Columns.Count)
                .Select(GetNumberAlignment)
                .ToList();

            var delimiterStr = delimiter == char.MinValue ? string.Empty : delimiter.ToString();
            var format = (Enumerable.Range(0, Columns.Count)
                .Select(i => " " + delimiterStr + " {" + i + "," + columnAlignment[i] + columnLengths[i] + "}")
                .Aggregate((s, a) => s + a) + " " + delimiterStr).Trim();
            return format;
        }

        private string GetNumberAlignment(int i)
        {
            return Options.NumberAlignment == Alignment.Right
                    && ColumnTypes != null
                    && NumericTypes.Contains(ColumnTypes[i])
                ? ""
                : "-";
        }

        private List<int> ColumnLengths()
        {
            var columnLengths = Columns
                .Select((t, i) => Rows.Select(x => x[i])
                    .Union(new[] { Columns[i] })
                    .Where(x => x != null)
                    .Select(x => x.ToString().Length).Max())
                .ToList();
            return columnLengths;
        }

        // Tweak area: had to change the namespace to match mine
        public void Write(Format format = PrintItemListExercise.Format.Default)
        {
            
            switch (format)
            {
                case PrintItemListExercise.Format.Default:
                    Options.OutputTo.WriteLine(ToString());
                    break;
                case PrintItemListExercise.Format.MarkDown:
                    Options.OutputTo.WriteLine(ToMarkDownString());
                    break;
                case PrintItemListExercise.Format.Alternative:
                    Options.OutputTo.WriteLine(ToStringAlternative());
                    break;
                case PrintItemListExercise.Format.Minimal:
                    Options.OutputTo.WriteLine(ToMinimalString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        private static IEnumerable<string> GetColumns<T>()
        {
            return typeof(T).GetProperties().Select(x => x.Name).ToArray();
        }

        private static object GetColumnValue<T>(object target, string column)
        {
            return typeof(T).GetProperty(column).GetValue(target, null);
        }

        private static IEnumerable<Type> GetColumnsType<T>()
        {
            return typeof(T).GetProperties().Select(x => x.PropertyType).ToArray();
        }
    }

    public class ConsoleTableOptions
    {
        public IEnumerable<string> Columns { get; set; } = new List<string>();
        public bool EnableCount { get; set; } = true;

        /// <summary>
        /// Enable only from a list of objects
        /// </summary>
        public Alignment NumberAlignment { get; set; } = Alignment.Left;

        /// <summary>
        /// The <see cref="TextWriter"/> to write to. Defaults to <see cref="Console.Out"/>.
        /// </summary>
        public TextWriter OutputTo { get; set; } = Console.Out;
    }

    public enum Format
    {
        Default = 0,
        MarkDown = 1,
        Alternative = 2,
        Minimal = 3
    }

    public enum Alignment
    {
        Left,
        Right
    }
}




