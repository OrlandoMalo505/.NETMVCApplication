using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Product
    {


        public long ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductType { get; set; }

        [Required]
        public float ProductPrice { get; set; }

        [Required]
        public int ProductQuantity { get; set; }

        [Required]
        public float ProductCost { get; set; }

        [Required]
        public int ProductCode { get; set; }

        [Required]
        public int UserId { get; set; }
        





        public List<Product> GetAllProducts(int id)
        {
            Database datab = new Database();
            var cnn = datab.database();
            List<Product> products = new List<Product>();
            cnn.Open();
            SqlCommand cmd = new SqlCommand("select * from Products where UserId=@id", cnn);

            cmd.Parameters.AddWithValue("id", id);

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    products.Add(new Product()
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductCode = Convert.ToInt32(reader["ProductCode"]),
                        ProductCost = Convert.ToInt64(reader["ProductCost"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductPrice = Convert.ToInt64(reader["ProductPrice"]),
                        ProductQuantity = Convert.ToInt32(reader["ProductQuantity"]),
                        ProductType = reader["ProductType"].ToString(),
                        UserId= Convert.ToInt32(reader["UserId"])
                        
                    });
                }
            }
            else
            {
                return null;
            }

            cnn.Close();

            return products;
        }


        public int SaveProduct(int id, Product product)
        {
            Database datab = new Database();
            var cnn = datab.database();

            string ProductName = product.ProductName;
            string ProductType = product.ProductType;
            float ProductPrice = product.ProductPrice;
            float ProductQuantity = product.ProductQuantity;
            float ProductCost = product.ProductCost;
            long ProductCode = product.ProductCode;

            string query = "INSERT INTO Products(ProductName, ProductType, ProductPrice, ProductQuantity, ProductCost, ProductCode, UserId) values (@ProductName , @ProductType ,@ProductPrice ,@ProductQuantity ,@ProductCost ,@ProductCode,@UserId)";

            SqlCommand cmd = new SqlCommand(query, cnn);
            cnn.Open();
            cmd.Parameters.AddWithValue("ProductName", ProductName);
            cmd.Parameters.AddWithValue("ProductType", ProductType);
            cmd.Parameters.AddWithValue("ProductPrice", ProductPrice);
            cmd.Parameters.AddWithValue("ProductQuantity", ProductQuantity);
            cmd.Parameters.AddWithValue("ProductCost", ProductCost);
            cmd.Parameters.AddWithValue("ProductCode", ProductCode);
            cmd.Parameters.AddWithValue("UserId", id);
            int i = cmd.ExecuteNonQuery();
            cnn.Close();
            return i;
        }

        public Product getProductById(int id)
        {
            Database datab = new Database();
            var cnn = datab.database();
            Product product = new Product();
            cnn.Open();
            SqlCommand cmd = new SqlCommand("select * from Products where ProductId=@id", cnn);
            cmd.Parameters.AddWithValue("id", id);
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    product.ProductId = Convert.ToInt64(reader["ProductId"]);
                    product.ProductName = reader["ProductName"].ToString();
                    product.ProductType = reader["ProductType"].ToString();
                    product.ProductPrice = Convert.ToInt64(reader["ProductPrice"]);
                    product.ProductQuantity = Convert.ToInt32(reader["ProductQuantity"]);
                    product.ProductCost = Convert.ToInt64(reader["ProductCost"]);
                    product.ProductCode = Convert.ToInt32(reader["ProductCode"]);
                    product.UserId = (int)reader["UserId"];
                    
                    

                }
            }
            return product;
        }

        public int EditProduct(int Userid, int id,Product product)
        {
            Database datab = new Database();
            var cnn = datab.database();

            string query = "Update Products set ProductName= @ProductName, ProductType=@ProductType, ProductPrice = @ProductPrice, ProductCost=@ProductCost , ProductQuantity=@ProductQuantity, ProductCode=@ProductCode, UserId=@UserId where ProductId=@id";

            SqlCommand cmd = new SqlCommand(query, cnn);
            cnn.Open();
            cmd.Parameters.AddWithValue("ProductName", product.ProductName);
            cmd.Parameters.AddWithValue("ProductType", product.ProductType);
            cmd.Parameters.AddWithValue("ProductPrice", product.ProductPrice);
            cmd.Parameters.AddWithValue("ProductCost", product.ProductCost);
            cmd.Parameters.AddWithValue("ProductQuantity", product.ProductQuantity);
            cmd.Parameters.AddWithValue("ProductCode", product.ProductCode);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("UserId", Userid);


            int i = cmd.ExecuteNonQuery();
            cnn.Close();
            return i;
        }


        public void DeleteProductById(int id)
        {
            Database datab = new Database();
            var cnn = datab.database();
            cnn.Open();

            SqlCommand cmd = new SqlCommand("delete from Products where ProductId=@id", cnn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteScalar();


        }


        public int CheckCode(int code)

        {
            string connect = @"Data Source=DESKTOP-D29MF8M;Initial Catalog=login;User ID=firstapp;Password=login";

            string query = "SELECT COUNT(*) FROM Products WHERE ProductCode = @code";

            using (SqlConnection conn = new SqlConnection(connect))

            {
                using (SqlCommand cmd = new SqlCommand(query, conn))

                {

                    cmd.Parameters.AddWithValue("code", code);

                    conn.Open();

                    return (int)cmd.ExecuteScalar();

                }
            }
        }


    }
}
