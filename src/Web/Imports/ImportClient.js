import ApiClient from '../App/ApiClient'

class ImportClient extends ApiClient {
  static baseUri = '/api/imports/';

  async detailAsync(id) {
    const response = await super.fetchAsync(`${ImportClient.baseUri}${id}/`);
    return (response.status >= 200 && response.status <= 300)
      ? await response.json()
      : null;
  }

  async insertAsync($import) {
    const response = await super.fetchAsync(
      ImportClient.baseUri,
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
}

const importClient = new ImportClient();
export default function useImportClient() {
  return importClient;
}
