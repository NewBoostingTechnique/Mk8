import ApiClient from '../App/ApiClient';

class NewClient extends ApiClient {

  static baseUri = '/api/news/';

  async indexAsync() {
    const response = await super.fetchAsync(NewClient.baseUri);
    return await response.json();
  }

  async migrateAsync() {
    const response = await super.fetchAsync(`${NewClient.baseUri}migrate/`, { method: 'POST' });
    return await response.json();
  }
}

const newClient = new NewClient();
export default function useNewClient() {
  return newClient;
}
