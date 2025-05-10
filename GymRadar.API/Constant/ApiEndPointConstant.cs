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
    }
}
