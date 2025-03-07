namespace CustomTestCreator.Clients.Infrastructure.Options
{
    public class MinioOptions
    {
        public string Endpoint { get; set; } = default!;
        public string AccessKey { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public bool WithSsl { get; init; } = false;
    }
}