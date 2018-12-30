namespace Brewdude.Domain.Entities
{
    public class UserBeers
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BeerId { get; set; }
    }
}