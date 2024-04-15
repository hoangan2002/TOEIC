namespace ChillToeic.Models
{
	public class TestDetailViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int NumberOfQuestion { get; set; }
		public int NumberOfPart { get; set; }
		public int NumberOfUserTest { get; set; }
		public int Duration { get; set; }
		public int? EducationCenterId { get; set; }
		
		public int Score { get; set; }
		public Boolean Status { get; set; }
		public DateTime CreatedAt { get; set; }

		public string EducationCenterName { get; set; } // Tên của EducationCenter
		public string CourseName { get; set; } // Tên của EducationCenter
	}
}