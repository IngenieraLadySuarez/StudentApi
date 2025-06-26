namespace StudentApi.DTO
{
    public class ClassmatesBySubjectDto
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public List<ClassmateDto> Classmates { get; set; }
    }
}
