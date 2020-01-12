using Entity_Queries.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Entity_Queries
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
           
        }
        NORTHWNDEntities db = new NORTHWNDEntities();

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = db.Categories.ToList();
            comboBox1.DisplayMember = "CategoryName";
            comboBox1.ValueMember = "CategoryId";

            comboBox2.DataSource = db.Categories.ToList();
            comboBox2.DisplayMember = "CategoryName";
            comboBox2.ValueMember = "CategoryId";

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int categoryId = (int)comboBox1.SelectedValue;
            MessageBox.Show(categoryId.ToString());

            Product product = new Product();
            product.ProductName = textBox1.Text;
            product.CategoryID = categoryId;
            product.UnitsInStock = (short) numericUpDown1.Value;
            product.UnitPrice = numericUpDown2.Value;
            db.Products.Add(product);
            bool result=  db.SaveChanges()>0;
            MessageBox.Show(result ? "Ürün Başarıyla Eklendi" :  "Ürün ekleme hatası");
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
            Listele(categoryId);

      

        }

        public void Listele(int id)
        {
            dataGridView1.DataSource = dataGridView1.DataSource = db.Products
                .Where(x=>x.CategoryID==id)
                .Select(o => new
                {
                    o.ProductID,
                    o.ProductName,
                    o.Category.CategoryName,
                    o.UnitPrice,
                    o.UnitsInStock
                    

                }).ToList();



        }

        void Temizle()
        {
            foreach (Control item in groupBox1.Controls)
            {

                if (item is TextBox)
                {
                    item.Text = "";
                }
                else if(item is ComboBox)
                {
                    ComboBox cmb = (ComboBox)item;
                    cmb.SelectedIndex = -1;
                }
                else if (item is NumericUpDown)
                {
                    NumericUpDown nmr =(NumericUpDown) item;
                    nmr.Value = nmr.Minimum;
                }


            }

        }

        private void ComboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int categoryId = (int)comboBox2.SelectedValue;
            Listele(categoryId);
        }
        Product product ;
        private void DataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count>0)

            {
                int id =(int)  dataGridView1.SelectedRows[0].Cells[0].Value;
                product = db.Products.Find(id);
                textBox1.Text = product.ProductName;
                numericUpDown1.Value = product.UnitsInStock!=null? product.UnitsInStock.Value: numericUpDown1.Minimum  ;
                numericUpDown2.Value = product.UnitPrice != null ? product.UnitPrice.Value : numericUpDown2.Minimum;
                comboBox1.SelectedValue = product.CategoryID;



            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {

            int id = (int) comboBox1 .SelectedValue;
            product.ProductName = textBox1.Text;
            product.UnitPrice = numericUpDown2.Value;
            product.UnitsInStock = Convert.ToInt16( numericUpDown1.Value);
            product.CategoryID =id;
            db.Entry(product).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            Temizle();
            Listele(id);


        }
        private void SilToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    try
                    {
                        int id = (int)item.Cells[0].Value;
                        db.Products.Remove(db.Products.Find(id));
                        db.SaveChanges();
                    }
                    catch
                    {
                        continue;
                    }
                    Listele((int)comboBox2.SelectedValue);
                }
            }
        }
    }
}
