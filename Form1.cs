using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity.SqlServer;

namespace Entity_Queries
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Models.NORTHWNDEntities db = new Models.NORTHWNDEntities();
        private void Button1_Click(object sender, EventArgs e)
        {

            //var result = from p in db.Products
            //             where p.UnitPrice >= 20 && p.UnitPrice <= 50
            //              orderby p.Unitprice descending
            //             select new
            //             {
            //                 p.ProductID,
            //                 p.ProductName,
            //                 p.UnitPrice,
            //                 p.UnitsInStock,
            //                 p.Category.CategoryName
            //             };

            //dataGridView1.DataSource = result.ToList();

            //dataGridView1.DataSource = db.Products
            //    .Where(x => x.UnitPrice >= 20 && x.UnitPrice <= 50)
            //    .OrderBy(x=> x.UnitPrice)

            //    .Select(x => new
            //    {
            //        UrunId=x.ProductID,
            //        UrunAdi=x.ProductName,
            //        Fiyat=x.UnitPrice,
            //        StokAdet=x.UnitsInStock,
            //        KategoriAdi=x.Category.CategoryName

            //    }).ToList();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //var result = from o in db.Orders
            //             select new
            //             { o.Customer.CompanyName,
            //                 Employee = o.Employee.FirstName + " " + o.Employee.LastName,
            //                 o.OrderID,
            //                 o.OrderDate,
            //                 Shipper = o.Shipper.CompanyName


            //             };
            //dataGridView1.DataSource = result.ToList();



            dataGridView1.DataSource = db.Orders
                .Select(o => new
                {
                    o.Customer.CompanyName,
                    Employee = o.Employee.FirstName + " " + o.Employee.LastName,
                    o.OrderID,
                    o.OrderDate,
                    Shipper = o.Shipper.CompanyName

                }).ToList();


        }

        private void Button3_Click(object sender, EventArgs e)
        {
            //var result = from c in db.Customers
            //             where c.CompanyName.Contains("restaurant")
            //             select c;
            //dataGridView1.DataSource = result.ToList();


            dataGridView1.DataSource = db.Customers.Where(
                x => x.CompanyName.Contains("restaurant")).ToList();

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Models.Product product = new Models.Product();

            int categoryID = db.Categories.FirstOrDefault(x => x.CategoryName == "Beverages").CategoryID;
            Models.Category category = db.Categories.FirstOrDefault(x => x.CategoryName == "Beverages");




            //product.CategoryID = category.CategoryID;
            //product.ProductName = "Kola 1";
            //product.UnitPrice = 5.00M;
            //product.UnitsInStock = 500;

            //db.Products.Add(product);
            //db.SaveChanges();

            //dataGridView1.DataSource = db.Products.Where(x => x.ProductName.Contains("Kola")).ToList();

            db.Categories
                .FirstOrDefault(x => x.CategoryName == "Beverages")
                .Products
                .Add(new Models.Product
                {

                    ProductName = "Kola 2",
                    UnitsInStock = 500,
                    UnitPrice = 5.00M


                }

                );

            db.SaveChanges();
            dataGridView1.DataSource = db.Products.Where(x => x.ProductName.Contains("Kola")).ToList();




        }

        private void Button5_Click(object sender, EventArgs e)
        {


            //dataGridView1.DataSource = db.Employees
            // .Select(o => new
            // {
            //     o.FirstName,
            //     o.LastName,
            //     o.BirthDate,
            //     Age= DateTime.Now.Year - o.BirthDate.Value.Year

            // }).ToList();


            var result = from x in db.Employees
                         select new
                         {
                             x.FirstName,
                             x.LastName,
                             x.BirthDate,
                             Age = DateTime.Now.Year - x.BirthDate.Value.Year,
                             yas = SqlFunctions.DateDiff("yy", x.BirthDate, DateTime.Now)
                         };
            dataGridView1.DataSource = result.ToList();






        }

        private void Button6_Click(object sender, EventArgs e)
        {
            //var result = from p in db.Products
            //             group p by p.Category.CategoryName into g
            //             select new
            //             {
            //                 KategoriAdi=g.Key,
            //                 StokSayisi= g.Count(),
            //                 ToplamStok = g.Sum(p=> p.UnitsInStock)
            //             };
            //dataGridView1.DataSource = result.ToList();








            dataGridView1.DataSource = db.Products.
                GroupBy(x=> new { x.Category.CategoryName, x.Category.Description } )
                .Select(x => new

                {
                    KategoriAdi= x.Key,
                    Urunsayisi = x.Count(),
                    ToplamStok = x.Sum(p=> p.UnitsInStock),
                    Aciklama =  x.Key.Description
                }

                ).ToList();








        }
    }
}
