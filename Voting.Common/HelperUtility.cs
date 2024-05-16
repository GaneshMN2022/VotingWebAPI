using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voting.Common {
    public static class HelperUtility {
        public static string GetCombinedName(string? firstName, string? lastName) {
            if (string.IsNullOrWhiteSpace(lastName)) return firstName ?? "";

            return firstName + " " + lastName;
        }
    }
}
