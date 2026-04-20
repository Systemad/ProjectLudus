namespace CatalogAPI.Features.Games.GetByReleaseDateRange;

public class GetByReleaseDateValidator : AbstractValidator<GetByReleaseDateRangeQuery>
{
    public GetByReleaseDateValidator()
    {
        RuleFor(x => x.Start)
            .LessThanOrEqualTo(x => x.End)
            .WithMessage("Start date cannot be after End date");

        RuleFor(x => x.Start)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-10)))
            .WithMessage("Start date cannot be more than 10 years in the past");

        RuleFor(x => x.End)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(5)))
            .WithMessage("End date cannot be more than 5 years in the future");
    }
}
