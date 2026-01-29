using FastEndpoints;
using FluentValidation;

namespace Ludus.Lists.Features.CreateList;

public class CreateListValidator : Validator<CreateListRequest>
{
    public CreateListValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("List name is required!")
            .MinimumLength(5)
            .WithMessage("List name must be at least 5 characters!");

        RuleFor(x => x.Public).NotEmpty().WithMessage("a value must be set!");
    }
}
