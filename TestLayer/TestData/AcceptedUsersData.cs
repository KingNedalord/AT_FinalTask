using System.Collections.Generic;

namespace TestLayer.TestData
{
    public static class AcceptedUsersData
    {
        public static IEnumerable<object[]> GetAcceptedUsers()
        {
            // the site lists: standard_user, locked_out_user, problem_user, performance_glitch_user
            // For valid login we'll use standard_user, problem_user and performance_glitch_user
            yield return new object[] { "standard_user" };
            yield return new object[] { "problem_user" };
            yield return new object[] { "performance_glitch_user" };
        }
    }
}
