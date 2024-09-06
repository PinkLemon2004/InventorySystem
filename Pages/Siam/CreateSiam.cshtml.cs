using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace inventorySystem.Pages.Siam
{
    public class CreateSiamModel : PageModel
    {
        public StockInfo stockInfo = new StockInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            stockInfo.item = Request.Form["item"];
            stockInfo.storeid = Request.Form["storeid"];
            stockInfo.supplier = Request.Form["supplier"];
            stockInfo.amount = Request.Form["amount"];

            if (string.IsNullOrEmpty(stockInfo.item) ||
                string.IsNullOrEmpty(stockInfo.storeid) ||
                string.IsNullOrEmpty(stockInfo.supplier) ||
                string.IsNullOrEmpty(stockInfo.amount))
            {
                errorMessage = "All fields are required.";
                return;
            }

            try
            {
                string connectionString = "Server=tcp:inventory-lemon.database.windows.net,1433;Initial Catalog=inventory2;Persist Security Info=False;User ID=lemon436;Password=@sqlcs436;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO stocks (item, storeid, supplier, amount) 
                                   VALUES (@item, @storeid, @supplier, @amount);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@item", stockInfo.item);
                        command.Parameters.AddWithValue("@storeid", stockInfo.storeid);
                        command.Parameters.AddWithValue("@supplier", stockInfo.supplier);
                        command.Parameters.AddWithValue("@amount", stockInfo.amount);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error: {ex.Message}";
                return;
            }

            stockInfo.item = "";
            stockInfo.storeid = "";
            stockInfo.supplier = "";
            stockInfo.amount = "";

            successMessage = "New item added successfully!";
            Response.Redirect("/Siam/IndexSiam");
        }
    }
}
