namespace Voting.Common {
    public static class Constants {
        public const string NameExistsMessage = "Apologies, it appears that the {0} name you've entered is already taken. Would you mind trying another name?";
        public const string UserNotFoundMessage = "Apologies, we couldn't find any {0} with the details provided. Please double-check your information and try again.";
        public const string PoolingNotValidMessage = "Apologies, we're unable to proceed as the voter has already voted in the pool.";
        public const string PoolingUpdatedSuccessMessage = "Vote successfully updated";

        public const string Candidate = "Candidate";
        public const string Voter = "Voter";
        public const string UserAddedSuccessfullyMessage = "{0} added successfully";

        public const string InvalidIdMessage = "Invalid value is provided for argument {0}. Kindly verify and retry.";
    }

    public static class ErrorMessageConstants {
        public const string DbUpdateException = "There was an error saving your changes. Please check your input and try again.";
        public const string DbEntityValidationException = "Your changes couldn't be saved. Make sure all required fields are filled correctly";
        public const string OptimisticConcurrencyException = "The data you're trying to update has been changed by another user. Please review your changes and try again";
        public const string SqlException = "Oops! Something went wrong on our end. Our team is already investigating.";
        public const string TimeoutException = "We're experiencing high traffic right now. Please try your request again in a few minutes.";
    }
}