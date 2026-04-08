using FluentValidation;

namespace CatalogAPI.Features.Games.GetByReleaseDateRange;

public class GetByReleaseDateValidator : AbstractValidator<GetByReleaseDateRangeQuery>
{
    public GetByReleaseDateValidator()
    {
        RuleFor(x => x.From)
            .LessThanOrEqualTo(x => x.To)
            .WithMessage("From date cannot be higher than To date");

        RuleFor(x => x.From)
            .GreaterThanOrEqualTo(DateTimeOffset.UtcNow.AddYears(-10).ToUnixTimeSeconds())
            .WithMessage("From date cannot be more than 10 years in past");

        RuleFor(x => x.To)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow.AddYears(5).ToUnixTimeSeconds())
            .WithMessage("To date cannot be more than 5 years in future");
    }
}