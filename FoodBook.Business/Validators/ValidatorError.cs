namespace FoodBook.Business.Validators
{
    public class ValidatorError
    {
        string message = "";
        public string GetErrorMessagesAsString(System.Collections.Generic.IList<FluentValidation.Results.ValidationFailure> errors)
        {
            foreach (var error in errors)
            {
                message += $"{error.ErrorMessage} ";
            }
            return message;
        }   
    }
}