using static System.Net.WebRequestMethods;
using System.Net;

namespace bl.dto
{
    public class Customer
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        // Validate method to check if Firstname and Lastname are not empty or null
        public string Validate()
        {
            if (string.IsNullOrEmpty(Firstname)) return "Firstname is empty or null";
            if (string.IsNullOrEmpty(Lastname)) return "Lastname is empty or null";
            return "";
        }

        // Method to insert customer information and sales asynchronously
        public static async Task<string> InsertAsync(bl.dto.Customer dtoc)
        {
            // Validate the customer DTO
            string err = dtoc.Validate();


            // Return the validation error if any
            if (!string.IsNullOrEmpty(err)) return err;

      

            // Insert customer asynchronously
            await bl.data.Customer.InsertRequestAsync(dtoc);

            // Insert sales asynchronously
            return ""; // No errors, return empty string
        }

       
        public static async Task<string> SavedataAll(bl.dto.Customer cus, List<bl.dto.Sales> dts)
        {
            var error = await bl.dto.Customer.InsertAsync(cus);
            if (!string.IsNullOrEmpty(error)) return error;

            var ret = await bl.model.Customer.GetNameAsync(cus.Firstname);


            string errors = await bl.dto.Sales.InsertAsync(dts, ret.Id);

            if (!string.IsNullOrEmpty(errors))
            {
                await bl.data.Customer.DeleteRequestAsync(ret.Firstname);
                return errors;

            }


            return "";
           
        }


        public static async Task<int> CountCustomerAsync()
        {
            int count = await bl.data.Customer.CountCustomerExecuteQueryAsync();

            return count;
        }
    }

}
