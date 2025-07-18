using FluentValidation.Results;

namespace Domain.Exceptions;

public class ValidationException : Exception
{
    public List<ValidationFailure> Errors { get; }

    public ValidationException(List<ValidationFailure> errors) : base("validation failed")
    {
        Errors = errors;
    }
}