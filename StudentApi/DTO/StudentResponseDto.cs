namespace StudentApi.DTO
{
    public class StudentResponseDto
    {
        public int StudentRecordId { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
    }

}
