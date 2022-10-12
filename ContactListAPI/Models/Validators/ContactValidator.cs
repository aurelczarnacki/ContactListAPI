using AutoMapper;
using FluentValidation;

namespace ContactListAPI.Models.Validators
{
    public class ContactValidator : AbstractValidator<Contact>
    {

        public ContactValidator(ContactListContext dbContext)
        {
            bool isSluzbowy = false;

            RuleFor(c => c.Name).NotEmpty().MaximumLength(20);
            RuleFor(c => c.Lastname).NotEmpty().MaximumLength(30);
            RuleFor(c => c.Email).NotEmpty().EmailAddress();
            RuleFor(c => c.Password).NotEmpty().MinimumLength(8)
                .Matches(@"(\d)+").WithMessage("Please use one or more digits.")
                .Matches("[A-Z]+").WithMessage("Please use one or more uppercase letters.")
                .Matches("[a-z]+").WithMessage("Please use one or more lowercase letters.");


            RuleFor(c => c.Category).Custom((value, context) =>
            {
                var isCategory = dbContext.Categories.Any(ca => ca.Name == value);
                if (value == "Służbowy")
                {
                    isSluzbowy = true;
                }

                if (!isCategory)
                {
                    context.AddFailure("Category", "Please use value from dictionary table.");
                }
            });

            RuleFor(c => c.SubCategory).Custom((value, context) =>
            {
                if (isSluzbowy)
                {

                    var isSubCategory = dbContext.SubCategories.Any(s => s.Name == value);
                    if (!isSubCategory)
                    {
                        context.AddFailure("SubCategory", "Please use value from dictionary table.");
                    }
                }
            });
        
    

            RuleFor(c => c.Email).Custom((value, context) =>
            {
                var isTaken = dbContext.Contacts.Any(e => e.Email == value);
                if (isTaken)
                {
                    context.AddFailure("Email", "That Email is taken.");
                }

            });

            RuleFor(c => c.Phone).MaximumLength(15);
        }
    }
}
