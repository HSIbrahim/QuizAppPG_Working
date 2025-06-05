namespace QuizAppPG.Services.Api
{
    public interface IQuizApiService
    {
        Task<ServiceResult<List<QuizCategoryDto>>> GetQuizCategoriesAsync();
        Task<ServiceResult<List<QuestionDto>>> GetQuizQuestionsAsync(int categoryId, string difficulty, int count = 10);
        Task<ServiceResult<AnswerResultDto>> SubmitSoloAnswerAsync(SubmitAnswerDto submitAnswerDto);
    }
}