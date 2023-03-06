namespace WebAPIProject.Models
{
    public class City
    {
        public City()
        {
            Photos = new List<Photo>(); 
        }
        public int Id { get; set; }
        public string Description { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public List<Photo> Photos { get; set; } //NullReferenceExcepiton can be exist(line 7)

        public User User { get; set; }
    }
}
