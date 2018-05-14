using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

namespace palladium.api.Cognito
{
    public class CognitoUserStore : ICognitoUserStore
    {
        public readonly string _clientId;
        public readonly string _poolId;
        public readonly Amazon.RegionEndpoint _region;

        public CognitoUserStore(string clientId, string poolId, Amazon.RegionEndpoint region)
        {
            _clientId = clientId;
            _poolId = poolId;
            _region = region;
        }

        public string PoolId => _poolId;
        public string ClientId => _clientId;

        /// <summary>
        /// Create a new Cognito user.
        /// </summary>
        /// <returns></returns>
        public Task<SignUpResponse> CreateAsync(CognitoUser user)
        {
            var signUpRequest = new SignUpRequest
            {
                ClientId = _clientId,
                Password = user.Password,
                Username = user.Email,
            };

            var emailAttribute = new AttributeType
            {
                Name = "email",
                Value = user.Email
            };
            signUpRequest.UserAttributes.Add(emailAttribute);

            using (var client = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Amazon.RegionEndpoint.EUWest1))
            {
                return client.SignUpAsync(signUpRequest);
            }
        }

        /// <summary>
        /// Create an auth token
        /// </summary>
        /// <returns></returns>
        public Task<InitiateAuthResponse> AuthAsync(string username, string password)
        {
            var initiateAuthRequest = new InitiateAuthRequest
            {
                ClientId = _clientId,
                AuthFlow = Amazon.CognitoIdentityProvider.AuthFlowType.USER_PASSWORD_AUTH,
                AuthParameters = new Dictionary<string, string>()
                {
                    { "USERNAME", username },
                    { "PASSWORD", password },
                }
            };

            using (var client = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), _region))
            {
                return client.InitiateAuthAsync(initiateAuthRequest);
            }
        }

    }

}