namespace TeachersClassesServer.Data
{
    public class CsunCompCoursesCSV
    {
        //Professor,ClassNum,CatalogNum,Title,Days,Time,Location
        public string Professor { get; set; } = null!;
        public decimal ClassNum { get; set; }
        public string CatalogNum { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Days { get; set; } = null!;
        public string Time { get; set; } = null!;
        public string Location { get; set; } = null!;

    }
}
