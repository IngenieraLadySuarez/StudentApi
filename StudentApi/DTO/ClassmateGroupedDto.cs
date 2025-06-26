namespace StudentApi.DTO
{
    public class ClassmateGroupedDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public List<string> SharedSubjects { get; set; } = new();
    }

}
