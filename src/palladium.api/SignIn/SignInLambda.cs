using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using palladium.api.Cognito;
using molybdenum.Logging;

namespace palladium.api.SignIn
{
    public class SignInLambda : molybdenum.BaseLambda, molybdenum.IAPIGatewayLambda
    {
        private readonly ICognitoUserStore cognitoUserStore;

        public SignInLambda()
        {
            var clientId = System.Environment.GetEnvironmentVariable("ClientId");
            var poolId = System.Environment.GetEnvironmentVariable("PoolId");
            cognitoUserStore = new CognitoUserStore(clientId, poolId, Amazon.RegionEndpoint.EUWest1);
        }

        public async Task<APIGatewayProxyResponse> Invoke(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                Logger = new Logger(context.Logger, "SignInLambda");

                var input = GetSafeRequestFromBody<SignInRequest>(request);
                Logger.Log($"signin username {input.Username}\n");
                Logger.Log($"signin password length {input.Password.Length}\n");
                Logger.Log($"ClientId {cognitoUserStore.ClientId}\n PoolId {cognitoUserStore.PoolId}\n");

                // Perform operation
                var authResponse = await cognitoUserStore.AuthAsync(input.Username, input.Password);

                var data = new
                {
                    accessToken = authResponse?.AuthenticationResult?.AccessToken,
                    idToken = authResponse?.AuthenticationResult?.IdToken
                };

                // Response
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
