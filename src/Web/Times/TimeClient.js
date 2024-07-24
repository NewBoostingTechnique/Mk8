import ApiClient from '../App/ApiClient';

class TimeClient extends ApiClient {

  static baseUri = '/api/time/';

  async insertAsync(time) {
    const response = await super.fetchAsync(
      TimeClient.baseUri,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Accept": "application/json",
        },
        body: JSON.stringify(time),
      }
    );
    return await response.json();
  }
}

const timeClient = new TimeClient();
export default function useTimeClient() {
  return timeClient;
}
