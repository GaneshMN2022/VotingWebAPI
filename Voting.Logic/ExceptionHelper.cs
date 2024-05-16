using Microsoft.EntityFrameworkCore;
using Voting.Common;

namespace Voting.Logic {
    public static class ExceptionHelper {

        public static bool IsEmptyOrNull<T>(T data) {
            if(data is null) return true;

            return false;
        }

        public static Exception ProcessException(Exception exception) {
            if (exception is DbUpdateException) return new Exception(ErrorMessageConstants.DbUpdateException);
            if (exception is DbUpdateException) return new Exception(ErrorMessageConstants.DbEntityValidationException);
            if (exception is DbUpdateException) return new Exception(ErrorMessageConstants.SqlException);
            if (exception is DbUpdateException) return new Exception(ErrorMessageConstants.OptimisticConcurrencyException);
            if (exception is DbUpdateException) return new Exception(ErrorMessageConstants.TimeoutException);
            return exception;
        }
    }
}
