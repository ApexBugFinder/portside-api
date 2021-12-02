using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityModel.Client;
using System.Net.Http;
using System;
using IdentityModel;

namespace Portfolio.WebApp.Helpers {
  public class TokenValidator {
    public static async Task<TokenResponse> RequestTokenAsync()
    {
      var handler = new SocketsHttpHandler();
      var cert = new X509Certificate2("/https/mtls.pfx", "password");
      handler.SslOptions.ClientCertificates = new X509CertificateCollection { cert };

      HttpClient client = new HttpClient(handler);

      DiscoveryDocumentResponse discoveryDocument = await client.GetDiscoveryDocumentAsync();


      // To create a token you can use one of the following methods, which totally depends upon which grant type you are using for token generation.

      var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
      {
        Address = discoveryDocument
                          .TryGetValue(OidcConstants.Discovery.MtlsEndpointAliases)
                          .TryGetValue(OidcConstants.Discovery.TokenEndpoint)
                          .ToString(),

        ClientId = "mtls",
        Scope = "api1"
      });

      if (response.IsError) throw new Exception(response.Error);
      return response;
    }
  }
}