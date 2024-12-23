import ApiClient from '../App/ApiClient';

class TimeClient extends ApiClient {

  static baseUri = '/api/times/';

  async createAsync(time) {
    return super.fetchAsync(
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
  }
}

const timeClient = new TimeClient();
export default function useTimeClient() {
  return timeClient;
}
