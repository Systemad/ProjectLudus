using FastEndpoints;
using FluentValidation;

namespace Me.Lists.Update;

public class UpdateListRequest
{
    public Guid ListId { get; set; }
    public string Name { get; set; }
    public bool Public { get; set; }
}

public class UpdateListValidator : Validator<UpdateListRequest>
{
    public UpdateListValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("List name is required!")
            .MinimumLength(5)
            .WithMessage("List name must be at least 5 characters!");

        RuleFor(x => x.Public).NotEmpty().WithMessage("a value must be set!");
    }
}
