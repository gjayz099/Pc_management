namespace bl.model
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public static async Task<List<bl.model.Customer>> GetAllAsync()
        {

            var ret = await bl.data.Customer.ExecuteQueryAsync();

            return ret;

        }

        public static async Task<bl.model.Customer> GetNameAsync(string firstname)
        {

            var ret = await bl.data.Customer.ExecuteQueryNameAsync(firstname);

            return ret;

        }
    }

}
