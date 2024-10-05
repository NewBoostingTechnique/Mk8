import ApiClient from '../App/ApiClient'

class MigrationClient extends ApiClient {
  static baseUri = '/api/migrations/';

  async createAsync($import) {
    const response = await super.fetchAsync(
      MigrationClient.baseUri,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Accept": "application/json",
        },
        body: JSON.stringify($import),
      }
    );
    return await response.json();
  }

  async detailAsync(id) {
    const response = await super.fetchAsync(`${MigrationClient.baseUri}${id}/`);
    return (response.status >= 200 && response.status <= 300)
      ? await response.json()
      : null;
  }

  async indexAsync(after) {
    let url = MigrationClient.baseUri;
    if (after)
      url += `?after=${JSON.stringify(after)}`;
    const response = await super.fetchAsync(url);
    return await response.json();
  }
}

const migrationClient = new MigrationClient();
export default function useMigrationClient() {
  return migrationClient;
}
