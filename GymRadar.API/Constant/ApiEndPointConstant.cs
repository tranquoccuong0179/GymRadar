using System.Runtime.CompilerServices;

namespace GymRadar.API.Constant
{
    public static class ApiEndPointConstant
    {
        static ApiEndPointConstant()
        {
        }

        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public static class Account
        {
            public const string AccountEndPoint = ApiEndpoint + "/account";
            public const string RegisterAccount = AccountEndPoint;
        }

        public static class Authentication
        {
            public const string AuthEndPoint = ApiEndpoint + "/auth";
            public const string Auth = AuthEndPoint;
        }

        public static class Gym
        {
            public const string GymEndPoint = ApiEndpoint + "/gym";
            public const string CreateNewGym = GymEndPoint;
            public const string GetAllGym = GymEndPoint;
            public const string GetGym = GymEndPoint + "/{id}";
            public const string GetAllPT = GymEndPoint + "/{id}/pts";
        }

        public static class PT
        {
            public const string PTEndPoint = ApiEndpoint + "/pt";
            public const string CreateNewPT = PTEndPoint;
            public const string GetAllPT = PTEndPoint;
        }

        public static class Admin
        {
            public const string AdminEndPoint = ApiEndpoint + "/admin";
            public const string GetAllPT = AdminEndPoint + "/get-pt";
        }

    }
}
