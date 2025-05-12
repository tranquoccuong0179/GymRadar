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
        }

        public static class PT
        {
            public const string PTEndPoint = ApiEndpoint + "/pt";
            public const string CreateNewPT = PTEndPoint;
        }
    }
}
