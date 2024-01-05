using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PharmacyManagenmentSystem
{
    public partial class AddOrder : Form
    {
        public string Med_name;
        public decimal Med_price;
        public int Med_quantity;
        public int Med_subTotal;
        public int totalQuantity;

        public int totalItemsCount = 0;
        public decimal totalPriceCount = 0;

        List<string> Items = new List<string>();
        public List<Tuple<string, int, decimal, int>> ListOfMedicine = new List<Tuple<string, int, decimal, int>>();

        IDatabaseAdapter dbAdapter = new DatabaseAdapter();

        public AddOrder()
        {
            InitializeComponent();

        }

        private void TypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            NamecomboBox.SelectedItem = null;
            if (TypecomboBox.SelectedItem != null)
            {
                Items.Clear();
                NamecomboBox.Items.Clear();
                QuantitytextBox.Clear();
                PricetextBox.Clear();
                SubTotaltextBox.Clear();
                string query = "Select MedName from Medicine where MedType = '" + TypecomboBox.SelectedItem.ToString() + "' ";
                Items = dbAdapter.GetInList(query);
                foreach (string item in Items)
                {
                    NamecomboBox.Items.Add(item);
                }
            }
        }

        private void NamecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NamecomboBox.SelectedItem != null)
            {
                QuantitytextBox.Clear();
                PricetextBox.Clear();
                SubTotaltextBox.Clear();

                string query = "Select MedQuantity from Medicine where MedName = '" + NamecomboBox.SelectedItem.ToString() + "' ";
                totalQuantity = int.Parse(dbAdapter.GetInText(query));
                QuantitytextBox.Text = totalQuantity.ToString();

                string query1 = "Select MedPrice from Medicine where MedName = '" + NamecomboBox.SelectedItem.ToString() + "' ";
                PricetextBox.Text = dbAdapter.GetInText(query1);
            }
        }
        private void GetTotalbtn_Click(object sender, EventArgs e)
        {
            if (PricetextBox.Text == null)
            {
                MessageBox.Show("fill all feilds");
            }
            else
            {
                SubTotaltextBox.Clear();
                Med_price = Convert.ToDecimal(PricetextBox.Text);
                Med_quantity = int.Parse(QuantitytextBox.Text);
                Med_subTotal = (int)(Med_quantity * Med_price);
                SubTotaltextBox.Text = Med_subTotal.ToString();
            }
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if ( SubTotaltextBox.Text == null)
            {
                MessageBox.Show("fill all feilds");
            }
            else
            {
                Med_name = NamecomboBox.SelectedItem.ToString();
                Med_quantity = int.Parse(QuantitytextBox.Text);
                Med_price = Convert.ToDecimal(PricetextBox.Text);
                Med_subTotal = int.Parse(SubTotaltextBox.Text);

                Tuple<string, int, decimal, int> medicineTuple = new Tuple<string, int, decimal, int>(Med_name, Med_quantity, Med_price, Med_subTotal);
                ListOfMedicine.Add(medicineTuple);

                dataGridView1.Refresh();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = ListOfMedicine;
                dataGridView1.Columns[0].HeaderText = "Name";
                dataGridView1.Columns[1].HeaderText = "Quantity";
                dataGridView1.Columns[2].HeaderText = "Price";
                dataGridView1.Columns[3].HeaderText = "Total";

                string query = "Insert into InvoiceItem values ('" + Med_name + "','" + Med_quantity + "','" + Med_price + "','" + Med_subTotal + "');";
                dbAdapter.Executecmd(query);

                int quantity = totalQuantity - Med_quantity;

                string query1 = "Update Medicine set MedQuantity = '" + quantity + "' where MedName = '" + Med_name + "';";
                string rec = dbAdapter.Executecmd(query1);

                totalItemsCount++;
                totalPriceCount += Convert.ToDecimal(SubTotaltextBox.Text);

                ResetMethod.Reset(this.Controls);
                MessageBox.Show(rec, totalItemsCount.ToString());
            }
        }
        private void Resetbtn_Click(object sender, EventArgs e)
        {
            ResetMethod.Reset(this.Controls);
        }

        private void Nextbtn_Click(object sender, EventArgs e)
        {
            Billing bill = new Billing();
            bill._medicineList = ListOfMedicine;
            bill.items = totalItemsCount;
            bill.amount = totalPriceCount;
            bill.Show();
            this.Hide();
        }
    }
}
