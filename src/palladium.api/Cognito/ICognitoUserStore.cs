using System;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;

namespace palladium.api.Cognito
{
    public interface ICognitoUserStore
    {
        Task<SignUpResponse> CreateAsync(CognitoUser user);
        Task<InitiateAuthResponse> AuthAsync(string username, string password);
        string PoolId { get; }
        string ClientId { get; }
    }

}
