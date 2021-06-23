namespace API.Entities
{
    public class Photo
    {
        public int ID { get; set; }
        public string URL { get; set; }
        public bool IsMain { get; set; }
        public string PublicID { get; set; }

    }
}