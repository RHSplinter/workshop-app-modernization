using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using PartsCatalogAPI.Data;
using PartsCatalogAPI.Models;

namespace PartsCatalogAPI.Controllers
{
    public class ProductsController : ApiController
    {
        private PartsCatalogContext db = new PartsCatalogContext();

        // GET: api/Products
        public IHttpActionResult GetProducts()
        {
            var products = db.Products.Include("Category").ToList();
            return Ok(products);
        }

        // GET: api/Products/5
        public IHttpActionResult GetProduct(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // GET: api/Products/Search?name=brake
        [HttpGet]
        [Route("api/Products/Search")]
        public IHttpActionResult SearchProducts(string name)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var results = new List<Product>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Products WHERE Name LIKE '%" + name + "%'";
                
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new Product
                            {
                                Id = (int)reader["Id"],
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (decimal)reader["Price"],
                                StockQuantity = (int)reader["StockQuantity"],
                                Sku = reader["Sku"].ToString(),
                                CategoryId = (int)reader["CategoryId"]
                            });
                        }
                    }
                }
            }

            return Ok(results);
        }

        // POST: api/Products
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.CreatedDate = DateTime.Now;
            product.IsActive = true;

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // PUT: api/Products/5
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            var existingProduct = db.Products.Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.Sku = product.Sku;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.LastModifiedDate = DateTime.Now;
            existingProduct.IsActive = product.IsActive;

            db.SaveChanges();

            return Ok(existingProduct);
        }

        // DELETE: api/Products/5
        public IHttpActionResult DeleteProduct(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        // GET: api/Products/LowStock
        [HttpGet]
        [Route("api/Products/LowStock")]
        public IHttpActionResult GetLowStockProducts()
        {
            var lowStockProducts = db.Products
                .Where(p => p.StockQuantity < 10)
                .ToList();

            return Ok(lowStockProducts);
        }

        // GET: api/Products/Debug
        [HttpGet]
        [Route("api/Products/Debug")]
        public IHttpActionResult GetDebugInfo()
        {
            var debugMode = ConfigurationManager.AppSettings["EnableDebugMode"];
            
            var info = new
            {
                TotalProducts = db.Products.Count(),
                DatabaseName = db.Database.Connection.Database,
                ServerName = db.Database.Connection.DataSource,
                DebugMode = debugMode,
                AdminUser = ConfigurationManager.AppSettings["AdminUsername"],
                ApiKey = ConfigurationManager.AppSettings["ApiKey"]
            };

            return Ok(info);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
