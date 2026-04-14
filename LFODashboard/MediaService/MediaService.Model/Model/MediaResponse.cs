namespace MediaService.Model.Model
{
    public class MediaResponse
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; } 
    }
}
