*Ordering System.cs*
using System.Windows.Forms;
using System.Drawing.Printing;

namespace LLEORDERINGSystem
{
    public partial class OrderingSystem : System.Windows.Forms.Form
    {
        private decimal orderTotal = 0;
        private ListBox orderListBox;
        private Label totalLabel;
        private readonly PrintDocument orderPrintDocument = new PrintDocument();
        private PrintPreviewDialog orderPrintPreviewDialog = new PrintPreviewDialog();
        private Menu menu;

        private Dictionary<string, (decimal Price, int Quantity)> orderItems;

        public OrderingSystem()
        {
            this.Text = "LLE ORDERING SYSTEM";
            this.Size = new Size(980, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            orderPrintDocument.PrintPage += OrderPrintDocument_PrintPage;
            orderPrintPreviewDialog.Document = orderPrintDocument;

            menu = new Menu();
            orderItems = new Dictionary<string, (decimal, int)>();
            GenerateUI();
        }

        private void GenerateUI()
        {
            var lblHeader = new Label
            {
                Text = "LLE ORDERING SYSTEM",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.DarkRed,
                AutoSize = true,
                Top = 20,
                Left = 20
            };
            this.Controls.Add(lblHeader);

            var grpMenu = new GroupBox
            {
                Text = "Menu",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = 70,
                Width = 620,
                Height = 460,
                BackColor = Color.White
            };
            this.Controls.Add(grpMenu);

            var flowMenu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            grpMenu.Controls.Add(flowMenu);

            menu.CreateMenuButtons(flowMenu, AddItemToOrder);

            var grpOrder = new GroupBox
            {
                Text = "Order Summary",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 660,
                Top = 70,
                Width = 280,
                Height = 460,
                BackColor = Color.White
            };
            this.Controls.Add(grpOrder);

            orderListBox = new ListBox
            {
                Font = new Font("Segoe UI", 9),
                Width = 240,
                Height = 240,
                Top = 30,
                Left = 20
            };
            grpOrder.Controls.Add(orderListBox);

            totalLabel = new Label
            {
                Text = "Total: ₱0.00",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Top = orderListBox.Bottom + 10,
                Left = 20,
                Width = 240
            };
            grpOrder.Controls.Add(totalLabel);

            var btnPlaceOrder = new Button
            {
                Text = "Place Order",
                Width = 240,
                Height = 40,
                Top = totalLabel.Bottom + 10,
                Left = 20,
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnPlaceOrder.Click += PlaceOrder_Click;
            grpOrder.Controls.Add(btnPlaceOrder);

            var btnClear = new Button
            {
                Text = "Clear Order",
                Width = 240,
                Height = 35,
                Top = btnPlaceOrder.Bottom + 10,
                Left = 20,
                BackColor = Color.Firebrick,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnClear.Click += ClearOrder_Click;
            grpOrder.Controls.Add(btnClear);
        }

        private void AddItemToOrder(string itemName, decimal price)
        {
            if (orderItems.ContainsKey(itemName))
            {
                orderItems[itemName] = (price, orderItems[itemName].Quantity + 1);
            }
            else
            {
                orderItems[itemName] = (price, 1);
            }

            UpdateOrderList();
        }

        private void UpdateOrderList()
        {
            orderListBox.Items.Clear();
            orderTotal = 0;

            foreach (var item in orderItems)
            {
                string itemText = $"{item.Key} x{item.Value.Quantity} - ₱{item.Value.Price * item.Value.Quantity}";
                orderListBox.Items.Add(itemText);
                orderTotal += item.Value.Price * item.Value.Quantity;
            }

            totalLabel.Text = "Total: ₱" + orderTotal.ToString("0.00");
        }

        private void PlaceOrder_Click(object sender, EventArgs e)
        {
            if (orderListBox.Items.Count == 0)
            {
                MessageBox.Show("No items in the order.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show($"Thank you for your order!\nTotal: ₱{orderTotal:0.00}", "Order Placed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            PrintOrder();
            ClearOrder_Click(this, EventArgs.Empty);
        }

        private void ClearOrder_Click(object sender, EventArgs e)
        {
            orderItems.Clear();
            UpdateOrderList();
        }

        private void PrintOrder()
        {
            if (orderListBox.Items.Count == 0)
            {
                MessageBox.Show("No items to print.", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            orderPrintPreviewDialog.ShowDialog();
        }

        private void OrderPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (e.Graphics == null) return;

            Font font = new Font("Segoe UI", 10);
            float lineHeight = font.GetHeight(e.Graphics) + 5;
            float x = 50;
            float y = 50;

            e.Graphics.DrawString("LLE ORDERING SYSTEM RECEIPT", new Font("Segoe UI", 14, FontStyle.Bold), Brushes.Black, x, y);
            y += lineHeight * 2;

            foreach (var item in orderItems)
            {
                e.Graphics.DrawString($"{item.Key} x{item.Value.Quantity} - ₱{item.Value.Price * item.Value.Quantity}", font, Brushes.Black, x, y);
                y += lineHeight;
            }

            y += lineHeight;
            e.Graphics.DrawString($"Total: ₱{orderTotal:0.00}", new Font("Segoe UI", 12, FontStyle.Bold), Brushes.Black, x, y);
            y += lineHeight * 2;

            e.Graphics.DrawString("Thank you for your order!", font, Brushes.Black, x, y);
            y += lineHeight;
            e.Graphics.DrawString("Date: " + DateTime.Now.ToString("MMM dd, yyyy hh:mm tt"), font, Brushes.Black, x, y);
        }
    }
}
