// This is an auto-generated file.


namespace SistemaOrbita.Web.Areas.Identity.Data
{

	public static class Permissions
	{



		public static List<string> GeneratePermissionsForModule(string module)
		{
			return new List<string>()
			{
					$"Permissions.{module}.View",
					$"Permissions.{module}.Upsert",
					$"Permissions.{module}.Delete",
			};
		}

		public static class Department
		{
			public const string View = "Permissions.Department.View";
			public const string Upsert = "Permissions.Department.Upsert";
			public const string Delete = "Permissions.Department.Delete";
		}
		public static class Employee
		{
			public const string View = "Permissions.Employee.View";
			public const string Upsert = "Permissions.Employee.Upsert";
			public const string Delete = "Permissions.Employee.Delete";
		}
		public static class EmployeeHistory
		{
			public const string View = "Permissions.EmployeeHistory.View";
			public const string Upsert = "Permissions.EmployeeHistory.Upsert";
			public const string Delete = "Permissions.EmployeeHistory.Delete";
		}
		public static class EmployeeProjectAssignment
		{
			public const string View = "Permissions.EmployeeProjectAssignment.View";
			public const string Upsert = "Permissions.EmployeeProjectAssignment.Upsert";
			public const string Delete = "Permissions.EmployeeProjectAssignment.Delete";
		}
		public static class EventType
		{
			public const string View = "Permissions.EventType.View";
			public const string Upsert = "Permissions.EventType.Upsert";
			public const string Delete = "Permissions.EventType.Delete";
		}
		public static class Gender
		{
			public const string View = "Permissions.Gender.View";
			public const string Upsert = "Permissions.Gender.Upsert";
			public const string Delete = "Permissions.Gender.Delete";
		}
		public static class Municipality
		{
			public const string View = "Permissions.Municipality.View";
			public const string Upsert = "Permissions.Municipality.Upsert";
			public const string Delete = "Permissions.Municipality.Delete";
		}
		public static class OrganizationDepartment
		{
			public const string View = "Permissions.OrganizationDepartment.View";
			public const string Upsert = "Permissions.OrganizationDepartment.Upsert";
			public const string Delete = "Permissions.OrganizationDepartment.Delete";
		}
		public static class Position
		{
			public const string View = "Permissions.Position.View";
			public const string Upsert = "Permissions.Position.Upsert";
			public const string Delete = "Permissions.Position.Delete";
		}
		public static class Project
		{
			public const string View = "Permissions.Project.View";
			public const string Upsert = "Permissions.Project.Upsert";
			public const string Delete = "Permissions.Project.Delete";
		}
	}

}



