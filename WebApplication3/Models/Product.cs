using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApplication3.Models
{
    public class Product
    {
        public long? ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductType { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public decimal? ProductPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int? ProductQuantity { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public decimal ProductCost { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public long ProductCode { get; set; }

        public int UserId { get; set; }

        public int? PriceSign { get; set; }

        public int? QuantitySign { get; set; }

        public Operators operators { get; set; }
        public List<Order> Orders { get; set; }


        public List<Product> GetAllProducts(int id)
        {
            Database datab = new Database();
            var cnn = datab.database();
            SqlCommand cmd = new SqlCommand("select * from Products where UserId=@id", cnn);
            cmd.Parameters.AddWithValue("id", id);

            return ListOfProducts(cmd, cnn);
        }


        public List<Product> ListOfProducts(SqlCommand cmd, SqlConnection cnn)
        {
            var products = new List<Product>();
            cnn.Open();
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    products.Add(ProductReader(reader));
                }
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
            var ProductPrice = product.ProductPrice;
            var ProductQuantity = product.ProductQuantity;
            decimal ProductCost = product.ProductCost;
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

        private Product ProductReader(SqlDataReader reader)
        {
            if (reader.HasRows)
            {
                return new Product()
                {
                    ProductId = Convert.ToInt64(reader["ProductId"]),
                    ProductName = reader["ProductName"].ToString(),
                    ProductType = reader["ProductType"].ToString(),
                    ProductPrice = Convert.ToDecimal(reader["ProductPrice"]),
                    ProductQuantity = Convert.ToInt32(reader["ProductQuantity"]),
                    ProductCost = Convert.ToDecimal(reader["ProductCost"]),
                    ProductCode = Convert.ToInt32(reader["ProductCode"]),
                    UserId = (int)reader["UserId"]
                };
            }
            return null;
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
            return ProductReader(reader);

        }

        public int EditProduct(int Userid, int id, Product product)
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
           
            Database datab = new Database();
            var cnn = datab.database();
           
            string query = "SELECT COUNT(*) FROM Products WHERE ProductCode = @code";
            SqlCommand cmd = new SqlCommand(query, cnn);
            
            cmd.Parameters.AddWithValue("code", code);
            cnn.Open();
            int i=(int)cmd.ExecuteScalar();
            cnn.Close();
            return i;

        }

        public List<Product> GenerateReport(Product product, bool isUser)
        {
            Database datab = new Database();
            var cnn = datab.database();
            SqlCommand cmd = new SqlCommand("", cnn);
            string sql = "SELECT * FROM Products WHERE ProductId IS NOT NULL";

            if (isUser)
                sql += " AND UserId = @UserId ";
            cmd.Parameters.AddWithValue("@UserId", product.UserId);



            if (!string.IsNullOrEmpty(product.ProductName))
            {
                sql += " and ProductName = @ProductName ";
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
            }

            if (product.ProductType != null)
            {
                sql += " and ProductType = @ProductType ";
                cmd.Parameters.AddWithValue("@ProductType", product.ProductType);
            }


            if (product.ProductPrice != null)
            {
                if (product.PriceSign == (int)Operators.LESS)
                    sql += " AND ProductPrice < @ProductPrice  ";

                if (product.PriceSign == (int)Operators.EQUAL)
                    sql += " AND ProductPrice = @ProductPrice  ";

                if (product.PriceSign == (int)Operators.GREATER)
                    sql += " AND ProductPrice > @ProductPrice  ";

                cmd.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);
            }

            if (product.ProductQuantity != null)
            {
                if (product.QuantitySign == (int)Operators.LESS)
                    sql += " AND ProductQuantity < @ProductQuantity  ";

                if (product.QuantitySign == (int)Operators.EQUAL)
                    sql += " AND ProductQuantity = @ProductQuantity  ";

                if (product.QuantitySign == (int)Operators.GREATER)
                    sql += " AND ProductQuantity > @ProductQuantity  ";

                cmd.Parameters.AddWithValue("@ProductQuantity", product.ProductQuantity);
            }


            cmd.CommandText = sql;
            return ListOfProducts(cmd, cnn);
        }

    }
}
