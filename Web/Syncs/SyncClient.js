import ApiClient from '../App/ApiClient'

class SyncClient extends ApiClient {
  static baseUri = '/api/sync/';

  async insertAsync() {
    return super.fetchAsync(
      SyncClient.baseUri,
      {
        method: "POST"
      }
    );
  }
}

const syncClient = new SyncClient();
export default function useSyncClient() {
  return syncClient;
}