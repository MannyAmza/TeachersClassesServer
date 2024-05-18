namespace TeachersClassesServer.DTO
{
    public class ProfessorDto
    {
        public int ProfessorId { get; set; }
        public string ProfessorName { get; set; } = null!;
        public int CourseNum { get; set; }
        public string Days { get; set; } = null!;
        public string Time { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Class { get; set; } = null!;

    }
}
