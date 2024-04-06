import ApiClient from '../App/ApiClient';

class AuthorizationClient extends ApiClient {
  static baseUrl = '/api/authorization/';

  async getAsync() {
    const response = await super.fetchAsync(AuthorizationClient.baseUrl);
    return (await response.text()) === 'true';
  }

  postAsync() {
    return super.fetchAsync(
      AuthorizationClient.baseUrl,
      {
        method: "POST"
      }
    );
  }
}

const authorizationClient = new AuthorizationClient();
export default function useAuthorizationClient() {
  return authorizationClient;
}