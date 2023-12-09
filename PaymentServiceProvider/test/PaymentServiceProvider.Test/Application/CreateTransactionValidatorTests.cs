using FluentValidation.TestHelper;
using PaymentServiceProvider.Application.Validators;
using PaymentServiceProvider.Infrastructure.Dto.Transaction;

namespace PaymentServiceProvider.Test.Application
{
    public class CreateTransactionValidatorTests
    {
        private readonly CreateTransactionValidator _createTransactionValidator;

        public CreateTransactionValidatorTests()
        {
            _createTransactionValidator = new CreateTransactionValidator();
        }

        [InlineData("debit_card")]
        [InlineData("credit_card")]
        [Theory(DisplayName = "CreateTransactionValidator: Should return valid")]
        public void CreateTransactionValidator_Should_Return_Valid(string paymentMethod)
        {
            //Arrange
            var createTransactionDto = new CreateTransactionDto(
                2313,
                "Smartband XYZ 3.0",
                paymentMethod,
                "4970110000000062",
                "cardHolderName",
                DateTime.Now.AddYears(1).ToString(),
                "145");

            //Assert
            var createTransactionValidatorResponse = _createTransactionValidator.TestValidate(createTransactionDto);

            //Act
            createTransactionValidatorResponse.ShouldNotHaveAnyValidationErrors();
        }

        [Fact(DisplayName = "CreateTransactionValidator: Should return invalid when TransactionValue is zero")]
        public void CreateTransactionValidator_Should_Return_Invalid_When_TransactionValue_Is_Zero()
        {
            //Arrange
            var transactionValue = 0;
            var createTransactionDto = new CreateTransactionDto(
                transactionValue,
                "Smartband XYZ 3.0",
                "debit_card",
                "4970110000000062",
                "cardHolderName",
                DateTime.Now.AddYears(3).Date.ToString(),
                "145");

            //Assert
            var createTransactionValidatorResponse = _createTransactionValidator.TestValidate(createTransactionDto);

            //Act
            createTransactionValidatorResponse
                .ShouldHaveValidationErrorFor(prop => prop.TransactionValue)
                .WithErrorCode("400")
                .WithErrorMessage("O valor da transação é obrigatório.E deve ser maior que zero.");
        }

        [Fact(DisplayName = "CreateTransactionValidator: Should return invalid when CardNumber is between 13 end 19")]
        public void CreateTransactionValidator_Should_Return_Invalid_When_CardNumber_Is_Between_13_And_19()
        {
            //Arrange
            var cardNumber = "12345";
            var createTransactionDto = new CreateTransactionDto(
                1233,
                "Smartband XYZ 3.0",
                "debit_card",
                cardNumber,
                "cardHolderName",
                DateTime.Now.AddYears(3).Date.ToString(),
                "145");

            //Assert
            var createTransactionValidatorResponse = _createTransactionValidator.TestValidate(createTransactionDto);

            //Act
            createTransactionValidatorResponse
                .ShouldHaveValidationErrorFor(prop => prop.CardNumber)
                .WithErrorCode("400")
                .WithErrorMessage("O número do cartão é obrigatório. E deve conter de 13 a 19 digitos.");
        }

        [Fact(DisplayName = "CreateTransactionValidator: Should return invalid when CodeCVC is between 3 end 4")]
        public void CreateTransactionValidator_Should_Return_Invalid_When_CodeCVC_Is_Between_3_And_4()
        {
            //Arrange
            var codeCVC = "145146";
            var createTransactionDto = new CreateTransactionDto(
                32421,
                "Smartband XYZ 3.0",
                "debit_card",
                "4970110000000062",
                "cardHolderName",
                DateTime.Now.AddYears(3).Date.ToString(),
                codeCVC);

            //Assert
            var createTransactionValidatorResponse = _createTransactionValidator.TestValidate(createTransactionDto);

            //Act
            createTransactionValidatorResponse
                .ShouldHaveValidationErrorFor(prop => prop.CodeCVC)
                .WithErrorCode("400")
                .WithErrorMessage("O codigo cvc do cartão é obrigatório.E deve conter de 3 a 4 digitos.");
        }

        [Fact(DisplayName = "CreateTransactionValidator: Should return invalid when ExpirationDate is greater than today")]
        public void CreateTransactionValidator_Should_Return_Invalid_When_ExpirationDate_Is_Greater_Than_Today()
        {
            //Arrange
            var expirationDate = DateTime.Now.AddYears(-1).Date.ToString();
            var createTransactionDto = new CreateTransactionDto(
                3453,
                "Smartband XYZ 3.0",
                "debit_card",
                "4970110000000062",
                "cardHolderName",
                expirationDate,
                "145");

            //Assert
            var createTransactionValidatorResponse = _createTransactionValidator.TestValidate(createTransactionDto);

            //Act
            createTransactionValidatorResponse
                .ShouldHaveValidationErrorFor(prop => prop.ExpirationDate)
                .WithErrorCode("400")
                .WithErrorMessage("A data de expiração do cartão é obrigatória. E deve estar valida.");
        }

        [Fact(DisplayName = "CreateTransactionValidator: Should return invalid when PaymentMethod is not debit_card or credit_card")]
        public void CreateTransactionValidator_Should_Return_Invalid_When_PaymentMethod_Is_Invalid()
        {
            //Arrange
            var createTransactionDto = new CreateTransactionDto(
                1234123,
                "Smartband XYZ 3.0",
                "wrongPaymentMethod",
                "4970110000000062",
                "cardHolderNameName",
                DateTime.Now.AddYears(3).Date.ToString(),
                "145");

            //Assert
            var createTransactionValidatorResponse = _createTransactionValidator.TestValidate(createTransactionDto);

            //Act
            createTransactionValidatorResponse
                .ShouldHaveValidationErrorFor(prop => prop.PaymentMethod)
                .WithErrorCode("400")
                .WithErrorMessage("O método de pagamento é obrigatório. E deve ser debit_card ou credit_card.");
        }

    }
}
