namespace Carpediem.Repository.Entities
{
    public class PersonEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool Sex { get; set; }
        public string Eps { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public int DocumentNumber { get; set; }
        public int DocumentTypeId { get; set; }
        public int UserId { get; set; }
    }
}