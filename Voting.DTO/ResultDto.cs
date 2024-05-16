
namespace Voting.DTO {
    public class ResultDto {
        public Result Result { get; set; } = new Result();
    }

    public class Result {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public List<string> Details { get; set; } = new List<string>();
    }

    public class BaseResultDto : BaseDto {
        public Result Result { get; set; } = new Result();
    }
}
