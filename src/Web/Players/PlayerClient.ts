import ApiClient from '../App/ApiClient';
import Player from './Player';

class PlayerClient extends ApiClient {

  private static readonly baseUri = '/api/players/';

  async createAsync(player: Player) {
    return super.fetchAsync(
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
  }

  deleteAsync(playerName: string) {
    return super.fetchAsync(
      `${PlayerClient.baseUri}${playerName}/`,
      {
        method: "DELETE"
      }
    );
  }

  async detailAsync(playerName: string) {
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
