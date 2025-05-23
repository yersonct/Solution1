namespace Entity.DTOs
{
    public class RolUserDTO
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int UserId { get; set; }
        public string RolName { get; set; }
        public string UserName { get; set; }
        public bool Active { get; set; }
    }
}