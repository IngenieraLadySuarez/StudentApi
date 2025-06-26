using System.Collections.Generic;

namespace StudentApi.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<ProfessorSubject> ProfessorSubjects { get; set; }
    }
}
