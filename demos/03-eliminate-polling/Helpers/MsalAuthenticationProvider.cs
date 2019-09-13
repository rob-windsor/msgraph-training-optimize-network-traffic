using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Graph;

namespace Helpers
{
  public class MsalAuthenticationProvider : IAuthenticationProvider
  {
    private IPublicClientApplication _clientApplication;
    private string[] _scopes;
    private string _username;
    private SecureString _password;
    private string _accessToken;

    public MsalAuthenticationProvider(IPublicClientApplication clientApplication, string[] scopes, string username, SecureString password)
    {
      _clientApplication = clientApplication;
      _scopes = scopes;
      _username = username;
      _password = password;
      _accessToken = null;
    }

    public async Task AuthenticateRequestAsync(HttpRequestMessage request)
    {
      if (string.IsNullOrEmpty(_accessToken))
      {
        _accessToken = await GetTokenAsync();
      }

      request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _accessToken);
    }

    public async Task<string> GetTokenAsync()
    {
      AuthenticationResult authResult = null;
      authResult = await _clientApplication.AcquireTokenByUsernamePassword(_scopes, _username, _password).ExecuteAsync();
      return authResult.AccessToken;
    }
  }
}