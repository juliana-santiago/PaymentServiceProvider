using FluentValidation;
using PaymentServiceProvider.Infrastructure.Dto.Transaction;
using PaymentServiceProvider.Util.Utils;

namespace PaymentServiceProvider.Application.Validators
{
    public class CreateTransactionValidator : AbstractValidator<CreateTransactionDto>
    {

        public CreateTransactionValidator()
        {
            var errorCode = "400";

            RuleFor(customer => customer.TransactionValue)
                .NotNull()
                .ExclusiveBetween(0, 10000000000M)
                .WithErrorCode(errorCode)
                .WithMessage("O valor da transação é obrigatório.E deve ser maior que zero.");

            RuleFor(customer => customer.TransactionDescription)
                .NotNull()
                .NotEmpty()
                .WithErrorCode(errorCode)
                .WithMessage("A descrição da transação é obrigatória.");

            RuleFor(customer => customer.PaymentMethod)
                .NotNull()
                .NotEmpty()
                .Matches(@"^(debit_card|credit_card)$")
                .WithErrorCode(errorCode)
                .WithMessage("O método de pagamento é obrigatório. E deve ser debit_card ou credit_card.");

            RuleFor(customer => customer.CardHolderName)
                .NotNull()
                .NotEmpty()
                .WithErrorCode(errorCode)
                .WithMessage("O nome do portador do cartão é obrigatório.");

            RuleFor(customer => customer.CardNumber)
                .NotNull()
                .NotEmpty()
                .CreditCard()
                .WithErrorCode(errorCode)
                .WithMessage("O número do cartão é obrigatório. E deve conter de 13 a 19 digitos.");

            RuleFor(customer => customer.CodeCVC)
                .NotEmpty()
                .NotNull()
                .Matches(@"^\d{3,4}$")
                .WithErrorCode(errorCode)
                .WithMessage("O codigo cvc do cartão é obrigatório.E deve conter de 3 a 4 digitos.");

            RuleFor(customer => customer.ExpirationDate)
                .NotEmpty()
                .Must(ValidateDate)
                .WithErrorCode(errorCode)
                .WithMessage("A data de expiração do cartão é obrigatória. E deve estar valida.");
        }

        private bool ValidateDate(string? date)
        {
            return date?.ParseToDate().Date >= DateTime.Now.Date;
        }
    }
}
