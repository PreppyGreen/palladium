using System;
using System.Collections.Generic;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

namespace palladium.api.Cognito
{
    public class CognitoUser
    {
        public string Password { get; set; }
        public UserStatusType Status { get; set; }
        public string Email { get; set; }
    }

}
