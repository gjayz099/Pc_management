using Microsoft.AspNetCore.Http;
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
        public string PictureName { get; set; }


        public string Validate()
        {
            // Check if ManufactureName is null or empty
            if (string.IsNullOrEmpty(ManufactureName)) return "Manufacture name is empty or null";

            // Check if Specification is null or empty
            if (string.IsNullOrEmpty(Specification)) return "Specification is empty or null";

            // Check if CategotyID is an empty GUID
            if (CategotyID == Guid.Empty) return "Category ID is empty";

            // Check if Price is zero
            if (Price == 0) return "Price must be a valid decimal";

            // Check if Stock is zero
            if (Stock == 0) return "Stock must be a valid integer";

            // Return an empty string if all validations pass
            return "";
        }

        public static async Task<string> InsertData(bl.dto.Manufacturies dto, IFormFile pictureFile)
        {
            // Save the picture to a folder and get the file name
            string pictureFileName = await bl.dto.Manufacturies.SavePictureToFolder(pictureFile);

            // Validate the dto object
            string err = dto.Validate();

            // Return the validation error if any
            if (!string.IsNullOrEmpty(err)) return err;

            // Insert data asynchronously using the provided dto and picture file name
            await bl.data.Manufaturies.InsertDataAsync(dto, pictureFileName);

            // Return an empty string indicating success
            return "";
        }

        public static async Task<string> UpdateData(bl.dto.Manufacturies dto, Guid id)
        {
            // Validate the dto object
            string err = dto.Validate();

            // Return the validation error if any
            if (!string.IsNullOrEmpty(err)) return err;

            // Update data asynchronously using the provided dto and ID
            await bl.data.Manufaturies.UpdateDataAsync(dto, id);

            // Return an empty string indicating success
            return "";
        }

        public static async Task<string> SavePictureToFolder(IFormFile pictureFile)
        {
            // Check if the picture file is null or empty
            if (pictureFile == null || pictureFile.Length == 0) return "Manufacture Img is empty or null";

            // Retrieve the save path from the configuration
            var config = bl.ConHelper.SavefileImg();
            string savePath = config.SaveImg;

            // Ensure the directory exists, create if not
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            // Get the extension of the uploaded picture file
            string extension = Path.GetExtension(pictureFile.FileName);

            // Generate a new GUID-based file name with the original extension
            string fileName = Guid.NewGuid().ToString() + extension;

            // Combine the save path and file name to get the full file path
            string filePath = Path.Combine(savePath, fileName);

            // Open a file stream and copy the picture file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }

            // Return the generated file name
            return fileName;
        }

        public static async Task<int> CountManuAsync()
        {
            int count = await bl.data.Manufaturies.CountManuExecuteQueryAsync();

            return count;
        }



    }





}
