import ApiClient from '../App/ApiClient';

class NewClient extends ApiClient {

  static baseUri = '/api/news/';

  async listAsync() {
    const response = await super.fetchAsync(NewClient.baseUri);
    return await response.json();
  }
}

const newClient = new NewClient();
export default function useNewClient() {
  return newClient;
}
