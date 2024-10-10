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
    let url = new URL(MigrationClient.baseUri, window.location.origin);
    Object.keys(after ?? {}).forEach(key => url.searchParams.append(`after.${key}`, after[key]));
    const response = await super.fetchAsync(url);
    return await response.json();
  }
}

const migrationClient = new MigrationClient();
export default function useMigrationClient() {
  return migrationClient;
}
