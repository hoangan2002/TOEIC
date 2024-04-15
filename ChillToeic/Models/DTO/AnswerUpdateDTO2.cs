namespace ChillToeic.Models.DTO
{
	public class AnswerUpdateDTO2
	{
		public string QuestionIdAndDetailId { get; set; }
		public int AnswerValue { get; set; }
		public IFormFile? ImgForm { get; set; }
		public string Answer {  get; set; }
	}
}
