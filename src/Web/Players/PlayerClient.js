import ApiClient from '../App/ApiClient';

class PlayerClient extends ApiClient {

  static baseUri = '/api/players/';

  async createAsync(player) {
    const response = await super.fetchAsync(
      PlayerClient.baseUri,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Accept": "application/json",
        },
        body: JSON.stringify(player),
      }
    );
    return await response.json();
  }

  deleteAsync(playerName) {
    return super.fetchAsync(
      `${PlayerClient.baseUri}${playerName}/`,
      {
        method: "DELETE"
      }
    );
  }

  async detailAsync(playerName) {
    const response = await super.fetchAsync(`${PlayerClient.baseUri}${playerName}/`);
    return (response.status >= 200 && response.status <= 300)
      ? await response.json()
      : null;
  }

  async indexAsync() {
    const response = await super.fetchAsync(PlayerClient.baseUri);
    return await response.json();
  }

  async migrateAsync() {
    const response = await super.fetchAsync(`${PlayerClient.baseUri}migrate/`, { method: "POST" });
    return await response.json();
  }
}

const playerClient = new PlayerClient();
export default function usePlayerClient() {
  return playerClient;
}
