using FluentValidation;
using Domain.Entities.Models;

namespace Application.Validation;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(book => book.ISBN)
            .NotEmpty().WithMessage("ISBN is required.")
            .Length(13).WithMessage("ISBN must be exactly 13 characters.");
        
        RuleFor(book => book.BookTitle)
            .NotEmpty().WithMessage("Book title is required.")
            .MaximumLength(50).WithMessage("Book title cannot exceed 50 characters.");

        RuleFor(book => book.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(book => book.AuthorId)
            .NotEmpty().WithMessage("Author ID is required.");

        RuleFor(book => book.Amount)
            .GreaterThanOrEqualTo(0).WithMessage("Amount must be non-negative.")
            .Must(BePositiveNumber).WithMessage("Amount must be a valid number.");
    }
    
    private bool BePositiveNumber(int amount)
    {
        return amount >= 0;
    }
}
