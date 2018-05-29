using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using palladium.api.Cognito;
using molybdenum.Logging;

namespace palladium.api.Register
{
    public class RegisterLambda : molybdenum.BaseLambda, molybdenum.IAPIGatewayLambda
    {
        private readonly ICognitoUserStore cognitoUserStore;

        public RegisterLambda()
        {
            var clientId = System.Environment.GetEnvironmentVariable("ClientId");
            var poolId = System.Environment.GetEnvironmentVariable("PoolId");
            cognitoUserStore = new CognitoUserStore(clientId, poolId, Amazon.RegionEndpoint.EUWest1);
        }

        public async Task<APIGatewayProxyResponse> Invoke(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                Logger = new Logger(context.Logger, "RegisterLambda");

                var input = GetSafeRequestFromBody<RegisterRequest>(request);
                Logger.Log($"register email {input.Email}\n");
                Logger.Log($"register password length {input.Password?.Length}\n");
                Logger.Log($"clientId {cognitoUserStore.ClientId}\n");

                if (string.IsNullOrEmpty(input.Email) == true)
                {
                    return new molybdenum.BadRequestResponse(new string[] { "Email invalid" })
                        .AutoLog(Logger)
                        .Response;
                }

                // Perform service
                var cognitoUser = new CognitoUser()
                {
                    Email = input.Email,
                    Password = input.Password
                };
                
                var signUpResponse = await cognitoUserStore.CreateAsync(cognitoUser);

                var data = new
                {
                    isSuccess = true,
                    userConfirmed = signUpResponse.UserConfirmed,
                    accountId = signUpResponse.UserSub
                };

                //Response
                return new molybdenum.JsonResponse(data)
                    .AutoLog(Logger)
                    .Response;
            }

            catch (Exception ex)
            {
                return new molybdenum.ErrorResponse(ex)
                    .AutoLog(Logger)
                    .Response;
            }
        }

    }
}
