using System.Collections.Generic;

namespace StudentApi.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ProfessorSubject> ProfessorSubjects { get; set; }

        public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    }
}
