using FluentValidation;
using Habits.Communication.Requests.Summary;
using Habits.Exception;

namespace Habits.Application.UseCases.Summary
{
    public class SummaryDateRangeValidator : AbstractValidator<RequestSummaryDateRange>
    {
        public SummaryDateRangeValidator()
        {
            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage(ResourceErrorMessages.END_DATE_CANT_BE_MINOR_OF_START_DATE);
        }
    }
}
