using System;
namespace bl.dto
{
    public class Manufacturies
    {
        public string ManufactureName { get; set; }
        public string Specification { get; set; } //ex 8 cores, 16 threads, 3.5 GHz base, 5.2 GHz turbo
        public Guid CategotyID { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }


        public string Validate()
        {
            if (string.IsNullOrEmpty(ManufactureName)) return "Manufacture name is empty or null";

            if (string.IsNullOrEmpty(Specification)) return "Specification is empty or null";

            if (CategotyID == Guid.Empty) return "Category ID is empty";

      
            if (Stock == 0) return "Stock must be a valid integer";


            if (Price == 0) return "Price must be a valid decimal";

            if (string.IsNullOrEmpty(Description)) return "Description is empty or null";

            return ""; // Return empty string if validation passes
        }


        //public static async Task<string> InsertData(bl.dto.Manufacturies dto)
        //{
        //    string err = dto.Validate();

        //    if (string.IsNullOrEmpty(err)) return err;

        //    await bl.data.Manufaturies.InsertDataAsync(dto);

        //    return "";
        //}


        //public static async Task<string> UpdateData(bl.dto.Manufacturies dto, Guid id)
        //{
        //    string err = dto.Validate();

        //    if (string.IsNullOrEmpty(err)) return err;

        //    await bl.data.Manufaturies.UpdateDataAsync(dto, id);

        //    return "";
        //}

    }





}
