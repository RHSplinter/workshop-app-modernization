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
    // SECURITY ISSUE: No authorization required
    public class CategoriesController : ApiController
    {
        private PartsCatalogContext db = new PartsCatalogContext();

        // GET: api/Categories
        public IHttpActionResult GetCategories()
        {
            var categories = db.Categories.ToList();
            return Ok(categories);
        }

        // GET: api/Categories/5
        public IHttpActionResult GetCategory(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // SECURITY ISSUE: SQL Injection vulnerability
        // GET: api/Categories/ByName?name=Brakes
        [HttpGet]
        [Route("api/Categories/ByName")]
        public IHttpActionResult GetCategoryByName(string name)
        {
            // Vulnerable to SQL injection
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Category result = null;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // SECURITY ISSUE: Direct string concatenation
                string query = "SELECT * FROM Categories WHERE Name = '" + name + "'";
                
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Category
                            {
                                Id = (int)reader["Id"],
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                ImageUrl = reader["ImageUrl"].ToString(),
                                IsActive = (bool)reader["IsActive"]
                            };
                        }
                    }
                }
            }

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/Categories/5/Products
        [HttpGet]
        [Route("api/Categories/{id}/Products")]
        public IHttpActionResult GetCategoryProducts(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            var products = db.Products.Where(p => p.CategoryId == id).ToList();
            return Ok(products);
        }

        // SECURITY ISSUE: No authorization for category creation
        // POST: api/Categories
        public IHttpActionResult PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            category.IsActive = true;
            db.Categories.Add(category);
            db.SaveChanges(); // ISSUE: Synchronous operation

            return CreatedAtRoute("DefaultApi", new { id = category.Id }, category);
        }

        // SECURITY ISSUE: No authorization for category updates
        // PUT: api/Categories/5
        public IHttpActionResult PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.Id)
            {
                return BadRequest();
            }

            var existingCategory = db.Categories.Find(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
            existingCategory.ImageUrl = category.ImageUrl;
            existingCategory.IsActive = category.IsActive;

            db.SaveChanges(); // ISSUE: Synchronous operation

            return Ok(existingCategory);
        }

        // SECURITY ISSUE: No authorization for category deletion
        // DELETE: api/Categories/5
        public IHttpActionResult DeleteCategory(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            // ISSUE: No check for existing products in category
            db.Categories.Remove(category);
            db.SaveChanges(); // ISSUE: Synchronous operation

            return Ok(category);
        }

        // SECURITY ISSUE: Mass update without authorization
        // PUT: api/Categories/BulkUpdate
        [HttpPut]
        [Route("api/Categories/BulkUpdate")]
        public IHttpActionResult BulkUpdateCategories(List<Category> categories)
        {
            foreach (var category in categories)
            {
                var existingCategory = db.Categories.Find(category.Id);
                if (existingCategory != null)
                {
                    existingCategory.Name = category.Name;
                    existingCategory.Description = category.Description;
                    existingCategory.IsActive = category.IsActive;
                }
            }

            db.SaveChanges(); // ISSUE: Synchronous operation

            return Ok(new { Updated = categories.Count });
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
