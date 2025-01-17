import ApiClient from '../App/ApiClient';
import Time from './Time';

class TimeClient extends ApiClient {

  public static readonly baseUri = '/api/times/';

  async createAsync(time: Time) {
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
