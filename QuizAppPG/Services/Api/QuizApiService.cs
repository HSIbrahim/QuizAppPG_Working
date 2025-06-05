namespace QuizAppPG.Services.Api
{
    public class QuizApiService : BaseApiService, IQuizApiService
    {
        public QuizApiService(HttpClient httpClient, ISecureStorageService secureStorageService)
            : base(httpClient, secureStorageService) { }

        public async Task<ServiceResult<List<QuizCategoryDto>>> GetQuizCategoriesAsync()
        {
            return await SendApiRequestAsync<List<QuizCategoryDto>>(HttpMethod.Get, "api/Quiz/categories");
        }

        public async Task<ServiceResult<List<QuestionDto>>> GetQuizQuestionsAsync(int categoryId, string difficulty, int count = 10)
        {
            return await SendApiRequestAsync<List<QuestionDto>>(HttpMethod.Get, $"api/Quiz/questions?categoryId={categoryId}&difficulty={difficulty}&count={count}");
        }

        public async Task<ServiceResult<AnswerResultDto>> SubmitSoloAnswerAsync(SubmitAnswerDto submitAnswerDto)
        {
            return await SendApiRequestAsync<AnswerResultDto>(HttpMethod.Post, "api/Quiz/submit-solo-answer", submitAnswerDto);
        }
    }
}