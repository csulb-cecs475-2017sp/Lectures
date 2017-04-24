using Cecs475.Scheduling.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test {
	class Program {
		static void Main(string[] args) {

			CatalogContext con = new CatalogContext();

			int choice = -1;
			do {
				Console.WriteLine("Menu:\n0. Quit\n1. Populate database\n2. Show courses\n3. Show course sections");
				choice = Convert.ToInt32(Console.ReadLine());

				switch (choice) {
					case 1:
						// Add some courses to the catalog
						var cecs174 = new CatalogCourse() {
							DepartmentName = "CECS",
							CourseNumber = "174",
						};
						con.Courses.Add(cecs174);

						var cecs274 = new CatalogCourse() {
							DepartmentName = "CECS",
							CourseNumber = "274",
						};
						cecs274.Prerequisites.Add(cecs174);
						con.Courses.Add(cecs274);

						var cecs228 = new CatalogCourse() {
							DepartmentName = "CECS",
							CourseNumber = "228",
						};
						cecs228.Prerequisites.Add(cecs174);
						con.Courses.Add(cecs228);

						var cecs277 = new CatalogCourse() {
							DepartmentName = "CECS",
							CourseNumber = "277",
						};
						cecs277.Prerequisites.Add(cecs274);
						cecs277.Prerequisites.Add(cecs228);
						con.Courses.Add(cecs277);

						// Add a semester term
						var fall2017 = new SemesterTerm() {
							Name = "Fall 2017",
							StartDate = new DateTime(2017, 8, 21),
							EndDate = new DateTime(2017, 12, 22)
						};
						con.SemesterTerms.Add(fall2017);

                        var spring2017 = new SemesterTerm() {
                            Name = "Spring 2017",
                            StartDate = new DateTime(2017, 1, 23),
                            EndDate = new DateTime(2017, 5, 26)
                        };
                        con.SemesterTerms.Add(spring2017);

						// Add instructors
						var neal = new Instructor() {
							FirstName = "Neal",
							LastName = "Terrell",
						};
						con.Instructors.Add(neal);

						var anthony = new Instructor() {
							FirstName = "Anthony",
							LastName = "Giaccalone"
						};
						con.Instructors.Add(anthony);

						// Add sections
						var cecs228_01 = new CourseSection() {
							CatalogCourse = cecs228,
							SectionNumber = 1,
							Instructor = neal,
							MeetingDays = DaysOfWeek.Tuesday | DaysOfWeek.Thursday,
							StartTime = new DateTime(2017, 1, 1, 9, 0, 0), // 9 am
							EndTime = new DateTime(2017, 1, 1, 9, 50, 0),
						};
						fall2017.CourseSections.Add(cecs228_01);

						var cecs274_11 = new CourseSection() {
							CatalogCourse = cecs274,
							SectionNumber = 11,
							Instructor = anthony,
							MeetingDays = DaysOfWeek.Monday| DaysOfWeek.Wednesday | DaysOfWeek.Friday,
							StartTime = new DateTime(2017, 1, 1, 13, 0, 0), // 1 pm
							EndTime = new DateTime(2017, 1, 1, 13, 50, 0),
						};
						fall2017.CourseSections.Add(cecs274_11);

                        var cecs274_5 = new CourseSection() {
                            CatalogCourse = cecs274,
                            SectionNumber = 5,
                            Instructor = anthony,
                            MeetingDays = DaysOfWeek.Monday | DaysOfWeek.Wednesday,
                            StartTime = new DateTime(2017, 1, 1, 9, 30, 0),
                            EndTime = new DateTime(2017, 1, 1, 10, 20, 0)
                        };
                        fall2017.CourseSections.Add(cecs274_5);

                        var cecs174_99 = new CourseSection() {
                            CatalogCourse = cecs174,
                            SectionNumber = 99,
                            Instructor = neal,
                            MeetingDays = DaysOfWeek.Monday | DaysOfWeek.Wednesday,
                            StartTime = new DateTime(2017, 1, 1, 8, 0, 0),
                            EndTime = new DateTime(2017, 1, 1, 8, 50, 0)
                        };
                        spring2017.CourseSections.Add(cecs174_99);

                        var cecs228_99 = new CourseSection() {
                            CatalogCourse = cecs228,
                            SectionNumber = 99,
                            Instructor = anthony,
                            MeetingDays = DaysOfWeek.Friday,
                            StartTime = new DateTime(2017, 1, 1, 10, 0, 0),
                            EndTime = new DateTime(2017, 1, 1, 11, 50, 0)
                        };
                        spring2017.CourseSections.Add(cecs228_99);

                        // Add students
                        var thomas = new Student() {
                            FirstName = "Thomas",
                            LastName = "Brooks"
                        };
                        con.Students.Add(thomas);

                        var brianna = new Student() {
                            FirstName = "Brianna",
                            LastName = "Halberstadt"
                        };
                        con.Students.Add(brianna);

                        var thomas174 = new CourseGrade() {
                            Student = thomas,
                            CourseSection = cecs174_99,
                            Grade = Grade.A
                        };
                        thomas.Transcript.Add(thomas174);

                        var thomas228 = new CourseGrade() {
                            Student = thomas,
                            CourseSection = cecs228_99,
                            Grade = Grade.D
                        };
                        thomas.Transcript.Add(thomas228);

						con.SaveChanges();
						break;
					case 2:
						// Print all courses in the catalog
						foreach (var course in con.Courses.OrderBy(c => c.CourseNumber)) {
							Console.Write($"{course.DepartmentName} {course.CourseNumber}");
							if (course.Prerequisites.Count > 0) {
								Console.Write(" (Prerequisites: ");
								Console.Write(string.Join(", ", course.Prerequisites));
								Console.Write(")");
							}
							Console.WriteLine();
						}
						break;

					case 3:
						// Print all offered sections for Fall 2017
						var fallSem = con.SemesterTerms.Where(s => s.Name == "Fall 2017").FirstOrDefault();
						if (fallSem == null) {
							break;
						}

						Console.WriteLine($"{fallSem.Name}: {fallSem.StartDate.ToString("MMM dd")} - {fallSem.EndDate.ToString("MMM dd")}");
						foreach (var section in fallSem.CourseSections) {
							Console.WriteLine($"{section.CatalogCourse}-{section.SectionNumber.ToString("D2")} -- " +
								$"{section.Instructor.FirstName[0]} {section.Instructor.LastName} -- " +
								$"{section.MeetingDays}, {section.StartTime.ToShortTimeString()} to {section.EndTime.ToShortTimeString()}");
                            if (section.EnrolledStudents.Count > 0) {
                                foreach (Student s in section.EnrolledStudents) {
                                    Console.WriteLine($"{s.LastName}, {s.FirstName}; ");
                                }
                            }
						}
						break;

                    case 4:
                        // Print student details
                        Console.WriteLine("Enter in the student's name.");
                        string enteredName = Console.ReadLine();

                        string enteredFirst;
                        string enteredLast;

                        foreach (Student s in SOMEWHERE) {
                            if (s.FirstName.Equals(enteredFirst) && s.LastName.Equals(enteredLast)) {
                                Console.WriteLine($"{s.FirstName} {s.LastName}");

                                Console.Write("Transcript: ");
                                foreach (CourseGrade g in s.Transcript) {
                                    Console.Write($"CECS {g.CourseSection.CatalogCourse} ({g.Grade}),");
                                }
                                Console.WriteLine();

                                Console.Write("Enrolled: ");
                                foreach (CourseSection s in s.EnrolledCourses) {
                                    Console.Write($"CECS {s.CatalogCourse}-{s.CourseSection}");
                                }
                                Console.WriteLine();
                                break;
                            }
                        }
                        break;

                    case 5:
                        // Register for course
                        Console.WriteLine("Enter in the student's name.");
                        enteredName = Console.ReadLine();

                        //enteredFirst;
                        //enteredLast;

                        foreach (Student s in SOMEWHERE) {
                            if (s.FirstName.Equals(enteredFirst) && s.LastName.Equals(enteredLast)) {
                                Console.WriteLine("Enter in course number");
                                int courseNumber = Console.ReadLine();
                                Console.WriteLine();

                                Console.WriteLine("Enter in section number");
                                int sectionNumber = Console.ReadLine();
                                Console.WriteLine();

                                foreach (CourseSection sec in fall2017.CourseSections) {
                                    if (sec.CatalogCourse.Equals(courseNumber) && sec.SectionNumber.Equals(sectionNumber)) {
                                        if (s.CanRegisterForCourse(sec)) {
                                            sec.EnrolledStudents.Add(s);
                                            con.SaveChanges();
                                        }
                                        else {
                                            Console.WriteLine($"Error: {s.RegistrationResults}");
                                        }
                                    }
                                }
                            }
                        }
                        break;
				}
				Console.WriteLine();
				Console.WriteLine();

			} while (choice != 0);
		}

	}
}