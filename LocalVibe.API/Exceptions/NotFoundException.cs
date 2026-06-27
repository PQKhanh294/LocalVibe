namespace LocalVibe.API.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string entityName, int id)
        : base($"{entityName} với Id = {id} không tìm thấy.") { }
}
