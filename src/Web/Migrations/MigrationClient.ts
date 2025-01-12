import ApiClient from '../App/ApiClient'

class MigrationClient extends ApiClient {
  private static readonly baseUri = '/api/migrations/';

  async createAsync() {
    const response = await super.fetchAsync(
      MigrationClient.baseUri,
      {
        method: "POST"
      }
    );
    return await response.json();
  }

  async detailAsync(id: string) {
    const response = await super.fetchAsync(`${MigrationClient.baseUri}${id}/`);
    return (response.status >= 200 && response.status <= 300)
      ? await response.json()
      : null;
  }

  async indexAsync(after: { [key: string]: string } | null = null) {
    let url = new URL(MigrationClient.baseUri, window.location.origin);
    Object.keys(after ?? {}).forEach(key => url.searchParams.append(`after.${key}`, after![key]));
    const response = await super.fetchAsync(url);
    return await response.json();
  }
}

const migrationClient = new MigrationClient();
export default function useMigrationClient() {
  return migrationClient;
}
