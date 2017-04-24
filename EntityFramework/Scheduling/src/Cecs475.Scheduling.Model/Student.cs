using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cecs475.Scheduling.Model {
	[Flags]
	public enum RegistrationResults {
		Success,
		PrerequisiteNotMet,
		TimeConflict,
		AlreadyEnrolled,
		AlreadyCompleted
	}

	public class Student {
		public int Id { get; set; }
		public virtual List<CourseGrade> Transcript { get; set; }
		public virtual List<CourseSection> EnrolledCourses { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public RegistrationResults RegistrationResults { get; set; }
		// public virtual ICollection<CourseSection> CompletedCourses { get; set; } = new List<CourseSection>();

		public bool CanRegisterForCourseSection(CourseSection section) {
			bool canRegister = true;

			// CHECK: s is not already enrolled in another section of c's catalog course (ALREADY ENROLLED)
			foreach (CourseSection c in EnrolledCourses) {
				if (c.Equals(section)) {
					RegistrationResults = RegistrationResults.AlreadyEnrolled;
					canRegister = false;
					break;
				}
			}
			
			// CHECK: s has not passed c's catalog course in the past (ALREADY COMPLETED)
			foreach (CourseGrade g in Transcript) {
				if (g.CourseSection.Equals(section) && (int)g.Grade >= 2) {
					RegistrationResults = RegistrationResults.AlreadyCompleted;
					canRegister = false;
					break;
				}
			}

			// CHECK: s has passed all courses listed in c's prerequisites (PREREQUISITE NOT MET)
			foreach (CatalogCourse c in section.CatalogCourse.Prerequisites) {
				foreach (CourseGrade g in Transcript) {
					if (g.CourseSection.Equals(c)) {
						if ((int)g.Grade < 2) {
							RegistrationResults = RegistrationResults.PrerequisiteNotMet;
							canRegister = false;
							break;
						}
					}
				}
				if (!canRegister)
					break;
			}

			// CHECK: s is not currently enrolled in any course section that has a time conflict with c (TIME CONFLICT)
			foreach (CourseSection s in EnrolledCourses) {
				if (((byte)s.MeetingDays & (byte)section.MeetingDays) != 0) {
					RegistrationResults = RegistrationResults.TimeConflict;
					canRegister = false;
					break;
				}
			}

			if (canRegister)
				RegistrationResults = RegistrationResults.Success;

			return canRegister;
		}
	}
}
