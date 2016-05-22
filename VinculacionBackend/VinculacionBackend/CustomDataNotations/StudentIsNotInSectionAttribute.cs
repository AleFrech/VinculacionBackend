using System.ComponentModel.DataAnnotations;
using System.Linq;
using VinculacionBackend.Data.Database;

namespace VinculacionBackend.CustomDataNotations
{
	public class StudentIsNotInSectionAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			if (value == null)
				return false;
			var sectionStudents = (SectionStudentModel)value;
			var context = new VinculacionContext();
			
			foreach (var student in sectionStudents.StudenstIds)
			{
				var sectionStudentRel = context.SectionUserRels.Include(x=>x.Student).Include(y=>y.Section).FirstOrDefault(z=>z.Student.StudentId == student && z.Section.Id == sectionStudents.SectionId);
				if (sectionStudentRel != null)
				{
					throw new Exception("El estudiante " + student + "Ya esta registrado en esta Seccion")
				}
			}
			
			return true;
		}
	}
}