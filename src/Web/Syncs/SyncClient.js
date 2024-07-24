import ApiClient from '../App/ApiClient'

class SyncClient extends ApiClient {
  static baseUri = '/api/syncs/';

  async detailAsync(syncId) {
    const response = await super.fetchAsync(`${SyncClient.baseUri}${syncId}/`);
    return (response.status >= 200 && response.status <= 300)
      ? await response.json()
      : null;
  }

  async insertAsync(sync) {
    const response = await super.fetchAsync(
      SyncClient.baseUri,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Accept": "application/json",
        },
        body: JSON.stringify(sync),
      }
    );
    return await response.json();
  }
}

const syncClient = new SyncClient();
export default function useSyncClient() {
  return syncClient;
}
