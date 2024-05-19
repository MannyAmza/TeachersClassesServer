using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Globalization;
using TeachersClassesServer.Data;
using TeachersClassesServer.Models;

namespace TeachersClassesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController(ClassSourceContext db, IHostEnvironment environment,
        UserManager<CoursesUser> userManager, ILogger<SeedController> logger)
        : ControllerBase
    {
        private readonly string _pathName = Path.Combine(environment.ContentRootPath, "Data/csun_comp_courses_sp24.csv");

        [HttpPost("User")]
        public async Task<ActionResult> SeedUsers()
        {
            (string name, string email) = ("CSUNStudentName", "comp584@csun.edu");
         CoursesUser user = new()
            {
                UserName = name,
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            if (await userManager.FindByNameAsync(name) is not null)
            {
                user.UserName = "user2";
            }
            _ = await userManager.CreateAsync(user, "CSUN_Stud3nt_P@55w0rd!")
                    ?? throw new InvalidOperationException();
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Courses")]
        public async Task<ActionResult<int>> SeedCourse()
        {
            // Ensure that the Class entities exist first
            await SeedClass();

            Dictionary<string, Class> classes = await db.Classes
                .ToDictionaryAsync(c => c.Cnum, StringComparer.OrdinalIgnoreCase);

            int existingProfessorCount = await db.Professors.CountAsync();
            logger.LogInformation($"Existing professors: {existingProfessorCount}");

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            int courseCount = 0;
            using (StreamReader reader = new(_pathName))
            using (CsvReader csv = new(reader, config))
            {
                IEnumerable<CsunCompCoursesCSV>? records = csv.GetRecords<CsunCompCoursesCSV>();
                foreach (CsunCompCoursesCSV record in records)
                {
                    if (classes.TryGetValue(record.CatalogNum, out Class? @class))
                    {
                        Professor professor = new()
                        {
                            Professor1 = record.Professor,
                            CourseNum = (int)record.ClassNum,
                            Days = record.Days,
                            Time = record.Time,
                            Location = record.Location,
                            ClassId = @class.ClassId
                        };
                        db.Professors.Add(professor);
                        courseCount++;
                    }
                    else
                    {
                        logger.LogWarning($"Class not found for {record.Professor}");
                    }
                }
                await db.SaveChangesAsync();
            }

            int newProfessorCount = await db.Professors.CountAsync() - existingProfessorCount;
            logger.LogInformation($"New professors added: {newProfessorCount}");

            return courseCount;
        }

        [HttpPost("Class")]
        public async Task<ActionResult<Professor>> SeedClass()
        {
            // create a lookup dictionary containing all the classes already existing 
            // into the Database (it will be empty on first run).
            Dictionary<string, Class> classesByName = db.Classes
                .AsNoTracking().ToDictionary(x => x.Cname, StringComparer.OrdinalIgnoreCase);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);

            List<CsunCompCoursesCSV> records = csv.GetRecords<CsunCompCoursesCSV>().ToList();
            foreach (CsunCompCoursesCSV record in records)
            {
                if (classesByName.ContainsKey(record.Title))
                {
                    continue;
                }

                Class @class = new()
                {
                    Cname = record.Title,
                    Cnum = record.CatalogNum,
                };
                await db.Classes.AddAsync(@class);
                classesByName.Add(record.Title, @class);
            }

            await db.SaveChangesAsync();

            return new JsonResult(classesByName.Count);
        }
    }
}
