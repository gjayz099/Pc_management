namespace bl.model
{
    public class Manufacturies
    {
        public Guid Id { get; set; }
        public string ManufactureName {  get; set; }
        public string Specification { get; set; } //ex 8 cores, 16 threads, 3.5 GHz base, 5.2 GHz turbo
        public Guid CategotyID { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }

    }
}
