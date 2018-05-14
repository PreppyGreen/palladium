using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using palladium.api;

namespace palladium.api.Tests
{
    public class RegisterTests
    {
        public RegisterTests()
        {
        }

        [Fact]
        public async Task RegisterLambda_WithInvalidEmail_Should400()
        {
            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;


            Register.RegisterLambda lambda = new Register.RegisterLambda();


            request = new APIGatewayProxyRequest();
            context = new TestLambdaContext();
            response = await lambda.Invoke(request, context);
            Assert.Equal(400, response.StatusCode);
            Assert.Contains("Email invalid", response.Body);
        }
    }
}
