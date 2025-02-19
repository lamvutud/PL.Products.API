namespace PL.Products.API.Pdf.Pdf;

public interface IPdfWriter<T>
    where T : IPdfRequest
{
    Task WriteAsync(T data, Stream stream);

    Task<byte[]> GetBytesAsync(T data);
}

public interface IPdfRequest
{
}
