import ApiClient from '../App/ApiClient';

class NewsClient extends ApiClient {

  static baseUri = '/api/news/';

  async listAsync() {
    const response = await super.fetchAsync(NewsClient.baseUri);
    return await response.json();
  }
}

const newsClient = new NewsClient();
export default function useNewsClient() {
  return newsClient;
}
