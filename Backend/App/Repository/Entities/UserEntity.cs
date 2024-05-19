namespace Carpediem.Repository.Entities
{

    public class UserEntity
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RolID { get; set; }
    }
}