using QuizAppPG.DTOs; // Corrected namespace: QuizCategoryDto, QuestionDto, AnswerResultDto, SubmitAnswerDto, ErrorDto, ServiceResult
using QuizAppPG.Services.Local;
using System.Net.Http; // For HttpMethod, HttpClient
using System.Collections.Generic; // For List

namespace QuizAppPG.Services.Api
{
    public class QuizApiService : BaseApiService, IQuizApiService
    {
        public QuizApiService(HttpClient httpClient, ISecureStorageService secureStorageService)
            : base(httpClient, secureStorageService) { }

        public async Task<ServiceResult<List<QuizCategoryDto>>> GetQuizCategoriesAsync()
        {
            // Corrected: Calls SendApiRequestAsync with ONE generic type argument <List<QuizCategoryDto>>
            return await SendApiRequestAsync<List<QuizCategoryDto>>(HttpMethod.Get, "api/Quiz/categories");
        }

        public async Task<ServiceResult<List<QuestionDto>>> GetQuizQuestionsAsync(int categoryId, string difficulty, int count = 10)
        {
            // Corrected: Calls SendApiRequestAsync with ONE generic type argument <List<QuestionDto>>
            return await SendApiRequestAsync<List<QuestionDto>>(HttpMethod.Get, $"api/Quiz/questions?categoryId={categoryId}&difficulty={difficulty}&count={count}");
        }

        public async Task<ServiceResult<AnswerResultDto>> SubmitSoloAnswerAsync(SubmitAnswerDto submitAnswerDto)
        {
            // Corrected: Calls SendApiRequestAsync with ONE generic type argument <AnswerResultDto>
            return await SendApiRequestAsync<AnswerResultDto>(HttpMethod.Post, "api/Quiz/submit-solo-answer", submitAnswerDto);
        }
    }
}