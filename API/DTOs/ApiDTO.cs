namespace API.DTOs
{
    public record ResponseApi<T>(
        int code,
        string msg,
        T? data
        );

    public record PageData<T>(
        int Total,
        IEnumerable<T> Rows
        );

    public static class UsuarioAtual
    {

        public static long UserId { get; set; }
        public static string Token { get; set; }
        public static DateTime Expired { get; set; }
        public static bool IsValido(string? tokenEnviado) =>
            !string.IsNullOrWhiteSpace(Token) &&
            Token == tokenEnviado &&
            DateTime.Now <= Expired;
    }
}
