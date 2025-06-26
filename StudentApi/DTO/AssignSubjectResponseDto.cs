namespace StudentApi.DTO
{
    public class AssignSubjectResponseDto
    {
        public int StudentId { get; set; }
        public string Student { get; set; } = null!;
        public int SubjectId { get; set; }
        public string Subject { get; set; } = null!;
        public string? Professor { get; set; } = null!;
    }

}
