using StudentApi.Models;

public class ProfessorSubject
{
    public int ProfessorId { get; set; }
    public Professor Professor { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    public class ProfessorSubjectKey
    {
        public int ProfessorId { get; set; }
        public int SubjectId { get; set; }
    }
}
