*Menu.cs*
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LLEORDERINGSystem
{
    public class Menu
    {
        public Dictionary<string, decimal> MenuItems { get; private set; }

        public Menu()
        {
            MenuItems = new Dictionary<string, decimal>
            {
                { "Cheeseburger", 65 },
                { "Double Cheeseburger", 120 },
                { "Big Mac", 150 },
                { "Quarter Pounder", 145 },
                { "1pc LLE Chicken", 105 },
                { "2pc LLE Chicken", 190 },
                { "Regular Fries", 60 },
                { "Large Fries", 85 },
                { "LLE Spaghetti", 75 },
                { "Coke Float", 55 },
                { "Iced Tea", 45 },
                { "LLE Coffee", 65 },
                { "Sundae", 45 },
                { "Apple Pie", 35 }
            };
        }

        public void CreateMenuButtons(FlowLayoutPanel flowMenu, Action<string, decimal> addItemToOrder)
        {
            foreach (var item in MenuItems)
            {
                var btn = new Button
                {
                    Text = $"{item.Key}\n₱{item.Value}",
                    Width = 170,
                    Height = 60,
                    BackColor = System.Drawing.Color.OrangeRed,
                    ForeColor = System.Drawing.Color.White,
                    Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };

                btn.Click += (s, e) => addItemToOrder(item.Key, item.Value);
                flowMenu.Controls.Add(btn);
            }
        }
    }
}
