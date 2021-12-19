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

        
        public float? ProductPrice { get; set; }

        public int? ProductQuantity { get; set; }

        
        public float? ProductCost { get; set; }

        public int? ProductCode { get; set; }

        public int? UserId { get; set; }

        public int? PriceSign { get; set; }

        public int? QuantitySign { get; set; }

        public Operators operators { get; set; }






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

            cnn.Close();

            return products;
        }


        public int SaveProduct(int id, Product product)
        {
            Database datab = new Database();
            var cnn = datab.database();

            string ProductName = product.ProductName;
            string ProductType = product.ProductType;
            float ProductPrice = (float)product.ProductPrice;
            float ProductQuantity = (float)product.ProductQuantity;
            float ProductCost = (float)product.ProductCost;
            long ProductCode = (long)product.ProductCode;

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

        public List<Product> GenerateReport(Product product)
        {
            Database datab = new Database();
            var cnn = datab.database();
            List<Product> products = new List<Product>();
            cnn.Open();
            var sql = new StringBuilder();
            sql.Append(@"SELECT * FROM Products WHERE");

            if (product.ProductName != null)
                sql.Append(" ProductName = @ProductName ");
            
            if (product.ProductType != null && product.ProductName==null)
                sql.Append(" ProductType = @ProductType ");
            else if(product.ProductType != null && product.ProductName != null)
                sql.Append(" AND ProductType = @ProductType ");

            if (product.ProductPrice != null && product.ProductType == null && product.ProductName == null)
            {
                if (product.PriceSign == 60)
                    sql.Append(" ProductPrice < @ProductPrice  ");

                if (product.PriceSign == 61)
                    sql.Append(" ProductPrice = @ProductPrice  ");

                if (product.PriceSign == 62)
                    sql.Append(" ProductPrice > @ProductPrice  ");
            }
            else if (product.ProductPrice != null && (product.ProductType != null || product.ProductName != null))
            {
                if (product.PriceSign == 60)
                    sql.Append(" AND ProductPrice < @ProductPrice  ");

                if (product.PriceSign == 61)
                    sql.Append(" AND ProductPrice = @ProductPrice  ");

                if (product.PriceSign == 62)
                    sql.Append(" AND ProductPrice > @ProductPrice  ");

            }
            if (product.ProductQuantity != null && product.ProductPrice == null && product.ProductType == null && product.ProductName == null)
            {
                if (product.QuantitySign == 60)
                    sql.Append(" ProductQuantity < @ProductQuantity  ");

                if (product.QuantitySign == 61)
                    sql.Append(" ProductQuantity = @ProductQuantity  ");

                if (product.QuantitySign == 62)
                    sql.Append(" ProductQuantity > @ProductQuantity  ");
            }
            else if (product.ProductQuantity != null && (product.ProductPrice != null || product.ProductType != null || product.ProductName != null))
            {
                if (product.QuantitySign == 60)
                    sql.Append(" AND ProductQuantity < @ProductQuantity  ");

                if (product.QuantitySign == 61)
                    sql.Append(" AND ProductQuantity = @ProductQuantity  ");

                if (product.QuantitySign == 62)
                    sql.Append(" AND ProductQuantity > @ProductQuantity  ");
            }
            string sql2 = sql.ToString();
            SqlCommand cmd = new SqlCommand(sql2, cnn);


            SqlParameter ProductNameParam = cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
            if (product.ProductName == null)
            {
                ProductNameParam.Value = DBNull.Value;
            }

            SqlParameter ProductTypeParam = cmd.Parameters.AddWithValue("@ProductType", product.ProductType);
            if (product.ProductType == null)
            {
                ProductTypeParam.Value = DBNull.Value;
            }

            SqlParameter ProductPriceParam = cmd.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);
            if (product.ProductPrice == null)
            {
                ProductPriceParam.Value = DBNull.Value;
            }

            SqlParameter ProductQuantityParam = cmd.Parameters.AddWithValue("@ProductQuantity", product.ProductQuantity);
            if (product.ProductQuantity == null)
            {
                ProductQuantityParam.Value = DBNull.Value;
            }
            SqlParameter QuantitySignParam = cmd.Parameters.AddWithValue("@QuantitySign",product.QuantitySign);
            if (product.QuantitySign == null)
            {
                QuantitySignParam.Value = DBNull.Value;
            }

            SqlParameter PriceSignParam = cmd.Parameters.AddWithValue("@PriceSign",product.PriceSign);
            if (product.PriceSign == null)
            {
                PriceSignParam.Value = DBNull.Value;
            }

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
                        UserId = Convert.ToInt32(reader["UserId"])

                    });
                }
            }

            cnn.Close();

            return products;
        }

    }
}
