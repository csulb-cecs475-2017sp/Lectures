using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cecs475.Scheduling.Model {
	[Flags]
	public enum Grade {
		F = 0,
		D = 1,
		C = 2,
		B = 3,
		A = 4
	}

	public class CourseGrade {
		public int Id { get; set; }
		public virtual Student Student { get; set; }
		public virtual CourseSection CourseSection { get; set; }

		public Grade Grade { get; set; }
	}
}
