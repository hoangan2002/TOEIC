namespace ChillToeic.Models.DTO
{
	public class QuestionDetailDTO
	{
		public int? QuestionNumber { get; set; }
		public string? ImagePath { get; set; }
		public string? Content { get; set; }
		public string? OptionA { get; set; }
		public string? OptionB { get; set; }
		public string? OptionC { get; set; }
		public string? OptionD { get; set; }
		public string? CorrectAnswer { get; set; }
	}
}
