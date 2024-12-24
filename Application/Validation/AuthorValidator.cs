using Domain.Models;
using FluentValidation;

namespace Application.Validation;

public class AuthorValidator : AbstractValidator<Author>
{
    public AuthorValidator()
    {
        RuleFor(author => author.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name cannot exceed 50 characters.");
        
        RuleFor(author => author.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(255).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(author => author.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.")
            .Must(date => DateTime.TryParse(date, out _)).WithMessage("Invalid date format.");

        RuleFor(author => author.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(255).WithMessage("Country name cannot exceed 1000 characters.");
    }
}
