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
}
