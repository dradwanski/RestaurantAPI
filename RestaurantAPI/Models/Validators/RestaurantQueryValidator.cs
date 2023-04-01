using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        private string[] allowedSortByColumnNames = new[]
        {
            nameof(Restaurant.Name),
            nameof(Restaurant.Description),
            nameof(Restaurant.Category),
        };
        public RestaurantQueryValidator()
        {
            RuleFor(r => r.pageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.pageSize).Custom((value, context) =>{
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("pageSize", $"PageSize must be in [{string.Join(", ",allowedPageSizes)}]");
                }
            });
            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}
