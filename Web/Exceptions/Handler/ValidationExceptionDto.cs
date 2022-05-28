using FluentValidation;

namespace Web.Exceptions.Handler;

public class ValidationExceptionDto
{
    public string Key { get; set; }
    public IEnumerable<ValidationMessage> Messages { get; set; }

    public static ValidationExceptionDto From(ValidationException exception) =>
        new()
        {
            Key = exception.GetType().Name,
            Messages = exception.Errors
                                .Select(x => new ValidationMessage
                                {
                                    Code = x.ErrorCode,
                                    Message = x.ErrorMessage,
                                    Field = x.PropertyName,
                                    Severity = x.Severity,
                                })
                                .ToList()
        };
}

public class ValidationMessage
{
    public string Code { get; set; }
    public string Message { get; set; }
    public string Field { get; set; }
    public Severity Severity { get; set; }
}