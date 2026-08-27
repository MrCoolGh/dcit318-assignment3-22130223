using System;
using System.Collections.Generic;
using System.Linq;

namespace Dcit318Assignment3
{
    // Q4(a): Immutable record types
    public record Student(int Id, string FullName, int Age);
    public record Course(int Id, string CourseName, int CreditHours);
    public record Enrollment(int StudentId, int CourseId, DateTime EnrollmentDate);

    // Q4(c): SIS app
    public class StudentInformationSystem
    {
        private readonly List<Student> _students = new();
        private readonly List<Course> _courses = new();
        private readonly List<Enrollment> _enrollments = new();

        public void SeedData()
        {
            _students.AddRange(new[]
            {
                new Student(1, "Nana Osei", 20),
                new Student(2, "Akosua Addo", 22),
                new Student(3, "Yaw Owusu", 19)
            });

            _courses.AddRange(new[]
            {
                new Course(101, "DCIT 318", 3),
                new Course(102, "DCIT 304", 3),
                new Course(103, "MATH 266", 2)
            });

            _enrollments.AddRange(new[]
            {
                new Enrollment(1, 101, DateTime.Now.AddDays(-15)),
                new Enrollment(1, 102, DateTime.Now.AddDays(-10)),
                new Enrollment(2, 103, DateTime.Now.AddDays(-8)),
                new Enrollment(3, 101, DateTime.Now.AddDays(-6)),
                new Enrollment(2, 101, DateTime.Now.AddDays(-4))
            });
        }

        // Q4(b): Local function returning student's courses
        public List<Course> GetStudentCourses(int studentId)
        {
            List<Course> StudentCoursesLocal()
            {
                var courseIds = _enrollments
                    .Where(e => e.StudentId == studentId)
                    .Select(e => e.CourseId)
                    .ToHashSet();

                return _courses
                    .Where(c => courseIds.Contains(c.Id))
                    .ToList();
            }

            return StudentCoursesLocal();
        }

        // Q4(d): Display grouped by student
        public void PrintStudentCourseReport()
        {
            Console.WriteLine("=== QUESTION 4: Student Information System ===");

            var grouped = _enrollments
                .Join(_students, e => e.StudentId, s => s.Id, (e, s) => new { e, s })
                .Join(_courses, es => es.e.CourseId, c => c.Id, (es, c) => new { es.s, c })
                .GroupBy(x => x.s);

            foreach (var group in grouped)
            {
                Console.WriteLine($"Student: {group.Key.FullName} (ID: {group.Key.Id})");
                foreach (var item in group)
                {
                    Console.WriteLine($"  - {item.c.CourseName} ({item.c.CreditHours} credits)");
                }
                Console.WriteLine();
            }

            // Demonstrate local-function query for one student
            int selectedStudent = 1;
            var courses = GetStudentCourses(selectedStudent);
            Console.WriteLine($"Courses for student ID {selectedStudent} (via local function):");
            foreach (var course in courses)
            {
                Console.WriteLine($"  - {course.CourseName}");
            }

            Console.WriteLine();
        }

        public void Run()
        {
            SeedData();
            PrintStudentCourseReport();
        }
    }
}
